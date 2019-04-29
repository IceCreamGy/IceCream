using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Text;
using Aliyun.OSS;
using Aliyun.OSS.Common;
using System.IO;

public struct UploadItem
{
	public string FileSaveName;
	public string FIlePath;
}

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

	/// <summary>
	/// 上传字符串
	/// </summary>
	/// <param name="fileName"></param>
	/// <param name="text"></param>
	public void Upload_String(string fileName, string text)
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

	string FileSaveName, FileLocalPath;
	/// <summary>
	/// 异步上传单个文件
	/// </summary>
	/// <param name="fileSaveName"></param>
	/// <param name="fileLocalPath"></param>
	public void Upload_FileByPath(string fileSaveName, string fileLocalPath)
	{
		FileSaveName = fileSaveName;
		FileLocalPath = fileLocalPath;
		thread = new Thread(UploadFile);
		thread.Start();
	}
	void UploadFile()
	{
		try
		{
			using (var fs = File.Open(FileLocalPath, FileMode.Open))
			{
				var putObjectRequest = new PutObjectRequest(AppConst.Bucket, FileSaveName, fs);
				putObjectRequest.StreamTransferProgress += Upload_ProgressCallback;
				client.PutObject(putObjectRequest);
			}
		}
		catch (System.Exception e)
		{
			Debug.Log("上传错误：" + e);
		}
		finally
		{
			thread.Abort();
		}
	}

	UploadItem[] ItemArray;
	/// <summary>
	/// 批量异步上传
	/// </summary>
	/// <param name="itemArray"></param>
	public void Upload_Files(UploadItem[] itemArray)
	{
		ItemArray = itemArray;
		thread = new Thread(UploadFiles);
		thread.Start();
	}
	void UploadFiles()
	{
		try
		{
			for (int i = 0; i < ItemArray.Length; i++)
			{
				using (var fs = File.Open(ItemArray[i].FIlePath, FileMode.Open))
				{
					var putObjectRequest = new PutObjectRequest(AppConst.Bucket, ItemArray[i].FileSaveName, fs);
					putObjectRequest.StreamTransferProgress += Upload_ProgressCallback;
					client.PutObject(putObjectRequest);
				}
			}
		}
		catch (System.Exception e)
		{
			Debug.Log("上传错误：" + e);
		}
		finally
		{
			thread.Abort();
		}
	}

	/// <summary>
	/// 上传进度 回调
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	void Upload_ProgressCallback(object sender, StreamTransferProgressArgs args)
	{
		UploadProcess = (args.TransferredBytes * 100 / args.TotalBytes) / 100;
	}


	public void DownloadAssetBundle()
	{

	}
}
