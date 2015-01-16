using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
using System.IO;
namespace UnityEditor.MotinNativePlugins
{
	public static class EclipsePostProcess
	{
		[PostProcessBuild(1000)]
		public static void OnPostProcessBuild(BuildTarget target, string path)
		{
		
			if (target == BuildTarget.Android)
			{
				Debug.Log ("BUILD LOCATION "+  EditorUserBuildSettings.GetBuildLocation(target));

				Debug.Log ("POST PROCESS ECLIPSE "+ path);
				string SourcePath  = path + "/UnityBaseProject/assets";
				string DestinationPath  = path+ "/../UnityBaseProject/assets";


				CopyFolder(SourcePath,DestinationPath);

				SourcePath  = path + "/UnityBaseProject/libs/armeabi-v7a";
				DestinationPath  = path+ "/../UnityBaseProject/libs/armeabi-v7a";
				
				
				CopyFolder(SourcePath,DestinationPath);


			}


		}

		static void CopyFolder(string source, string dest)
		{
			//Now Create all of the directories
			foreach (string newPath in Directory.GetFiles(dest, "*.*", 
			                                              SearchOption.TopDirectoryOnly))
			{
				//Debug.Log("COPY " + newPath.Replace(source, dest));
				File.Delete(newPath);
			}

			foreach (string dirPath in Directory.GetDirectories(dest, "*", 
			                                                    SearchOption.TopDirectoryOnly))
			{
				//Debug.Log("DELETE " + dirPath);
				Directory.Delete(dirPath,true);
			}


			//Now Create all of the directories
			foreach (string dirPath in Directory.GetDirectories(source, "*", 
			                                                    SearchOption.AllDirectories))
			{
				Debug.Log("CREATE " + dirPath);
				Directory.CreateDirectory(dirPath.Replace(source, dest));
			}
			//Debug.Log("FILES");
			//Copy all the files
			foreach (string newPath in Directory.GetFiles(source, "*.*", 
			                                              SearchOption.AllDirectories))
			{
				//Debug.Log("COPY " + newPath.Replace(source, dest));
				File.Copy(newPath, newPath.Replace(source, dest),true);
			}
		}
	}
}
