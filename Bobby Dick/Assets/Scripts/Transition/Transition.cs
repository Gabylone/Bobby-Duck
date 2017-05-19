using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Transition : MonoBehaviour {

		// lerp
	private bool lerping = false;

	private float timer = 0f;
	[SerializeField]
	private float duration = 1f;

	private bool fade = false;

	[SerializeField]
	private Image targetImage;

	[SerializeField]
	private GameObject transitionCanvas;

	[SerializeField]
	private Color targetColor = Color.black;

	void Start () {
		
	}

	void Update () {
		if ( lerping ) {
			UpdateLerp ();
		}
	}

	public void Switch () {
		Fade = !Fade;
	}

	public float Duration {
		get {
			return duration;
		}
		set {
			duration = value;
		}
	}

	public Color TargetColor {
		get {
			return targetColor;
		}
		set {
			targetColor = value;
		}
	}

	private void UpdateLerp () {

		float l = timer / duration;

		targetImage.color = Color.Lerp (Color.clear, targetColor, fade ? l : 1 - l);

		timer += Time.deltaTime;
		if (timer >= duration) {

			transitionCanvas.SetActive (fade);

			lerping = false;
		}
	}


	public bool Fade {
		get {
			return fade;
		}
		set {
			fade = value;

			lerping = true;

			timer = 0f;

			transitionCanvas.SetActive (true);
		}
	}
}
