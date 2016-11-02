using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIButton : MonoBehaviour {

	[SerializeField]
	private RectTransform menu;

	[SerializeField]
	private RectTransform button;

	private bool lerping = false;

	Vector2 buttonScale;
	Vector2 menuScale;

	Vector2 buttonPos;
	Vector2 menuPos;

	float timer = 0f;

	[Header("Sounds")]
	[SerializeField] private bool playSounds = false;
	[SerializeField] private AudioClip openSound;
	[SerializeField] private AudioClip closeSound;

	[SerializeField]
	private float duration = 2f;

	bool opened = false;

	void Start () {
		buttonScale = button.sizeDelta;
		menuScale = menu.sizeDelta;

		buttonPos = button.localPosition;
		menuPos = menu.localPosition;

		menu.sizeDelta = buttonScale;
		menu.position = buttonPos;
	}

	// Update is called once per frame
	void Update () {

		if (lerping) {
			timer += Time.deltaTime;

			float l = opened ? (timer / duration) : 1 - (timer / duration);

			button.sizeDelta = Vector2.Lerp (buttonScale, menuScale, l);
			menu.sizeDelta = Vector2.Lerp (buttonScale, menuScale, l);

			button.localPosition = Vector2.Lerp (buttonPos, menuPos, l);
			menu.localPosition = Vector2.Lerp (buttonPos, menuPos, l);

			foreach ( Image image in button.GetComponentsInChildren<Image>() )
				image.color = Color.Lerp (Color.white, Color.clear, l);

			foreach ( Image image in button.GetComponentsInChildren<Image>() )
				image.color = Color.Lerp (Color.clear, Color.white, 1-l);

			if (timer >= duration ){ 
				lerping = false;

				if (opened)
					button.gameObject.SetActive (false);
				else
					menu.gameObject.SetActive (false);
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

		if ( playSounds )
			SoundManager.Instance.PlaySound ( opened ? closeSound : openSound );

		menu.gameObject.SetActive (true);
		button.gameObject.SetActive (true);

		button.GetComponent<Button> ().interactable = !opened;

	}
	#endregion


}
