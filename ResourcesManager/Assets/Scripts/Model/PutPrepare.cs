using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class PutPrepare : MonoBehaviour
{
	public List<string> FileList = new List<string>();
	public static Dictionary<string, string> Dic_UpLoad = new Dictionary<string, string>();

	// Use this for initialization
	void Start()
	{
		string path = Application.streamingAssetsPath;
		DirectoryInfo dir = new DirectoryInfo(path);
		FileInfo[] childInfo = dir.GetFiles();
		for (int i = 0; i < childInfo.Length; i++)
		{
			string childName = childInfo[i].Name;
			if (childName.Contains("manifest") || childName.Contains("meta") || childName.Contains("StreamingAssets"))
				continue;

			FileList.Add(childName);
			Dic_UpLoad.Add("BiLan/" + childName, childInfo[i].FullName);
		}
	}
}
