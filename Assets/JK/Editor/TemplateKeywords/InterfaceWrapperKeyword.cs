using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Editor.TemplateKeywords
{
    [Serializable]
    public class InterfaceWrapperKeyword : ITemplateKeyword
    {
        public string Keyword => "#INTERFACE_WRAPPER#";

        public string ReplaceInFile(string path, string file)
        {
            if (!file.Contains(Keyword))
                return file;

            string fileName = Path.GetFileName(path);

            string firstChar = "w";

            if (fileName[0].ToString().ToLower() != firstChar)
            {
                Debug.LogError($"{nameof(InterfaceWrapperKeyword)} expected file name to start with {firstChar}, but found {fileName}");
                return file;
            }

            string directoryPath = TemplateUtils.ExtractDirectoryPath(path);
            string relativePath = TemplateUtils.ExtractPathUnderAssets(directoryPath);
            string namespaceName = TemplateUtils.RemoveFoldersInPath(relativePath, TemplateUtils.CommonSilentFolders);

            namespaceName = namespaceName.Replace("/", ".");

            string interfaceName = fileName.Substring(1, fileName.Length - 4);

            string interfaceFullName = $"{namespaceName}.{interfaceName}";

            Type interfaceType = Type.GetType(interfaceFullName + ", Assembly-CSharp")
                ?? Type.GetType(interfaceName + ", Assembly-CSharp")
                ?? Type.GetType(interfaceFullName)
                ?? Type.GetType(interfaceName);

            if (interfaceType == null)
            {
                Debug.LogError($"{nameof(InterfaceWrapperKeyword)} could not find interface with name {interfaceName} nor {interfaceFullName}");
                return file;
            }

            file = file.Replace(Keyword, GetWrappingCode(interfaceType));

            return file;
        }

        private string GetWrappingCode(Type interfaceType)
        {
            string fieldName = "wrapped";
            string indSpace = "    ";
            string indentation = "";

            StringBuilder sb = new StringBuilder();

            // https://stackoverflow.com/a/26766221

            // find all parent interfaces
            var interfaceHierarchy = (new Type[] { interfaceType })
                .Concat(interfaceType.GetInterfaces());

            // get all defined and inherited properties
            var properties = interfaceHierarchy.SelectMany(i => i.GetProperties());

            using (var provider = new FancyTypeNameProvider())
            {
                sb.AppendLine($"{indentation} {provider.GetTypeName(interfaceType)}");
                sb.Append(indentation);
                sb.AppendLine("{");

                indentation += indSpace;

                sb.AppendLine($"{indentation}[SerializeField]");
                sb.AppendLine($"{indentation}private {provider.GetTypeName(interfaceType)} {fieldName} = default;");

                sb.AppendLine();

                foreach (PropertyInfo prop in properties)
                {
                    sb.Append($"{indentation}public {provider.GetTypeName(prop.PropertyType)} {prop.Name}");

                    if (prop.SetMethod == null)
                    {
                        sb.AppendLine($" => {fieldName}.{prop.Name};");
                    }
                    else
                    {
                        sb.AppendLine();

                        sb.Append(indentation);
                        sb.AppendLine("{");

                        sb.Append(indentation + indSpace);
                        sb.AppendLine($"get => {fieldName}.{prop.Name};");

                        sb.Append(indentation + indSpace);
                        sb.AppendLine($"set => {fieldName}.{prop.Name} = value;");

                        sb.Append(indentation);
                        sb.AppendLine("}");
                    }

                    sb.AppendLine();
                }

                // get all defined and inherited methods
                var methods = interfaceHierarchy.SelectMany(i => i.GetMethods());

                foreach (MethodInfo method in methods)
                {
                    // exclude properties getters/setters and maybe other stuff
                    if (method.IsSpecialName)
                        continue;

                    sb.Append($"{indentation}public {provider.GetTypeName(method.ReturnType)} {method.Name}(");
                    sb.Append(string.Join(", ", method.GetParameters().Select(par => $"{provider.GetTypeName(par.ParameterType)} {par.Name}")));
                    sb.AppendLine(")");

                    sb.Append(indentation);
                    sb.AppendLine("{");

                    sb.Append(indentation + indSpace);

                    if (method.ReturnType != typeof(void))
                        sb.Append("return ");

                    sb.Append($"{fieldName}.{method.Name}(");
                    sb.Append(string.Join(", ", method.GetParameters().Select(par => par.Name)));
                    sb.AppendLine(");");

                    sb.Append(indentation);
                    sb.AppendLine("}");

                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        class FancyTypeNameProvider : IDisposable
        {
            private static Dictionary<Type, string> Primitives = new Dictionary<Type, string>()
            {
                { typeof(void), "void" },
                { typeof(string), "string" },
                { typeof(char), "char" },
                { typeof(int), "int" },
                { typeof(uint), "uint" },
                { typeof(float), "float" },
                { typeof(double), "double" },
                { typeof(short), "short" },
                { typeof(ushort), "ushort" },
                { typeof(long), "long" },
                { typeof(ulong), "ulong" },
            };

            public string GetTypeName(Type type)
            {
                if (Primitives.TryGetValue(type, out string name))
                    return name;
                else
                    return type.Name;
            }

            public void Dispose()
            {
            }
        }
    }
}