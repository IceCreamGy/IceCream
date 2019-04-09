using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//进度，需要开始打包AB包3

public class BundleEditor : MonoBehaviour
{
	static string path = "Assets/Arts/Data/ABundleConfig.asset";
	static Dictionary<string, string> m_AllFileDir = new Dictionary<string, string>();    //资源文件的路径
	static List<string> m_AllFileAB = new List<string>();      //用来剔除的列表
	static Dictionary<string, List<string>> m_AllPrefabDir = new Dictionary<string, List<string>>();    //单个Prefab的AB包

	[MenuItem("Tools/BuildABundle")]
	private static void Build()
	{
		m_AllFileDir.Clear();

		//加载常规资源
		//----------------------------------------------------------------------------------------------------
		AssetBundle_Config config = AssetDatabase.LoadAssetAtPath<AssetBundle_Config>(path);

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

		//开始冗余筛选
		//----------------------------------------------------------------------------------------------------


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
					Debug.Log(allDepend[j]);
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


		EditorUtility.ClearProgressBar();
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
