using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEditor;

//检索出StreamingAsset 文件夹中，需要上传的AssetBundle
public class AssetBundle_UploadInspect : BaseManager
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

			//childName = Path.Combine(AppFacade.instance.Client.GetHttpServerBundleDir(), childName);
			//childName = Util.StandardlizePath(childName);
			childName = AppFacade.instance.Client.GetHttpServerBundleDir() + "/" + childName;
			FileList.Add(childName);
			Dic_UpLoadFullPath.Add(childName, childInfo[i].FullName);
			Dic_UpLoadSize.Add(childName, childInfo[i].Length);
		}

		WriteMD5();
	}
	void WriteMD5()
	{
		string writePath = Client.GetStreamingMD5Path();
		StringBuilder sb = new StringBuilder();

		foreach (string item in Dic_UpLoadFullPath.Keys)
		{
			string strSimplePath = Application.streamingAssetsPath.Replace("/", "\\");
			string pathInAsset = Dic_UpLoadFullPath[item].Replace(strSimplePath, "");
			pathInAsset = pathInAsset.Replace("\\", "");

			if (pathInAsset.Contains("md5")|| pathInAsset.Contains("Md5")|| pathInAsset.Contains("MD5"))
			{
				continue;
			}
			sb.Append(pathInAsset + "|");
			sb.Append(GetMD5(Dic_UpLoadFullPath[item]) + "|");
			sb.Append(Dic_UpLoadSize[item]);

			sb.AppendLine();
		}

		File.WriteAllText(writePath, sb.ToString());

		Debug.Log("写入  MD5    " + writePath);
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

	public void UploadAssetBundle_Method()
	{
		Debug.Log("开始上传");
		AppFacade.instance.GetMsgManager().Broadcast(Msg.Res_Upload_Start, null);

		UploadItem[] items = new UploadItem[Dic_UpLoadFullPath.Keys.Count];
		int i = 0;
		foreach (string key in Dic_UpLoadFullPath.Keys)
		{
			items[i].FileSaveName = key;
			items[i].FIlePath = Dic_UpLoadFullPath[key];

			i++;
		}

		CloudServer.instance.Upload_Files(items);

		UploadMD5();
	}

	void UploadMD5()
	{
		CloudServer.instance.Upload_FileByPath(Client.GetHttpServerMD5Path(), AppFacade.instance.Client.GetStreamingMD5Path());
	}
}
