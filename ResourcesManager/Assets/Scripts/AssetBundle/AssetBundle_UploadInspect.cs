using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEditor;

//检索出StreamingAsset 文件夹中，需要上传的AssetBundle
public class AssetBundle_UploadInspect : MonoBehaviour
{
	public List<string> FileList = new List<string>();
	public static Dictionary<string, string> Dic_UpLoadFullPath = new Dictionary<string, string>();
	public static Dictionary<string, long> Dic_UpLoadSize = new Dictionary<string, long>();

	public long Res_TotalSize = 0;


	public void Start()
	{
		Prepare();
	}

	void Prepare()
	{
		string checkPath = Application.streamingAssetsPath;
		DirectoryInfo dir = new DirectoryInfo(checkPath);
		FileInfo[] childInfo = dir.GetFiles();
		for (int i = 0; i < childInfo.Length; i++)
		{
			string childName = childInfo[i].Name;
			if (childName.Contains("manifest") || childName.Contains("meta") || childName.Contains("StreamingAssets"))
				continue;

			childName = AppFacade.instance.Client.GetHttpServerBundleDir() + "/" + childName;
			FileList.Add(childName);
			Dic_UpLoadFullPath.Add(childName, childInfo[i].FullName);
			Dic_UpLoadSize.Add(childName, childInfo[i].Length);
		}

		WriteMD5();
	}
	void WriteMD5()
	{
		if (!Directory.Exists(AppFacade.instance.Client.GetPersisdentPath()))
		{
			Directory.CreateDirectory(AppFacade.instance.Client.GetPersisdentPath());
		}
		if (!File.Exists(AppFacade.instance.Client.GetPersisdentMD5File()))
		{
			File.Create(AppFacade.instance.Client.GetPersisdentMD5File()).Dispose();
		}

		string writePath = AppFacade.instance.Client.GetPersisdentMD5File();
		StringBuilder sb = new StringBuilder();

		foreach (string item in Dic_UpLoadFullPath.Keys)
		{
			string strSimplePath = Application.streamingAssetsPath.Replace("/", "\\");
			string pathInAsset = Dic_UpLoadFullPath[item].Replace(strSimplePath, "");
			pathInAsset = pathInAsset.Replace("\\", "");

			sb.Append(pathInAsset + "|");
			sb.Append(GetMD5(Dic_UpLoadFullPath[item]) + "|");
			sb.Append(Dic_UpLoadSize[item]);

			sb.AppendLine();
		}

		File.WriteAllText(writePath, sb.ToString());

		Debug.Log("写入  MD5");
	}
	string GetMD5(string fullPath)
	{
		byte[] buffer = null;
		try
		{ buffer = File.ReadAllBytes(fullPath); }
		catch
		{ Debug.Log("GetFileBytes Wrong    " + fullPath); }

		byte[] returnBytes;
		MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
		returnBytes = md5.ComputeHash(buffer);
		string strMD5 = BitConverter.ToString(returnBytes);
		strMD5 = strMD5.Replace("-", "");
		return strMD5;
	}

	void UploadAssetBundle_Method()
	{
		UploadItem[] items = new UploadItem[Dic_UpLoadFullPath.Keys.Count];
		int i = 0;
		foreach (string key in Dic_UpLoadFullPath.Keys)
		{
			items[i].FileSaveName = key;
			items[i].FIlePath = Dic_UpLoadFullPath[key];
		}
		CloudServer.instance.Upload_Files(items);

		UploadMD5();
	}

	void UploadMD5()
	{
		CloudServer.instance.Upload_FileByPath("Assets_MD5", AppFacade.instance.Client.GetPersisdentMD5File());
	}
}
