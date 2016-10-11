using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapUI : MonoBehaviour {

	[SerializeField]
	private Image scrollView;

	[SerializeField]
	private Image mapButton;

	private bool lerping = false;

	Vector2 buttonScale;
	Vector2 menuScale;

	Vector2 buttonPos;
	Vector2 menuPos;

	float timer = 0f;

	[SerializeField]
	private float duration = 2f;

	bool opened = false;

	void Start () {
		buttonScale = mapButton.rectTransform.sizeDelta;
		menuScale = scrollView.rectTransform.sizeDelta;

		buttonPos = mapButton.rectTransform.localPosition;
		menuPos = scrollView.rectTransform.localPosition;

		scrollView.rectTransform.sizeDelta = buttonScale;
		scrollView.rectTransform.position = buttonPos;
	}

	// Update is called once per frame
	void Update () {

		if (lerping) {
			timer += Time.deltaTime;

			float l = opened ? (timer / duration) : 1 - (timer / duration);

			mapButton.rectTransform.sizeDelta = Vector2.Lerp (buttonScale, menuScale, l);
			scrollView.rectTransform.sizeDelta = Vector2.Lerp (buttonScale, menuScale, l);

			mapButton.rectTransform.localPosition = Vector2.Lerp (buttonPos, menuPos, l);
			scrollView.rectTransform.localPosition = Vector2.Lerp (buttonPos, menuPos, l);

			mapButton.color = Color.Lerp (Color.white, Color.clear, l);
			scrollView.color = Color.Lerp (Color.clear, Color.white, 1-l);

			if (timer >= duration ){ 
				lerping = false;

				if (opened)
					mapButton.gameObject.SetActive (false);
				else
					scrollView.gameObject.SetActive (false);
			}
		}


	}

	#region ui events
	public void Switch () {

		if (lerping)
			return;

		timer = 0f;

		lerping = true;
		opened = !opened;

		scrollView.gameObject.SetActive (true);
		mapButton.gameObject.SetActive (true);

		mapButton.GetComponent<Button> ().interactable = !opened;

	}
	#endregion


}
