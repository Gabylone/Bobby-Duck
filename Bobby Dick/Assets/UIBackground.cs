using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class UIBackground : MonoBehaviour {

	RectTransform rectTransform;

	float initXPos = 0f;

	float duration = 1f;

	public float targetX = 0f;

	public GameObject uiGroup;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform> ();

		CombatManager.Instance.onFightStart += HandleOnFightStart;
		CombatManager.Instance.onFightEnd += HandleOnFightEnd;

		initXPos = rectTransform.rect.position.x;
	}

	void HandleOnFightEnd ()
	{
		HOTween.To ( rectTransform  , duration , "anchoredPosition" , new Vector2 ( -initXPos , 0f ) );

//		uiGroup.SetActive (true);
	}

	void HandleOnFightStart ()
	{
		HOTween.To ( rectTransform  , duration , "anchoredPosition" , new Vector2 ( 0f , 0f ) );

//		uiGroup.SetActive (false);

	}
}
