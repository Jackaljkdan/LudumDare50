using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JK.Editor.TemplateKeywords
{
    public static class TemplateUtils
    {
        public const string PathFallback = "BelLavoroComplimenti";

        public static readonly string[] CommonSilentFolders = new string[]
        {
            "Scripts",
            "Assets",
            "ScriptTemplates",
        };

        public static string ExtractPathUnderAssets(string path, string fallback = PathFallback)
        {
            string assets = "/Assets";

            int lastAssetsIndex = path.LastIndexOf(assets);

            if (lastAssetsIndex < 0)
            {
                Debug.LogError($"{nameof(ExtractDirectoryPath)} path {path} does not look right");
                return fallback;
            }

            string extractedPath = path.Substring(lastAssetsIndex + assets.Length);

            if (extractedPath.StartsWith("/"))
            {
                extractedPath = extractedPath.Substring(1);
            }
            else if (extractedPath.Length > 0)
            {
                Debug.LogError($"{nameof(ExtractDirectoryPath)} path {path} does not look right");
                return fallback;
            }

            return extractedPath;
        }

        public static string RemoveFoldersInPath(string path, IEnumerable<string> folders, string fallback = PathFallback)
        {
            string cleanedPath = path;

            foreach (string folder in folders)
            {
                if (cleanedPath == folder)
                    return fallback;

                cleanedPath = cleanedPath.Replace($"/{folder}", "");
                cleanedPath = cleanedPath.Replace($"{folder}/", "");
            }

            if (cleanedPath.Length > 0)
                return cleanedPath;
            else
                return fallback;
        }

        public static string ExtractDirectoryPath(string path)
        {
            int lastFolderEnd = path.LastIndexOf("/");

            if (lastFolderEnd >= 0)
                path = path.Substring(0, lastFolderEnd);

            return path;
        }

        public static string ExtractFilename(string path)
        {
            int lastFolderEnd = path.LastIndexOf("/");

            if (lastFolderEnd >= 0)
                path = path.Substring(lastFolderEnd + 1, path.Length - lastFolderEnd - 1);

            return path;
        }
    }
}