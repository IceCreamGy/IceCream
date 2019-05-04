using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class UpdateManager : BaseManager
{
	private Dictionary<string, MD5_FileInfo> old_res_dic;       //旧的Md5码
	private Dictionary<string, MD5_FileInfo> new_res_dic;       //最新的Md5码（从服务器下载的）
	private Dictionary<string, MD5_FileInfo> need_download_dic;     //需要下载的资源
	private Dictionary<string, MD5_FileInfo> downloaded_dic;        //已经下载过的资源

	private long res_total_size = 0L;

	private void Start()
	{
		MsgManager.Register(Msg.CheckRes, this, "CheckUpdate");
	}

	//通过按钮点击启动更新
	public void CheckUpdate()
	{
		Debug.Log("执行更新");

		if (!File.Exists(Client.GetPersistentMD5Path()))
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
		old_res_dic = GetMd5_Old();
		StartCoroutine(DownLoadNewMD5());
		CheckNeedDownDic();

		if (need_download_dic.Count == 0)
		{
			MsgManager.Broadcast(Msg.Res_DownLoad_Finish, null);
		}
		else
		{
			MsgManager.Broadcast(Msg.Res_Download_Start, new object[] { res_total_size });
			StartCoroutine(DownloadAsset(need_download_dic));
		}
	}

	/// <summary>
	/// 获取存在本地老版本的 MD5码
	/// </summary>
	/// <returns></returns>
	private Dictionary<string, MD5_FileInfo> GetMd5_Old()
	{
		string path = AppFacade.instance.Client.GetPersistentMD5Path();
		return GetMD5_Tools(path);
	}

	/// <summary>
	/// 获取存在本地最新的 MD5码
	/// </summary>
	/// <returns></returns>
	private Dictionary<string, MD5_FileInfo> GetMd5_New()
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
		string inFile = Client.GetStreamingMD5Path();
		string outFile = Client.GetPersistentMD5Path();
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
		MsgManager.Broadcast(Msg.Res_Release_Start, new object[] { files.Length });
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
			MsgManager.Broadcast(Msg.Res_Release_One, null);
			yield return new WaitForEndOfFrame();
		}
		//解包完更新
		UpdateRes();
	}

	/// <summary>
	/// 从网络下载最新的Md5码
	/// </summary>
	/// <returns></returns>
	IEnumerator DownLoadNewMD5()
	{
		string ServerMD5Path = Path.Combine(AppConst.Res_Download_Address, Client.GetHttpServerMD5Path());
		string md5_url = Util.StandardlizePath(ServerMD5Path);
		md5_url = "http://" + md5_url;

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
		if (!File.Exists(Client.GetPersisdentServerMD5File()))
		{
			File.Create(Client.GetPersisdentServerMD5File()).Dispose();
		}
		File.WriteAllText(Client.GetPersisdentServerMD5File(), www.text);
		www.Dispose();

		yield return new WaitForEndOfFrame();
	}

	/// <summary>
	/// 检查出需要更新的资源
	/// </summary>
	void CheckNeedDownDic()
	{
		new_res_dic = GetMd5_New();
		need_download_dic = new Dictionary<string, MD5_FileInfo>();
		downloaded_dic = new Dictionary<string, MD5_FileInfo>();

		foreach (var pair in new_res_dic)
		{
			if (!old_res_dic.ContainsKey(pair.Key))
			{
				//1.不包含的    
				need_download_dic.Add(pair.Key, pair.Value);
				res_total_size += pair.Value.Size;
			}
			else if (!old_res_dic[pair.Key].MD5.Equals(pair.Value.MD5))
			{
				//2.不一样的Md5
				need_download_dic.Add(pair.Key, pair.Value);
				res_total_size += pair.Value.Size;
			}
			else
			{
				//3.  本地与服务器一致的
				downloaded_dic.Add(pair.Key, pair.Value);
			}
		}
	}

	/// <summary>
	/// 下载需要更新的资源
	/// </summary>
	/// <param name="needDownload_dic"></param>
	/// <returns></returns>
	IEnumerator DownloadAsset(Dictionary<string, MD5_FileInfo> needDownload_dic)
	{
		int has_download_count = 0;
		int need_download_count = needDownload_dic.Count;

		foreach (var pair in needDownload_dic)
		{
			string path = Util.PathCombine(AppConst.Res_Download_Address, Client.GetHttpServerBundleDir(), pair.Key);
			Debug.Log(path);

			WWW www = new WWW("http://" + path);
			while (!www.isDone)
			{
				yield return null;
			}
			if (string.IsNullOrEmpty(www.error))
			{
				string file_path = Util.PathCombine(Client.GetPersistentPath(), pair.Key);
				string file_md5 = Util.GetBytesMD5(www.bytes);
				if (string.Equals(file_md5, pair.Value.MD5))    //下载完成后，对Md5的教研
				{
					string directory = file_path.Substring(0, file_path.LastIndexOf('/'));
					if (!Directory.Exists(directory))
					{
						Directory.CreateDirectory(directory);
					}
					File.WriteAllBytes(file_path, www.bytes);
					downloaded_dic.Add(pair.Key, pair.Value);
					has_download_count++;
					MsgManager.Broadcast(Msg.Res_Download_One, new object[] { (float)has_download_count / (float)need_download_count, pair.Key });
				}
				else
				{
					//教研出错的话，需要加到下载队列中，重新下载
					Debug.LogError("下载出错  " + pair.Key);
				}
			}
			else
			{
				Debug.Log(string.Format("下载出错    {0}    因为   {1} ", pair.Key, www.error));
			}
		}

		if (has_download_count == need_download_count)
		{
			ExportDownloadedMD5(downloaded_dic);
			MsgManager.Broadcast(Msg.Res_DownLoad_Finish, null);
			Debug.Log("下载完成");
		}
		else
		{
			Debug.Log(string.Format("下载进度  download_count    {0}   need_download  {1}", has_download_count, need_download_count));
		}
	}

	void ExportDownloadedMD5(Dictionary<string, MD5_FileInfo> dic)
	{
		StringBuilder sb = new StringBuilder();
		foreach (var pair in dic)
		{
			sb.AppendLine(string.Format("{0}|{1}|{2}", pair.Value.Name, pair.Value.MD5, pair.Value.Size));
		}
		File.WriteAllText(Client.GetPersistentMD5Path(), sb.ToString());
	}

	private void OnApplicationQuit()
	{
		//程序退出时，保存已经下载完毕的
	}
}
