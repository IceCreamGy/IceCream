using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 提示系统
/// </summary>
public class HintPanel : MonoBehaviour
{
	public Hint_Item item;
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void AddMessage(string str)
	{
		StartCoroutine(item.Hint(str));
	}
}
