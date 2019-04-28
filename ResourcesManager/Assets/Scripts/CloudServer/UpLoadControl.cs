using Aliyun.OSS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using Aliyun.OSS.Common;
using System.Threading;
using System;

public class UpLoadControl : MonoBehaviour
{
	public static UpLoadControl Instance;

	private OssClient client;
	private Thread thread;

	private string fileName;
	private string localPath;

	private Action PutSuccessCallBack;
	private Action<float> PutWithProcessCallBack = null;
	public static float putProcess;
	private bool isPutSuccess = false;

	private void Awake()
	{
		Instance = this;
		client = new OssClient(AppConst.EndPoint, AppConst.AccessKeyId, AppConst.AccessKeySecret);
	}

	#region   上传
	//同步  字符串上传
	public void UpLoad_String(string fileName, string text)
	{
		try
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			using (Stream stream = new MemoryStream(bytes))
			{
				client.PutObject(AppConst.Bucket, fileName, stream);
				Debug.Log("字符串上传成功：" + text);
			}
		}
		catch (OssException e)
		{
			Debug.Log("字符串上传错误：" + e);
		}
		catch (System.Exception e)
		{
			Debug.Log("字符串上传错误：" + e);
		}
	}

	//异步   资源上传
	public void UpLoad_AssetBundle()
	{
		thread = new Thread(UpLoad_AssetBundleMethod);
		thread.Start();
	}

	public void UpLoad_AssetBundleMethod()
	{
		try
		{
			foreach (string key in AssetBundle_UploadInspect.Dic_UpLoadFullPath.Keys)
			{
				Debug.Log("path    " + AssetBundle_UploadInspect.Dic_UpLoadFullPath[key]);
				using (var fs = File.Open(AssetBundle_UploadInspect.Dic_UpLoadFullPath[key], FileMode.Open))

				{
					var putObjectRequest = new PutObjectRequest(AppConst.Bucket, key, fs);
					putObjectRequest.StreamTransferProgress += streamProgressCallback;
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

	static void streamProgressCallback(object sender, StreamTransferProgressArgs args)
	{
		putProcess = args.TransferredBytes * 100 / args.TotalBytes;
		putProcess = putProcess / 100;
	}
	#endregion

	#region 下载


	#endregion
}
