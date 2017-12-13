using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

public class CombatFeedback : MonoBehaviour {

	public Text text;

	public float fadeDuration = 1f;

	public GameObject group;

	public float decalUp = 1f;

	public Image backgroundImage;

	Vector3 initPos;

	// Use this for initialization
	void Start () {
		Hide ();

		initPos = transform.localPosition;
	}

	bool displaying;

	public void Display (string content) {
		Display (content, Color.white);
	}

	public void Display (string content, Color color) {

		HOTween.Kill (transform);
		HOTween.Kill (text);
		HOTween.Kill (backgroundImage);

		Show ();

		Tween.Bounce (transform);

		transform.localPosition = initPos;
		HOTween.To (transform , fadeDuration , "localPosition" , initPos + Vector3.up * decalUp);

		text.text = content;
		text.color = Color.black;
		HOTween.To (text, fadeDuration/2, "color", Color.clear, false , EaseType.Linear , fadeDuration/2);

		backgroundImage.color = color;
		HOTween.To (backgroundImage, fadeDuration/2, "color", Color.clear, false , EaseType.Linear , fadeDuration/2);

		Invoke ("Hide", fadeDuration);

	}

	void Show () {
		displaying = true;
		group.SetActive (true);
		Tween.Bounce (transform);
	}

	void Hide () {
		displaying = false;
		group.SetActive (false);
	}

}
