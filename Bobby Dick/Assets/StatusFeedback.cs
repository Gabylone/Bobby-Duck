using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusFeedback : MonoBehaviour {

	public Image image;

	public Text uiText;

	public void SetText ( string text ) {
		uiText.text = text;
	}

	public void SetSprite ( Sprite sprite ) {
		image.sprite = sprite;
	}

}
