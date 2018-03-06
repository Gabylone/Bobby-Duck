using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class UIBackground : MonoBehaviour {

	RectTransform rectTransform;

	public float initXPos = 0f;
	public float skillX = 0f;
	public float hiddenX = 0f;

	public float duration = 0.3f;

	public GameObject uiGroup;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform> ();

		CombatManager.Instance.onFightStart += MoveBackGround;
		CombatManager.Instance.onFightEnd += HandleFightEnding;

		initXPos = rectTransform.rect.position.x;
	}

	void HandleFightEnding ()
	{
		ShowBackGround ();

		Invoke("HandleFightEndingDelay" , duration);
	}

	void HandleFightEndingDelay () {
		Crews.playerCrew.UpdateCrew (Crews.PlacingType.Map);
	}

	void ShowBackGround ()
	{
		HOTween.To ( rectTransform  , duration , "anchoredPosition" , new Vector2 ( initXPos , 0f ) );

		uiGroup.SetActive (true);
	}

	void MoveBackGround ()
	{
		HOTween.To ( rectTransform  , duration , "anchoredPosition" , new Vector2 ( skillX , 0f ) );

		uiGroup.SetActive (false);
	}

	void HideBackground ()
	{
		HOTween.To ( rectTransform  , duration , "anchoredPosition" , new Vector2 ( hiddenX , 0f ) );

		uiGroup.SetActive (false);

	}
}
