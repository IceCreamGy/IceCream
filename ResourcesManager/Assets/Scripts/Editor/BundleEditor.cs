using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class BundleEditor : MonoBehaviour
{
	static string path = "Assets/Arts/Data/AssetBundle_Config.asset";

	[MenuItem("Tools/BuildABundle")]
	private static void Build()
	{
		AssetBundle_Config config = AssetDatabase.LoadAssetAtPath<AssetBundle_Config>(path);

		for (int i = 0; i < config.DirectroyPath.Count; i++)
		{
			Debug.Log(config.DirectroyPath[i].path);
		}
	}
}
