using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Editor.TemplateKeywords
{
    [Serializable]
    public class ScriptPathKeyword : ITemplateKeyword
    {
        public string Keyword => "#SCRIPTPATH#";

        public string ReplaceInFile(string path, string file)
        {
            string directoryPath = TemplateUtils.ExtractDirectoryPath(path);
            string relativePath = TemplateUtils.ExtractPathUnderAssets(directoryPath);
            string cleanedPath = TemplateUtils.RemoveFoldersInPath(relativePath, TemplateUtils.CommonSilentFolders);

            file = file.Replace(Keyword, cleanedPath);

            return file;
        }
    }
}