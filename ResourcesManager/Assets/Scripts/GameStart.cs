using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameStart : MonoBehaviour
{
	public string targetSTR;
	// Use this for initialization
	void Start()
	{
		Debug.Log(Crc32.GetCrc32(targetSTR));
	}

	// Update is called once per frame
	void Update()
	{

	}
}
