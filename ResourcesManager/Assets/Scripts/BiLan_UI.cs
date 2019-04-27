using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BiLan_UI : MonoBehaviour
{
	public Text Text_Notification;
	public Image Image_Process;

	public static BiLan_UI instance;
	private void Awake()
	{
		if (instance != null)
		{
			Debug.Log(string.Format("存在多个  {0}对象", name));
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}
	}
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		Image_Process.fillAmount = 1 - UpLoadControl.putProcess;
	}

	public void ShowNotification(string str)
	{
		Text_Notification.text = str;
	}
}
