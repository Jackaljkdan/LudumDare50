using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Editor.TemplateKeywords
{
    public interface ITemplateKeyword
    {
        string Keyword { get; }

        string ReplaceInFile(string path, string file);
    }
}