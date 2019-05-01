using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AppFacade : MonoBehaviour
{
	//不同平台的接口
	public IClient Client { get; private set; }

	Dictionary<Type, BaseManager> managerDic = new Dictionary<Type, BaseManager>();

	public static AppFacade instance;
	private void Awake()
	{
		if (instance != null)
		{ Debug.Log("重复的  " + this.ToString()); }
		else
		{ instance = this; }

		InitFileServer();
		Client = GetClient(Application.platform);
	}
	// Use this for initialization
	void Start()
	{

	}

	private void InitFileServer()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("file_server");
		AppConst.Res_Download_Address = textAsset.text;
	}

	private IClient GetClient(RuntimePlatform platform)
	{
		IClient client = null;
		switch (platform)
		{
			case RuntimePlatform.WindowsPlayer:
				client = new WindowsClient();
				break;
			case RuntimePlatform.WindowsEditor:
				client = new WindowsClient();
				break;
			case RuntimePlatform.Android:
				client = new AndroidClient();
				break;
			case RuntimePlatform.IPhonePlayer:
				client = new IOSClient();
				break;
		}

		return client;
	}

	public MsgManager GetMsgManager()
	{
		return managerDic[typeof(MsgManager)] as MsgManager;
	}
}
