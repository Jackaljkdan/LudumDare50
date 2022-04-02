#if UNITY_EDITOR
// the #if guard is necessary because UnityEditor.AssetModificationProcessor fails when building

using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

namespace JK.Editor.TemplateKeywords
{
    /// <summary>
    /// https://stackoverflow.com/questions/39461801/unity-add-default-namespace-to-script-template/52395369#52395369
    /// </summary>
    public class AdditionalTemplateKeywords : UnityEditor.AssetModificationProcessor
    {
        public static string[] UnityBuiltinKeywords = new string[]
        {
            "#SCRIPTNAME#",
            "#NOTRIM#",
        };

        public static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            int index = path.LastIndexOf(".");
            if (index < 0) return;
            string file = path.Substring(index);

            if (file != ".cs" && file != ".js" && file != ".boo")
                return;

            string assets = "Assets";

            index = Application.dataPath.LastIndexOf(assets);
            path = Application.dataPath.Substring(0, index) + path;
            file = System.IO.File.ReadAllText(path);

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var keywordClasses = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(t => typeof(ITemplateKeyword).IsAssignableFrom(t) && t.GetInterfaces().Contains(typeof(ITemplateKeyword)));

            watch.Stop();

            if (keywordClasses != null)
            {
                Dictionary<string, string> keywordsApplied = new Dictionary<string, string>();

                foreach (var kw in UnityBuiltinKeywords)
                    keywordsApplied.Add(kw, "Unity built-in");

                foreach (Type cls in keywordClasses)
                {
                    ITemplateKeyword keyword = Activator.CreateInstance(cls) as ITemplateKeyword;

                    if (keyword != null)
                    {
                        if (keywordsApplied.TryGetValue(keyword.Keyword, out string previousClassName))
                        {
                            Debug.LogError($"{nameof(AdditionalTemplateKeywords)} {previousClassName} and {cls.Name} are conflicting on keyword {keyword.Keyword}, will apply only {previousClassName}");
                            continue;
                        }

                        keywordsApplied.Add(keyword.Keyword, cls.Name);
                        file = keyword.ReplaceInFile(path, file);
                    }
                }
            }
            
            System.IO.File.WriteAllText(path, file);
            AssetDatabase.Refresh();

            watch.Stop();
            Debug.Log($"{nameof(AdditionalTemplateKeywords)} applying keywords took {watch.Elapsed.TotalMilliseconds} ms");
        }
    }
}

#endif  // UNITY_EDITOR