using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class StatusFeedback : MonoBehaviour {

	public Image image;

	public Text uiText;

	public GameObject textGroup;

	public float tweenDur = 0.5f;
	public float tweenScaleAmount = 1.2f;

	public void SetCount ( int count ) {

		if (count > 1) {
			textGroup.SetActive (true);
			uiText.text = count.ToString ();
		} else {
			textGroup.SetActive (false);
		}

	}

	public void SetSprite ( Sprite sprite ) {
		image.sprite = sprite;
	}

	public void SetColor (Color color)
	{
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
//		Tween.Bounce(transform,tweenDur,1.2f);
		Invoke ("HideDelay" , tweenDur);
	}

	void HideDelay () {
		gameObject.SetActive (false);


	}
}
