using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Spacecraft.Editor
{
public class NamespaceResolver : UnityEditor.AssetModificationProcessor
{
    public static void OnWillCreateAsset(string metaFilePath)
    {
        var FileName = Path.GetFileNameWithoutExtension(metaFilePath);
        if (!FileName.EndsWith(".cs"))
            return;
        var ActualFile = $"{Path.GetDirectoryName(metaFilePath)}\\{FileName}";
        var SegmentedPath = $"{Path.GetDirectoryName(metaFilePath)}".Split(new[]{'\\'}, StringSplitOptions.None);

        var GeneratedNamespace = "";
        var FinalNamespace = "";

        if (SegmentedPath.Length <= 2) FinalNamespace = EditorSettings.projectGenerationRootNamespace;
        else
        {
            for (var i = 2; i < SegmentedPath.Length; i++)
            {
                GeneratedNamespace += i == SegmentedPath.Length - 1 ? SegmentedPath[i] : SegmentedPath[i] + ".";
            }

            FinalNamespace = EditorSettings.projectGenerationRootNamespace + "." + GeneratedNamespace;
        }

        var Content = File.ReadAllText(ActualFile);
        var NewContent = Content.Replace("#NAMESPACE#", FinalNamespace);

        if (Content != NewContent)
        {
            File.WriteAllText(ActualFile, NewContent);
            AssetDatabase.Refresh();
        }
        
    }
}
}