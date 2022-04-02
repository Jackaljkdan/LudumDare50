using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Editor.TemplateKeywords
{
    [Serializable]
    public class ProjectNameKeyword : ITemplateKeyword
    {
        public string Keyword => "#PROJECTNAME#";

        public string ReplaceInFile(string path, string file)
        {
            return file.Replace(Keyword, Application.productName);
        }
    }
}