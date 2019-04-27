using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

//进度，需要开始打包AB包5

public class BundleEditor : MonoBehaviour
{
	static string ConfigPath = "Assets/Arts/Data/ABundleConfig.asset";
	static string m_BundleTargetPath = Application.streamingAssetsPath;           //打包存放的位置

	List<string> Dependences = new List<string>();

	[MenuItem("Tools/BuildABundle")]
	private static void Build()
	{
		AssetBundle_Config config = AssetDatabase.LoadAssetAtPath<AssetBundle_Config>(ConfigPath);
	
		EditorUtility.ClearProgressBar();
	}

	[MenuItem("Tools/BuildABundle-Simple")]
	private static void BuildSimple()
	{
		BuildPipeline.BuildAssetBundles(m_BundleTargetPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
		AssetDatabase.Refresh();
	}
}
