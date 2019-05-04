using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WindowsClient : GameClient
{
	//http下载相关
	private string http_md5 = "Windows/WindowsAssetBundle/ServerMd5.txt";                    //平台下的MD5文件下载路径
	private string http_bundle_download_dir = "Windows/WindowsAssetBundle";           //bundle包下载目录
	
	public override string GetHttpServerMD5Path()
	{
		return http_md5;
	}

	public override string GetHttpServerBundleDir()
	{
		return http_bundle_download_dir;
	}
}