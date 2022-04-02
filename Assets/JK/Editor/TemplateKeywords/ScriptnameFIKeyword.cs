using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Editor.TemplateKeywords
{
    /// <summary>
    /// Suppose that the file name is "xyzFactoryInstaller", this replaces each keyword with "xyz"
    /// </summary>
    [Serializable]
    public class ScriptnameFIKeyword : ITemplateKeyword
    {
        public string Keyword => "#SCRIPTNAME_FI#";

        public string ReplaceInFile(string path, string file)
        {
            string fileName = TemplateUtils.ExtractFilename(path);
            string scriptNameWithoutFactoryInstaller = fileName.Replace("FactoryInstaller", "").Replace(".cs", "");

            file = file.Replace(Keyword, scriptNameWithoutFactoryInstaller);

            return file;
        }
    }
}