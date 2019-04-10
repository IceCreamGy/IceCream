using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//进度，需要开始打包AB包4

public class BundleEditor : MonoBehaviour
{
	static string ConfigPath = "Assets/Arts/Data/ABundleConfig.asset";
	static Dictionary<string, string> m_AllFileDir = new Dictionary<string, string>();    //资源文件的路径
	static List<string> m_AllFileAB = new List<string>();      //用来剔除的列表
	static Dictionary<string, List<string>> m_AllPrefabDir = new Dictionary<string, List<string>>();    //单个Prefab的AB包

	static AssetBundle_Config config;

	[MenuItem("Tools/BuildABundle")]
	private static void Build()
	{
		config = AssetDatabase.LoadAssetAtPath<AssetBundle_Config>(ConfigPath);

		ClearData();
		Load_Asset();
		Load_Prefab();
		ClearABName();

		EditorUtility.ClearProgressBar();
	}                           

	static void ClearData()
	{
		m_AllFileDir.Clear();
		m_AllFileAB.Clear();
		m_AllPrefabDir.Clear();
	}
	static void Load_Asset()      //加载常规资源
	{	
		for (int i = 0; i < config.DirectroyPath.Count; i++)
		{
			string DirName = config.DirectroyPath[i].name;
			string DirPath = config.DirectroyPath[i].path;
			if (m_AllFileDir.ContainsKey(DirName))
			{
				Debug.LogError("资源名重复，请检查");
			}
			else
			{
				m_AllFileDir.Add(DirName, DirPath);
				m_AllFileAB.Add(DirPath);
			}
		}

		//设置AB包
		foreach (string name in m_AllFileDir.Keys)
		{
			SetABName(name, m_AllFileDir[name]);
		}
	}
	static void Load_Prefab()       //加载Prefab，开始冗余筛选
	{
		string[] allStr = AssetDatabase.FindAssets("t:Prefab", config.m_AllPrefabPath.ToArray());
		for (int i = 0; i < allStr.Length; i++)
		{
			string path = AssetDatabase.GUIDToAssetPath(allStr[i]);
			EditorUtility.DisplayProgressBar("加载 Prefab配置表", "Load Prefab" + path, i * 1.0f / allStr.Length);

			if (!ContainAllFileAB(path))
			{
				GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
				string[] allDepend = AssetDatabase.GetDependencies(path);
				List<string> allDependPath = new List<string>();

				for (int j = 0; j < allDepend.Length; j++)
				{		
					if (!ContainAllFileAB(allDepend[j]) && !allDepend[j].EndsWith(".cs"))
					{
						m_AllFileAB.Add(allDepend[j]);
						allDependPath.Add(allDepend[j]);
					}
				}

				if (m_AllPrefabDir.ContainsKey(obj.name))
				{
					Debug.LogError("存在相同的Prefab  " + obj.name);
				}
				else
				{
					m_AllPrefabDir.Add(obj.name, allDependPath);
				}
			}
		}

		foreach (string name in m_AllPrefabDir.Keys)
		{
			SetABName(name, m_AllPrefabDir[name]);
		}	
	}
	static void ClearABName()      //清除AssetBundle Name，防止meta文件的修改
	{
		string[] oldABNames = AssetDatabase.GetAllAssetBundleNames();
		for (int i = 0; i < oldABNames.Length; i++)
		{
			AssetDatabase.RemoveAssetBundleName(oldABNames[i], true);
			EditorUtility.DisplayProgressBar("清除AB包名", " ABundle-Name" + oldABNames[i], i / oldABNames.Length);
		}
	}

	static void SetABName(string name, string path)
	{
		AssetImporter assetImport = AssetImporter.GetAtPath(path);
		if (assetImport == null)
		{
			Debug.LogError("路径错误：  " + path);
		}
		else
		{
			assetImport.assetBundleName = name;
		}
	}
	static void SetABName(string name, List<string> paths)
	{
		for (int i = 0; i < paths.Count; i++)
		{
			SetABName(name, paths[i]);
		}
	}

	static bool ContainAllFileAB(string path)
	{
		for (int i = 0; i < m_AllFileAB.Count; i++)
		{
			if (path == m_AllFileAB[i] || path.Contains(m_AllFileAB[i]))
			{
				return true;
			}
		}

		return false;
	}
}
