using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombatFeedback : MonoBehaviour {

	Animator animator;

	public Text text;

	public float fadeDuration = 1f;

	Color initColor;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		text = GetComponentInChildren<Text> ();
	}

	float timer;

	void Update () {

		if ( displaying ) {

			text.color = Color.Lerp (initColor, Color.clear, timer / fadeDuration);

			timer += Time.deltaTime;

			if (timer >= fadeDuration) {
				displaying = false;
				gameObject.SetActive (false);
			}
		}
	}

	bool displaying;
	
	public void Display (string content) {
		Display (content, Color.white);
	}
	public void Display (string content, Color color) {
		text.color = color;
		text.text = content;

		initColor = text.color;

		displaying = true;

		timer = 0f;

		gameObject.SetActive (true);

	}

}
