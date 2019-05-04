using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AssetCheck_UI : MonoBehaviour
{
	public GameObject Panel_UpLoad, Panel_DownLoad;
	public Panel_Hint hintPanel;
	// Use this for initialization
	void Start()
	{
		AppFacade.instance.GetMsgManager().Register(Msg.Res_Upload_Start, this, "OnRes_UploadStart");
		AppFacade.instance.GetMsgManager().Register(Msg.Res_Upload_Finish, this, "OnRes_UploadFinish");

		AppFacade.instance.GetMsgManager().Register(Msg.Res_Download_Start, this, "OnRes_DownloadStart");
		AppFacade.instance.GetMsgManager().Register(Msg.Res_DownLoad_Finish, this, "OnRes_DownloadFinish");
	}

	public void OnRes_UploadStart()
	{
		Panel_UpLoad.SetActive(true);
	}
	public void OnRes_UploadFinish()
	{
		Panel_UpLoad.SetActive(false);
	}
	public void OnRes_DownloadStart(float resSize)
	{
		//Debug.Log("需要下载的资源大小   " + resSize);
		Panel_DownLoad.SetActive(true);
	}
	public void OnRes_DownloadFinish()
	{
		Panel_DownLoad.SetActive(false);
		hintPanel.AddMessage("下载完成");
	}

	/// <summary>
	/// 从服务器对比资源
	/// </summary>
	public void OnClick_CheckAsset()
	{
		AppFacade.instance.GetMsgManager().Broadcast(Msg.CheckRes, null);
		AppFacade.instance.GetMsgManager().Broadcast(Msg.Res_Download_One, new object[] { 0 });
	}

	void Hint(string str)
	{
		hintPanel.AddMessage(str);
	}

	public void OnClick_LoadAb()
	{
		string path = Util.PathCombine(AppFacade.instance.Client.GetPersistentPath(), "scrollview-hero");
		Debug.Log("AbLoadPath  " + path);
		AssetBundle ab = AssetBundle.LoadFromFile(path);
		GameObject go = ab.LoadAsset<GameObject>("ScrollView_Hero");
		go = Instantiate(go, AppFacade.instance.Canvas);

		LoadDependents();
	}

	void LoadDependents()
	{
		string[] dependents = new string[] { "mengjun-1", "mengjun-0", "mengjun-2", "mengjun-3", "mengjun-4", "mengjun-5" };

		for (int i = 0; i < dependents.Length; i++)
		{
			string path = Util.PathCombine(AppFacade.instance.Client.GetPersistentPath(), dependents[i]);
			AssetBundle ab = AssetBundle.LoadFromFile(path);
		}
	}
}


//mengjun-1|0F22847E4ED97BDFB03AABC58E582C6B|230221     225
