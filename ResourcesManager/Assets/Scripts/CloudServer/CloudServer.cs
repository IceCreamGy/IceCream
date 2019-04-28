using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Text;
using Aliyun.OSS;
using Aliyun.OSS.Common;
using System.IO;

public class CloudServer : MonoBehaviour
{
	public static CloudServer instance;
	private void Awake()
	{
		instance = this;
	}

	private OssClient client;
	private Thread thread;

	public float UploadProcess;    //上传的进度
	public float DownloadProcess;    //下载的进度

	// Use this for initialization
	void Start()
	{
		client = new OssClient(AppConst.EndPoint, AppConst.AccessKeyId, AppConst.AccessKeySecret);
	}

	public void UploadAssetBundle()
	{
		thread = new Thread(AssetBundle_UploadMethod);
		thread.Start();

		Debug.Log("开始上传  AssetBundle");
	}
	public void AssetBundle_UploadMethod()
	{
		try
		{
			foreach (string key in AssetBundle_UploadInspect.Dic_UpLoadFullPath.Keys)
			{
				Debug.Log("path    " + AssetBundle_UploadInspect.Dic_UpLoadFullPath[key]);
				using (var fs = File.Open(AssetBundle_UploadInspect.Dic_UpLoadFullPath[key], FileMode.Open))
				{
					var putObjectRequest = new PutObjectRequest(AppConst.Bucket, key, fs);
					putObjectRequest.StreamTransferProgress += UploadProgressCallback;
					client.PutObject(putObjectRequest);

					Debug.Log(string.Format("AssetBundle  {0} 上传成功：   ", key));
				}
			}
		}
		catch (System.Exception e)
		{
			Debug.Log("AssetBundle上传错误：" + e);
		}
		finally
		{
			thread.Abort();
		}
	}
	void UploadProgressCallback(object sender, StreamTransferProgressArgs args)
	{
		UploadProcess = (args.TransferredBytes * 100 / args.TotalBytes)/100;
	}


	public void DownloadAssetBundle()
	{

	}
}
