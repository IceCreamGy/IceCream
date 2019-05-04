using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Hint_Item : MonoBehaviour
{
	public RectTransform Rt;
	public CanvasGroup Cg;
	public Text HintText;

	float small = 0.8f;
	public IEnumerator Hint(string str)
	{
		HintText.text = str;
		Rt.localScale = new Vector3(small, small, small);
		Rt.DOScale(1, 0.4f).SetEase(Ease.OutBack);
		Cg.alpha = 0;
		Cg.DOFade(1, 0.4f);

		yield return new WaitForSeconds(1.5f);
		Cg.DOFade(0, 0.4f);
	}
}
