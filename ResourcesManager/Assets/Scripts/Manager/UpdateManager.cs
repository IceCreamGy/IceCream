using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UpdateManager : BaseManager
{
	private Dictionary<string, MD5_FileInfo> old_res_dic;       //旧的资源
	private Dictionary<string, MD5_FileInfo> new_res_dic;       //服务器中，最新的资源
	private Dictionary<string, MD5_FileInfo> need_download_dic;     //需要下载的资源
	private Dictionary<string, MD5_FileInfo> downloaded_dic;        //已经下载过的资源

	//通过按钮点击启动更新
	public void CheckUpdate()
	{
		if (!File.Exists(Client.GetPersisdentMD5File()))
		{
			StartCoroutine(MoveStreamingAssetsToPersisdentPath());     //解压到 Persistent
		}
		else
		{
			UpdateRes();        //更新
		}
	}

	void UpdateRes()
	{
		old_res_dic = GetClientMD5Data();
		StartCoroutine(DownLoadNewMD5());
	}

	/// <summary>
	/// 获取存在本地 的老版本MD5码
	/// </summary>
	/// <returns></returns>
	private Dictionary<string, MD5_FileInfo> GetClientMD5Data()
	{
		if (!Directory.Exists(Client.GetPersistentPath()))
		{
			Directory.CreateDirectory(Client.GetPersistentPath());
		}

		if (!File.Exists(Client.GetPersisdentMD5File()))
		{
			File.Create(Client.GetPersisdentMD5File()).Dispose();
		}
		string path = AppFacade.instance.Client.GetPersisdentMD5File();
		return GetMD5_Tools(path);
	}

	/// <summary>
	/// 获取存在本地  的最新版本MD5码
	/// </summary>
	/// <returns></returns>
	private Dictionary<string, MD5_FileInfo> GetServerMD5Data()
	{
		string path = AppFacade.instance.Client.GetPersisdentServerMD5File();
		return GetMD5_Tools(path);
	}

	private Dictionary<string, MD5_FileInfo> GetMD5_Tools(string path)
	{
		Dictionary<string, MD5_FileInfo> dic = new Dictionary<string, MD5_FileInfo>();
		string[] lines = File.ReadAllLines(path);
		foreach (string line in lines)
		{
			if (!string.IsNullOrEmpty(line))
			{
				string[] md5_file_Infos = line.Split('|');
				string name = md5_file_Infos[0];
				string md5Code = md5_file_Infos[1];
				long size = long.Parse(md5_file_Infos[2]);
				dic.Add(name, new MD5_FileInfo(name, md5Code, size));
			}
		}
		if (lines == null)
		{
			Debug.LogError("Md5码  加载错误");
			return null;
		}
		else
		{
			return dic;
		}
	}

	/// <summary>
	/// 资源的解压
	/// </summary>
	/// <returns></returns>
	IEnumerator MoveStreamingAssetsToPersisdentPath()
	{
		string streamingPath = Client.GetStreamingDataPath(); //随包资源目录
		string persistentPath = Client.GetPersistentPath();  //持久化资源目录

		if (Directory.Exists(persistentPath)) Directory.Delete(persistentPath, true);
		Directory.CreateDirectory(persistentPath);

		//先找到MD5 匹配文件
		string inFile = Client.GetStreamingMD5File();
		string outFile = Client.GetPersisdentMD5File();
		if (File.Exists(outFile)) File.Delete(outFile);

		if (Application.platform == RuntimePlatform.Android)
		{
			WWW www = new WWW(inFile);
			yield return www;

			if (www.isDone)
			{
				File.WriteAllBytes(outFile, www.bytes);
			}
			yield return 0;
		}
		else
		{
			File.Copy(inFile, outFile, true);
		}
		yield return new WaitForEndOfFrame();

		//释放所有文件到数据目录
		string[] files = File.ReadAllLines(outFile);
		//MsgManager.Broadcast(Msg.Res_Release_Start, new object[] { files.Length });
		foreach (var file in files)
		{
			string[] fs = file.Split('|');
			inFile = Path.Combine(streamingPath, fs[0]);
			outFile = Path.Combine(persistentPath, fs[0]);

			string dir = Path.GetDirectoryName(outFile);
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

			if (Application.platform == RuntimePlatform.Android)
			{
				WWW www = new WWW(inFile);
				yield return www;

				if (www.isDone)
				{
					File.WriteAllBytes(outFile, www.bytes);
				}
				yield return 0;
			}
			else
			{
				if (File.Exists(outFile))
				{
					File.Delete(outFile);
				}
				File.Copy(inFile, outFile, true);
			}
			//MsgManager.Broadcast(Msg.Res_Release_One, null);
			yield return new WaitForEndOfFrame();
		}
		//解包完更新
		UpdateRes();
	}

	IEnumerator DownLoadNewMD5()
	{
		string ServerMD5Path = Path.Combine(AppConst.Res_Download_Address, Client.GetHttpServerMD5Path());
		string md5_url = Util.StandardlizePath(ServerMD5Path);
		md5_url = "http://" + md5_url;
		Debug.Log(md5_url);
		WWW www = new WWW(md5_url);
		while (!www.isDone)
		{
			yield return null;
		}
		if (!string.IsNullOrEmpty(www.error))
		{
			Debug.LogError(www.error);
			www.Dispose();
			yield break;
		}
		File.WriteAllText(Client.GetPersisdentServerMD5File(), www.text);
		www.Dispose();

		yield return new WaitForEndOfFrame();
	}
}
