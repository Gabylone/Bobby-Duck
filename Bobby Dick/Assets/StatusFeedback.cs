using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class StatusFeedback : MonoBehaviour {

	public Image image;

	public Text uiText;

	public float tweenDur = 0.5f;
	public float tweenScaleAmount = 1.2f;

	public void SetText ( string text ) {
		uiText.text = text;
	}

	public void SetSprite ( Sprite sprite ) {
		image.sprite = sprite;
	}

	public void SetColor (Color color)
	{
//		image.color = color;
		GetComponent<Image>().color = color;
		int a = 0;
		foreach (var item in GetComponentsInChildren<Image>()) {
			if ( a > 0 )
				HOTween.To ( item , tweenDur , "color" , Color.black );
			++a;
		}
		uiText.color = Color.white;
	}

	public void Hide (){
		HOTween.To ( transform , tweenDur , "localScale" , Vector3.one * tweenScaleAmount );

		foreach (var item in GetComponentsInChildren<Image>()) {
			HOTween.To ( item , tweenDur , "color" , Color.clear );
		}

		HOTween.To ( uiText , tweenDur , "color" , Color.clear );

		Invoke ("HideDelay" , tweenDur);
	}

	void HideDelay () {
		gameObject.SetActive (false);


	}
}
