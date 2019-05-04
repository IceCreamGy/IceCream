using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AppFacade : MonoBehaviour
{
	//不同平台的接口
	public IClient Client { get; private set; }

	Dictionary<Type, BaseManager> managerDic = new Dictionary<Type, BaseManager>();

	public Transform Canvas { get; private set; }
	public Transform GoContainer { get; private set; }

	public static AppFacade instance;
	private void Awake()
	{
		if (instance != null)
		{ Debug.Log("重复的  " + this.ToString()); }
		else
		{ instance = this; }

		InitFileServer();
		Client = GetClient(Application.platform);

		DontDestroyOnLoad(this.gameObject);

		Canvas = GameObject.Find("Canvas").transform;
		DontDestroyOnLoad(Canvas.gameObject);
		GoContainer = new GameObject("GoContainer").transform;
		GoContainer.transform.SetParent(transform);

		InitManager();
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

	private void InitManager()
	{
		managerDic.Add(typeof(MsgManager), gameObject.AddComponent<MsgManager>());
		managerDic.Add(typeof(UpdateManager), gameObject.AddComponent<UpdateManager>());
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
	public SpriteManager GetSpriteManager()
	{
		return managerDic[typeof(SpriteManager)] as SpriteManager;
	}
}
