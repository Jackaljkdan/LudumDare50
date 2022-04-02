using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Editor.TemplateKeywords
{
    /// <summary>
    /// Suppose that the file name is "xyzFiller", this replaces each keyword with "xyz"
    /// </summary>
    [Serializable]
    public class ScriptnameFilKeyword : ITemplateKeyword
    {
        public string Keyword => "#SCRIPTNAME_Fil#";

        public string ReplaceInFile(string path, string file)
        {
            string fileName = TemplateUtils.ExtractFilename(path);
            string scriptNameWithoutFactoryInstaller = fileName.Replace("Filler", "").Replace(".cs", "");

            file = file.Replace(Keyword, scriptNameWithoutFactoryInstaller);

            return file;
        }
    }
}