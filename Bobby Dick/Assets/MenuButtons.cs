using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class MenuButtons : MonoBehaviour {

	public GameObject group;
	public GameObject[] buttonObjs;
	public float tweenDuration = 1f;
	public float timeBetweenDisplay = 0.15f;
	public float timeBetweenDisplay_Hide = 0.03f;
	public RectTransform rectTransform;

	public bool opened = false;

	void Start () {

//		HideAll ();
		HideDelay();
	}

	public void Hide ()
	{
		opened = false;

		Invoke ("HideDelay", tweenDuration);


		Vector2 targetPos = rectTransform.anchoredPosition;

		targetPos.x = 100f;

		HOTween.To ( rectTransform , tweenDuration , "anchoredPosition" , targetPos );

//		StartCoroutine (HideCoroutine ());

	}
	void HideDelay () {
		group.SetActive (false);
	}
//	public void Hide () {
//		foreach (var item in buttonObjs) {
//			item.SetActive (false);
//		}
//	}
	public void Show ()
	{
		opened = true;

		Vector2 targetPos = rectTransform.anchoredPosition;

		group.SetActive (true);

		targetPos.x = 0f;

		HOTween.To ( rectTransform , tweenDuration , "anchoredPosition", targetPos);
//		HOTween.To ( rectTransform , tweenDuration , "anchoredPosition" , targetPos , false , EaseType.EaseOutBounce , 0f);

//		StartCoroutine (ShowCoroutine ());
	}

	IEnumerator ShowCoroutine ()
	{
		foreach (var item in buttonObjs) {
			item.SetActive (true);
			Tween.ClearFade(item.transform);
			Tween.Bounce (item.transform, timeBetweenDisplay, 1.1f);
//			LayoutRebuilder.ForceRebuildLayoutImmediate (rectTransform);
			yield return new WaitForSeconds ( timeBetweenDisplay );
		}
		yield return new WaitForEndOfFrame ();
	}

	IEnumerator HideCoroutine ()
	{
		for (int i = 0; i < buttonObjs.Length; i++) {

			int index = buttonObjs.Length -1 - i;

			//			Tween.Bounce (buttonObjs [index].transform, timeBetweenButtonDisplay , 0.f);
			Tween.Fade (buttonObjs[index].transform, timeBetweenDisplay);

			yield return new WaitForSeconds ( timeBetweenDisplay_Hide );
			//
			//			if (index > 0) {
			//				buttonObjs[index-1].SetActive(false);
			//				LayoutRebuilder.ForceRebuildLayoutImmediate (rectTransform);
			//				Tween.ClearFade(buttonObjs[index].transform);
			//			}

		}

		yield return new WaitForEndOfFrame ();

		HideAll ();

	}


	void HideAll () {
		foreach (var item in buttonObjs) {
			item.SetActive (false);
		}
	}
}
