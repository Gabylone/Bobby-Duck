using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombatFeedback : MonoBehaviour {

	public Text text;

	public float fadeDuration = 1f;

	public GameObject group;

	public Image backgroundImage;

	// Use this for initialization
	void Start () {
		Hide ();
	}

	bool displaying;

	public void Display (string content) {
		Display (content, Color.white);
	}

	public void Display (string content, Color color) {

		Show ();

		Tween.Bounce (transform);

//		backgroundImage.color = color;

		text.text = content;
//		if (color != Color.white) {
//			text.color = Color.black;
//		} else {
//			text.color = Color.black;
//		}

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
