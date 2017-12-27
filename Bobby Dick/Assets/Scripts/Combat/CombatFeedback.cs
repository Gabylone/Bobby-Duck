using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

public class CombatFeedback : MonoBehaviour {

	public Text text;

	public float fadeDuration = 1f;


	public CombatFeedback secondCombatFeedback;
	float secondFeedbackDelay = 0.8f;

	public GameObject group;

	public float decalUp = 1f;

	public Image backgroundImage;

	bool displaying = false;

	bool right = false;

	Vector3 initPos;

	// Use this for initialization
	void Start () {
		Hide ();

		initPos = transform.localPosition;

		right = transform.position.x > 0;
	}

	public void Display (string content) {
		Display (content, Color.white);
	}

	public void Display (string content, Color color) {
		Display (content, color, 0f);
	}

	public void Display (string content, Color color, float delay) {

		if ( displaying ) {
			if (secondCombatFeedback != null) {
				secondCombatFeedback.Display (content, color, secondFeedbackDelay);
			}
			else
				Debug.LogError ("tenté de display combat feedback mais le second était nul");

			return;

		}

		displaying = true;

		text.text = content;
		backgroundImage.color = color;

		Invoke ("DisplayDelay", delay);
	}
//
	void DisplayDelay () {
		
		Show ();

		Tween.Bounce (transform);

		transform.localPosition = initPos;

		if (right) {
			HOTween.To (transform, fadeDuration, "localPosition", initPos + Vector3.right * decalUp);
		} else {
			HOTween.To (transform, fadeDuration, "localPosition", initPos + Vector3.left * decalUp);
		}

		text.color = Color.black;
		HOTween.To (text, fadeDuration/2, "color", Color.clear, false , EaseType.Linear , fadeDuration/2);

		HOTween.To (backgroundImage, fadeDuration/2, "color", Color.clear, false , EaseType.Linear , fadeDuration/2);

		Invoke ("Hide", fadeDuration);

	}

	void Show () {
		group.SetActive (true);
		Tween.Bounce (transform);
	}

	void Hide () {
		
		displaying = false;

		group.SetActive (false);

	}

}
