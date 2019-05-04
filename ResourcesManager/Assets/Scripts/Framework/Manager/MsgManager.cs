using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public enum Msg
{
	Res_Release_Start = 1,                            //资源开始释放
	Res_Release_One = 2,                              //单个释放成功

	CheckRes = 1000,       //检查资源

	Res_Upload_Start = 1001,                  //资源开始上传
	Res_Upload_One = 1002,                    //单个上传完成
	Res_Upload_Finish = 1003,                 //资源上传完成

	Res_Download_Start = 1004,                  //资源开始下载
	Res_Download_One = 1005,                    //单个下载完成
	Res_DownLoad_Finish = 1006,                 //资源下载完成

}

/// <summary>
/// 消息注册
/// </summary>
public class MsgRegister
{
	public object obj { get; private set; }
	public MethodInfo methodInfo { get; private set; }

	public MsgRegister(object obj, string methodName)
	{
		Type type = obj.GetType();
		MethodInfo methodInfo = type.GetMethod(methodName);
		this.obj = obj;
		this.methodInfo = methodInfo;
	}

	public void Invoke(object[] param)
	{
		if (methodInfo == null)
		{
			Debug.LogError(obj.GetType().ToString() + "    为空。      请检查调用的方法是否为 Public");
		}
		methodInfo.Invoke(obj, param);
	}
}
/// <summary>
/// 消息派发
/// </summary>
public class MsgDispatcher
{
	public Msg msg { get; private set; }
	public object[] param { get; private set; }

	public MsgDispatcher(Msg msg, object[] param)
	{
		this.msg = msg;
		this.param = param;
	}
}

//线程安全的消息分拨中心
public class MsgManager : BaseManager
{
	public readonly object locker = new object();
	public Dictionary<Msg, Dictionary<object, MsgRegister>> msg_dic = new Dictionary<Msg, Dictionary<object, MsgRegister>>();
	public Queue<MsgDispatcher> dispacherQueue = new Queue<MsgDispatcher>();

	/// <summary>
	/// 注册消息
	/// </summary>
	/// <param name="msg"></param>
	/// <param name="obj"></param>
	/// <param name="method"></param>
	public void Register(Msg msg, object obj, string methodName)
	{
		if (!msg_dic.ContainsKey(msg))
		{
			msg_dic.Add(msg, new Dictionary<object, MsgRegister>());
		}

		MsgRegister register = new MsgRegister(obj, methodName);
		msg_dic[msg][obj] = register;
	}

	/// <summary>
	/// 移除消息监听
	/// </summary>
	/// <param name="msg"></param>
	/// <param name="obj"></param>
	public void UnRegister(Msg msg, object obj)
	{
		if (msg_dic.ContainsKey(msg))
		{
			if (msg_dic[msg].ContainsKey(obj))
			{
				msg_dic[msg].Remove(obj);
			}
		}
	}

	/// <summary>
	/// 添加到消息队列
	/// </summary>
	/// <param name="msg"></param>
	/// <param name="param"></param>
	public void Broadcast(Msg msg, object[] param)
	{
		lock (locker)
		{
			MsgDispatcher dispacher = new MsgDispatcher(msg, param);
			dispacherQueue.Enqueue(dispacher);
		}
	}

	/// <summary>
	/// 广播执行
	/// </summary>
	private void Dispatch()
	{
		if (dispacherQueue.Count > 0)
		{
			lock (locker)
			{
				int count = dispacherQueue.Count;
				for (int i = 0; i < count; i++)
				{
					MsgDispatcher dispacher = dispacherQueue.Dequeue();
					Msg msg = dispacher.msg;
					object[] param = dispacher.param;
					if (msg_dic.ContainsKey(msg))
					{
						Dictionary<object, MsgRegister> dic = msg_dic[msg];
						IEnumerator iter = dic.GetEnumerator();
						while (iter.MoveNext())
						{
							KeyValuePair<object, MsgRegister> pair = (KeyValuePair<object, MsgRegister>)iter.Current;
							MsgRegister register = pair.Value;
							register.Invoke(param);
						}
					}
				}
			}
		}
	}

	private void Update()
	{
		Dispatch();
	}
}
