using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;


//检索出StreamingAsset 文件夹中，需要上传的AssetBundle
public class AssetBundle_UploadInspect : MonoBehaviour
{
	public List<string> FileList = new List<string>();
	public static Dictionary<string, string> Dic_UpLoadFullPath = new Dictionary<string, string>();
	//public static Dictionary<string, string> Dic_UpLoadMD5 = new Dictionary<string, string>();
	public static Dictionary<string, long> Dic_UpLoadSize = new Dictionary<string, long>();

	public long Res_TotalSize = 0;

	private void Start()
	{
		Debug.Log("开始准备  AB Inspect");
		Prepare();
	}

	// Use this for initialization
	public void Prepare()
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

		Debug.Log(" AB Inspect 准备完成  ");
	}

	void WriteMD5()
	{
		Debug.Log("写入  MD5");

		if (!Directory.Exists(AppFacade.instance.Client.GetPersisdentPath()))
		{
			Directory.CreateDirectory(AppFacade.instance.Client.GetPersisdentPath());
		}
		if (!File.Exists(AppFacade.instance.Client.GetPersisdentMD5File()))
		{
			File.Create(AppFacade.instance.Client.GetPersisdentMD5File()).Dispose();
			//File.Create(AppFacade.instance.Client.GetPersisdentMD5File());
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
	}

	string GetMD5(byte[] buffer)
	{
		byte[] returnBytes;
		MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
		returnBytes = md5.ComputeHash(buffer);
		string strMD5 = BitConverter.ToString(returnBytes);

		return strMD5;
	}
	string GetMD5(string fullPath)
	{
		byte[] buffer = null;
		try
		{
			buffer = File.ReadAllBytes(fullPath);
		}
		catch
		{
			Debug.Log("GetFileBytes Wrong    " + fullPath);
		}

		byte[] returnBytes;
		MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
		returnBytes = md5.ComputeHash(buffer);
		string strMD5 = BitConverter.ToString(returnBytes);
		strMD5 = strMD5.Replace("-", "");
		return strMD5;
	}
}
