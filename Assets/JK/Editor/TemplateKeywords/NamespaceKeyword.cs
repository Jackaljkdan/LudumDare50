using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Editor.TemplateKeywords
{
    [Serializable]
    public class NamespaceKeyword : ITemplateKeyword
    {
        public string Keyword => "#NAMESPACE#";

        public string ReplaceInFile(string path, string file)
        {
            string directoryPath = TemplateUtils.ExtractDirectoryPath(path);
            string relativePath = TemplateUtils.ExtractPathUnderAssets(directoryPath);
            string namespaceName = TemplateUtils.RemoveFoldersInPath(relativePath, TemplateUtils.CommonSilentFolders);

            namespaceName = namespaceName.Replace("/", ".");

            file = file.Replace(Keyword, namespaceName);

            return file;
        }
    }
}