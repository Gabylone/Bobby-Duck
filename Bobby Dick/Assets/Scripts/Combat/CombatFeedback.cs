using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

public class CombatFeedback : MonoBehaviour {

	public Text text;
	public Image statusImage;
	public GameObject group;
	public Image backgroundImage;

	public float fadeDecal = 1f;
	public float fadeDuration = 1f;

	public CombatFeedback secondCombatFeedback;
	float secondFeedbackDelay = 1.2f;

	bool displaying = false;

	Vector3 initPos;

	// Use this for initialization
	void Start () {
		Hide ();

		initPos = transform.localPosition;
	}

	// status
	public void Display (Fighter.Status status) {
		Display (status, Color.white);
	}
	public void Display (Fighter.Status status, Color color) {
		Display (status, color, 0f);
	}
	public void Display (Fighter.Status status, Color color, float delay) {

		if ( displaying ) {
			if (secondCombatFeedback != null) {
				secondCombatFeedback.Display (status, color, secondFeedbackDelay);
			}
			else
				Debug.LogError ("tenté de display combat feedback mais le second était nul");
		}

		displaying = true;

		backgroundImage.color = color;
		statusImage.sprite = SkillManager.statusSprites [(int)status];
	
		statusImage.gameObject.SetActive (true);
		text.gameObject.SetActive (false);

		Invoke ("DisplayDelay", delay);
	}
	//

	// text
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
		}

		displaying = true;

		backgroundImage.color = color;
		text.text = content;

		text.gameObject.SetActive (true);
		statusImage.gameObject.SetActive (false);

		Invoke ("DisplayDelay", delay);
	}

	void DisplayDelay () {
		
		Show ();

		Tween.ClearFade (transform);
		Tween.Bounce (transform);

		transform.localPosition = initPos;

		HOTween.To (transform, fadeDuration, "localPosition", initPos + Vector3.up * fadeDecal);

		Invoke ("Fade",fadeDuration/2f);
		Invoke ("Hide", fadeDuration);

	}

	void Fade() {
		Tween.Fade (transform, fadeDuration/2f);
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
