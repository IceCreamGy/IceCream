using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOSClient : GameClient
{
	//http下载相关
	private string http_md5 = "ios/md5.txt";                    //平台下的MD5文件下载路径
	private string http_bundle_download_dir = "ios";           //bundle包下载目录

	public override string GetHttpServerBundleDir()
	{
		return http_bundle_download_dir;
	}

	public override string GetHttpServerMD5Path()
	{
		return http_md5;
	}
}