using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aliyun.OSS;
using System.IO;
using Aliyun.OSS.Common;
using System.Threading;
using System;

public class DownLoadControl : MonoBehaviour
{
	public string LocalPath;
	public string key;

	private Thread getThread;

	public static DownLoadControl instance;
	private void Awake()
	{
		instance = this;
	}

	OssClient client;
	// Use this for initialization
	void Start()
	{
		client = new OssClient(AppConst.EndPoint, AppConst.AccessKeyId, AppConst.AccessKeySecret);
		LocalPath = Application.persistentDataPath;
		LocalPath += "/ABData/";
	}

	//化解 异步线程不能操作的尴尬
	bool loadComplete = false;
	private void Update()
	{
		if (loadComplete)
		{
			mAction();
			loadComplete = false;
		}
	}

	Action mAction;
	public void DownloadAssetBundle(Action ac)
	{
		mAction = ac;
		key = "BiLan/shop";
		LocalPath += key;

		Debug.Log("Start  Get object succeeded");

		try
		{
			var obj = client.GetObject(AppConst.Bucket, key);
			using (var requestStream = obj.Content)
			{
				byte[] buf = new byte[requestStream.Length];
				var fs = File.Open(LocalPath, FileMode.OpenOrCreate);
				var len = 0;
				// 通过输入流将文件的内容读取到文件或者内存中。
				while ((len = requestStream.Read(buf, 0, 1024)) != 0)
				{
					fs.Write(buf, 0, len);
				}
				fs.Close();
			}
			Debug.Log("Get object succeeded");
			loadComplete = true;
		}
		catch (OssException ex)
		{
			Debug.Log(ex.Message);
			throw;
		}
	}
}
