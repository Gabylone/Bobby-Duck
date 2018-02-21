using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Holoville.HOTween;

public class DisplayHunger : MonoBehaviour {

	public GameObject hungerGroup;
	public Image fillImage;
	float maxFillAmountScale = 1f;
	public Image backGroundImage;

	public virtual void Start () {
//		HideHunger ();

		maxFillAmountScale = fillImage.rectTransform.rect.height;
	}

	public void Show () {
		hungerGroup.SetActive (true);
	}
	public void HideHunger () {
		hungerGroup.SetActive (false);
	}

	public virtual void UpdateHungerIcon ( CrewMember member ) {

		Show ();
		HOTween.Kill (fillImage.rectTransform);

		float fillAmount = 1f - ((float)member.CurrentHunger / (float)member.maxHunger);
		Vector2 v = new Vector2 (fillImage.rectTransform.rect.width, (fillAmount * maxFillAmountScale));
		HOTween.To ( fillImage.rectTransform , 0.5f , "sizeDelta" , v );

		Tween.Bounce (transform, 0.2f, 1.1f);

	}
}
