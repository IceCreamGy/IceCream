using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameStart : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		string path = "Assets/Arts/Data/AssetBundle_Config.asset";
		Debug.Log(path);

		AssetBundle_Config config = AssetDatabase.LoadAssetAtPath<AssetBundle_Config>(path);

		for (int i = 0; i < config.PrefabPath.Count; i++)
		{
			Debug.Log(config.PrefabPath[i]);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
