using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UnityEditor.Hyplay
{
    public class HyplayAddRedirectOnBuild : IPostprocessBuildWithReport
    {
        public int callbackOrder { get; }
        public void OnPostprocessBuild(BuildReport report)
        {
            #if !UNITY_WEBGL
            return;
            #endif
            // 1. Get the Build Output Path
            string buildOutputPath = report.summary.outputPath;
            string targetDirectory = buildOutputPath;
            string filePath = "redirect.html";
            string targetFilePath = Path.Combine(targetDirectory, filePath); 

            File.WriteAllText(targetFilePath, "");
            Debug.Log(targetFilePath);
            
        }
    }
}