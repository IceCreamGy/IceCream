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

	/// <summary>
	/// 获取存在本地 的老版本MD5码
	/// </summary>
	/// <returns></returns>
	private Dictionary<string, MD5_FileInfo> GetClientMD5Data()
	{
		if (!Directory.Exists(Client.GetPersisdentPath()))
		{
			Directory.CreateDirectory(Client.GetPersisdentPath());
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
}
