using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AssetCheck_UI : MonoBehaviour
{
	public HintPanel hintPanel;
	// Use this for initialization
	void Start()
	{
		AppFacade.instance.GetMsgManager().Register(Msg.Res_DownLoad_Finish, this, "OnDownloadFinish");
	}

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// 从服务器对比资源
	/// </summary>
	public void OnClick_CheckAsset()
	{
		AppFacade.instance.GetMsgManager().Broadcast(Msg.CheckRes, null);
	}

	void Hint(string str)
	{
		hintPanel.AddMessage(str);
	}

	public void OnDownloadFinish()
	{
		hintPanel.AddMessage("下载完成");
	}
}
