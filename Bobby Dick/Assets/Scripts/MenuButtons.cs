using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class MenuButtons : MonoBehaviour {

	public GameObject group;
	public GameObject[] buttonObjs;
	public float tweenDuration = 1f;
	public float timeBetweenDisplay = 0.15f;
	public float timeBetweenDisplay_Hide = 0.03f;
	public RectTransform rectTransform;

	public bool opened = false;

	void Start () {
        Show();

        CombatManager.Instance.onFightStart += Hide;
        CombatManager.Instance.onFightEnd += Show;
    }

	public void Hide ()
	{
		opened = false;

		Invoke ("HideDelay", tweenDuration);

		Vector2 targetPos = rectTransform.anchoredPosition;

		targetPos.x = 100f;

		HOTween.To ( rectTransform , tweenDuration , "anchoredPosition" , targetPos );


	}
	void HideDelay () {
		group.SetActive (false);
	}

	public void Show ()
	{
		opened = true;

		Vector2 targetPos = rectTransform.anchoredPosition;

		group.SetActive (true);

		targetPos.x = 0f;

		HOTween.To ( rectTransform , tweenDuration , "anchoredPosition", targetPos);


    }
}
