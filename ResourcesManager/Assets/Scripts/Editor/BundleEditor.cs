using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class BundleEditor : MonoBehaviour
{
    static string path = "Assets/Arts/Data/ABundleConfig.asset";
    static Dictionary<string, string> Dic = new Dictionary<string, string>();


	[MenuItem("Tools/BuildABundle")]
	private static void Build()
	{
		AssetBundle_Config config = AssetDatabase.LoadAssetAtPath<AssetBundle_Config>(path);

		for (int i = 0; i < config.DirectroyPath.Count; i++)
		{
            string DirName =config.DirectroyPath[i].name;
			string DirPath= config.DirectroyPath[i].path;
            Dic.Add(DirName, DirPath);
		}
	}
}
