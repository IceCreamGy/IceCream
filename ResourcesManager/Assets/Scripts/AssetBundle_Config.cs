using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ABundleConfig", menuName = "CreatABundleConfig")]
public class AssetBundle_Config : ScriptableObject
{
	//遍历这个文件夹下面所有的Prefab，所有的Prefab的名字不能重复，必须保证名字的唯一性
	public List<string> PrefabPath = new List<string>();
	public List<DirInfo> DirectroyPath = new List<DirInfo>();

	[Serializable]
	public struct DirInfo   //目录信息
	{
		public string name;
		public string path;
	}
}
