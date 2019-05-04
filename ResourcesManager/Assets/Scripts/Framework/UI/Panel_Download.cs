using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class Panel_Download : MonoBehaviour
{
	public Image FillArea;
	public Text text_ShowContent;
	StringBuilder sb = new StringBuilder();
	// Use this for initialization
	void Start()
	{
		AppFacade.instance.GetMsgManager().Register(Msg.Res_Download_One, this, "ChangeSlider");
	}

	public void ChangeSlider(float value, string itemName)
	{
		sb.AppendLine(itemName);
		FillArea.fillAmount = value;
		text_ShowContent.text = sb.ToString();
	}
}
