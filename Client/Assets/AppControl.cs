using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AppControl : MonoBehaviour
{
	public Transform Container;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			DownLoadControl.instance.DownloadAssetBundle(OnLoadComplete);
		}
	}

	void OnLoadComplete()
	{
		AssetBundle ab = AssetBundle.LoadFromFile(DownLoadControl.instance.LocalPath);
		GameObject go = ab.LoadAsset<GameObject>("ScrollView-Shop");
		Instantiate(go, Container);
	}
}
