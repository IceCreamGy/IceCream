using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Msg
{
	Res_Release_Start = 1,                            //资源开始释放
	Res_Release_One = 2,                              //单个释放成功

	Res_Download_Start = 1001,                  //资源开始下载
	Res_Download_One = 1002,                    //单个下载完成
	Res_DownLoad_Finish = 1003,                 //资源下载完成

}

public class MsgManager : BaseManager
{
	/// <summary>
	/// 注册消息
	/// </summary>
	/// <param name="msg"></param>
	/// <param name="obj"></param>
	/// <param name="method"></param>
	public void Register(Msg msg,object obj,string method)
	{

	}

	/// <summary>
	/// 广播消息
	/// </summary>
	/// <param name="msg"></param>
	/// <param name="param"></param>
	public void Broadcast(Msg msg, object[] param)
	{

	}
}
