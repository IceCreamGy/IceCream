using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour {

	public IClient Client { get { return AppFacade.instance.Client; } }
}
