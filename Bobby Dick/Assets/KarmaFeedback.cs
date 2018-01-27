using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class KarmaFeedback : MonoBehaviour {

	public GameObject group;

	public RectTransform rectTransform;
	public Text text;
	public Image image;

	public float decalY = 30f;

	public float duration = 2f;

	// Use this for initialization
	void Start () {
		Karma.onChangeKarma += HandleOnChangeKarma;
		Hide ();
	}

	void HandleOnChangeKarma (int previousKarma, int newKarma)
	{
		Show ();

		Tween.ClearFade (transform);

		rectTransform.localPosition = Vector3.zero;
		HOTween.Kill (rectTransform);
		HOTween.Kill (transform);
		CancelInvoke ("Fade");
		HOTween.To (rectTransform, duration, "localPosition", Vector3.up * decalY);

		if (newKarma > previousKarma) {
			text.text = "Bonne Action !";
			image.color = Color.green;

		} else {
			text.text = "Mauvaise Action !";
			image.color = Color.red;
		}

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
