using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Transition : MonoBehaviour {

	bool lerping = false;

	float timer = 0f;
	[SerializeField]
	private float duration = 1f;

	bool isFaded = false;

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

			timer += Time.deltaTime;

			float l = isFaded ? (timer / duration) : 1-(timer / duration);

			targetImage.color = Color.Lerp ( Color.clear , targetColor , l );

			if (timer >= duration) {
				
				if (!isFaded)
					transitionCanvas.SetActive (false);
				
				lerping = false;
			}

		}
	}

	public void Switch () {
		transitionCanvas.SetActive (true);

		timer = 0f;

		isFaded = !isFaded;
		lerping = true;
	}

	public void QuickSwitch () {
		targetImage.color = isFaded ? Color.clear : Color.black;
		isFaded = !isFaded;
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
}
