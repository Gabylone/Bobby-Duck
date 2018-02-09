using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class InfoFeedbacks : MonoBehaviour {

	public GameObject group;

	public RectTransform rectTransform;
	public Text text;
	public Image image;

	public float decalY = 30f;

	public float duration = 2f;

	// Use this for initialization
	public virtual void Start () {

		Hide ();

	}

	public virtual void Print ( string str ) {
		Print (str, Color.white);
	}
	public virtual void Print ( string str , Color color ) {
		
		Show ();

		Tween.ClearFade (transform);

		rectTransform.localPosition = Vector3.zero;

		HOTween.Kill (rectTransform);
		HOTween.Kill (transform);

		CancelInvoke ("Fade");

		HOTween.To (rectTransform, duration, "localPosition", Vector3.up * decalY);

		text.text = str;
		image.color = color;

		Invoke ("Fade",duration/2f);
	}

	void Fade () {
		Tween.Fade (transform, duration/2f);

		Invoke ("Hide", duration/2f);
	}

	void Show () {
		group.SetActive (true);
	}

	void Hide () {
		group.SetActive (false);
	}
}
