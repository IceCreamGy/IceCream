using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
	public MsgManager MsgManager { get { return AppFacade.instance.GetMsgManager(); } }
	public IClient Client { get { return AppFacade.instance.Client; } }
}
