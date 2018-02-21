using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMenuButton : MonoBehaviour {

	public Sprite disabledSprite;
	public Sprite enabledSprite;

	public Image image;
	public Image buttonImage;

	void Start () {
		
		UpdateButton ();

	}

	public void OnActivate () {

		KeepOnLoad.displayTuto = !KeepOnLoad.displayTuto;

		Tween.Bounce (transform, 0.2f , 1.05f);

		UpdateButton ();

		//
	}

	void UpdateButton() {

		if ( KeepOnLoad.displayTuto ) {
			image.sprite = enabledSprite;
			buttonImage.color = Color.green;
		} else {
			image.sprite = disabledSprite;
			buttonImage.color = Color.red;
		}

	}
}
