using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Upload : MonoBehaviour {
	public Image FillArea;
	// Use this for initialization
	void Start()
	{
		AppFacade.instance.GetMsgManager().Register(Msg.Res_Upload_One, this, "ChangeSlider");
	}

	public void ChangeSlider(float value)
	{
		FillArea.fillAmount = value;
	}
}
