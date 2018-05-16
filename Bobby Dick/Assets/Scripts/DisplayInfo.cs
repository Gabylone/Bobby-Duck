using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Holoville.HOTween;

public class DisplayInfo : MonoBehaviour {

	public GameObject group;

	public RectTransform parentRectTransform;
	public RectTransform rectTransform;

	public Text titleText;
	public Text descriptionText;

	public GameObject confirmGroup;

	public float tweenDuration = 1f;

	public enum Corner {

		None,

		TopLeft,
		BottomLeft,
		BottomRight,
		TopRight
	}

	public virtual void Start () {

		Hide ();
	}

//	void Update () 
//	{
//		if ( Input.GetKeyDown (KeyCode.J) )
//		{
//			Move (Corner.None);
//		}
//		if ( Input.GetKeyDown (KeyCode.K) )
//		{
//			Move (Corner.BottomLeft);
//		}
//		if ( Input.GetKeyDown (KeyCode.L) )
//		{
//			Move (Corner.BottomRight);
//		}
//		if ( Input.GetKeyDown (KeyCode.M) )
//		{
//			Move (Corner.TopRight);
//		}
//		if ( Input.GetKeyDown (KeyCode.O) )
//		{
//			Move (Corner.TopLeft);
//		}
//	}

	public void Move ( Corner corner ) {

		Canvas.ForceUpdateCanvases ();
		LayoutRebuilder.ForceRebuildLayoutImmediate (rectTransform);

		switch (corner) {
		case Corner.None:
			float x = (parentRectTransform.rect.width / 2f) - (rectTransform.rect.width/2f);
			float y = -(parentRectTransform.rect.height / 2f) + (rectTransform.rect.height/2f);
			HOTween.To (rectTransform, tweenDuration, "anchoredPosition", new Vector2 (x,y)  );
			break;
		case Corner.TopLeft:
			HOTween.To (rectTransform, tweenDuration, "anchoredPosition", new Vector2 (250f, -60f) );
			break;
		case Corner.BottomLeft:
			HOTween.To (rectTransform, tweenDuration, "anchoredPosition", new Vector2 (250f, -parentRectTransform.rect.height + (rectTransform.rect.height+60f) ) );
			//			HOTween.To (rectTransform, tweenDuration, "anchoredPosition", new Vector2 (200f,- parentRectTransform.rect.height) );
			break;
		case Corner.BottomRight:
			HOTween.To (rectTransform, tweenDuration, "anchoredPosition", new Vector2 (parentRectTransform.rect.width - rectTransform.rect.width - 10 , -parentRectTransform.rect.height + (rectTransform.rect.height+60f) )  );
			break;
		case Corner.TopRight:
			HOTween.To (rectTransform, tweenDuration, "anchoredPosition", new Vector2 (parentRectTransform.rect.width - rectTransform.rect.width - 10 ,-60f) );
			break;
		}

	}

	public void Fade() {

		Tween.Bounce (group.transform);
		Tween.Fade (group.transform, Tween.defaultDuration);

		Invoke ("Hide",Tween.defaultDuration);
	}

	public void Display ( string title, string description ) {
		Show ();
		titleText.text = title;
		descriptionText.text = description;
	}

	public void Show () {

		HOTween.Kill (group.transform);
		HOTween.Kill (rectTransform);
		CancelInvoke ("Hide");

		Tween.ClearFade (group.transform);
		group.SetActive (true);

		Tween.Bounce (group.transform);
	}

	public void Hide () {

		group.SetActive (false);
		confirmGroup.SetActive (false);
	}

	public virtual void Confirm () {
		confirmGroup.SetActive (false);
		Fade ();

	}

}
