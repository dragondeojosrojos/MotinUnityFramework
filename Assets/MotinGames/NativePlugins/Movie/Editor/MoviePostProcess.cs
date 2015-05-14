using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;

namespace UnityEditor.MotinNativePlugins
{
    public static class MoviePostProcess
    {
        [PostProcessBuild(400)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
			Debug.Log ("POstProcess Movie");


           #if UNITY_5
            if (target == BuildTarget.iOS)
#else
			if (target == BuildTarget.iPhone)
#endif
            {
                UnityEditor.XCodeEditor.XCProject project = new UnityEditor.XCodeEditor.XCProject(path);

                // Find and run through all projmods files to patch the project

                string projModPath = System.IO.Path.Combine(Application.dataPath, "MotinGames/NativePlugins/Movie/Editor/iOS");
                var files = System.IO.Directory.GetFiles(projModPath, "*.projmods", System.IO.SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    project.ApplyMod( file);
                }
                project.Save();

                //PlistMod.UpdatePlist(path, FBSettings.AppId);
                //FixupFiles.FixSimulator(path);

               // FixupFiles.AddVersionDefine(path);
            }

            if (target == BuildTarget.Android)
            {
                // The default Bundle Identifier for Unity does magical things that causes bad stuff to happen
                if (PlayerSettings.bundleIdentifier == "com.Company.ProductName")
                {
                    Debug.LogError("The default Unity Bundle Identifier (com.Company.ProductName) will not work correctly.");
                }
                //if (!FacebookAndroidUtil.IsSetupProperly())
                //{
                //    Debug.LogError("Your Android setup is not correct. See Settings in Facebook menu.");
                //}
            }
        }
    }
}
