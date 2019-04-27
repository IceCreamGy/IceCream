using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiLan_Control : MonoBehaviour
{
	public Transform UiCanvas;
	public Transform GirlContainer;
	// Use this for initialization
	void Start()
	{
		UiCanvas = GameObject.Find("Canvas").GetComponent<Transform>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			BiLan_UI.instance.ShowNotification("开始上传");
			//UpLoadControl.Instance.UpLoad_Image(OnUpLoadComplete,"Z23.jpg", "Arts/UI/Girl-2.jpg");
			UpLoadControl.Instance.UpLoad_AssetBundle();
		}
	}

	void OnUpLoadComplete()
	{
		BiLan_UI.instance.ShowNotification("开始完成");
	}

	public void LoadShop()
	{
		AssetBundle ab1 = AssetBundle.LoadFromFile(GetAllName("girl-1"));
		GameObject GoGIrl1 = ab1.LoadAsset<GameObject>("Image-1");
		Instantiate(GoGIrl1, GirlContainer);

		AssetBundle ab2 = AssetBundle.LoadFromFile(GetAllName("girl-2"));
		GameObject GoGIrl2 = ab2.LoadAsset<GameObject>("Image-2");
		Instantiate(GoGIrl2, GirlContainer);

		AssetBundle ab3 = AssetBundle.LoadFromFile(GetAllName("girl-3"));
		GameObject GoGilr3 = ab3.LoadAsset<GameObject>("Image-3");
		Instantiate(GoGilr3, GirlContainer);
	}

	string GetAllName(string str)
	{
		return Application.streamingAssetsPath + "/" + str;
	}
}
