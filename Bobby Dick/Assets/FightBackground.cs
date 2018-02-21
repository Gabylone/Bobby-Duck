using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class FightBackground : MonoBehaviour {

	public GameObject group;

	public Image backGroundImage;
	public Image darkImage;

	public float fadeDuration = 1f;

	// Use this for initialization
	void Start () {
		CombatManager.Instance.onFightStart += HandleFightStarting;
		CombatManager.Instance.onFightEnd += HandleFightEnding;
	}

	void HandleFightEnding ()
	{
		HOTween.To (backGroundImage, fadeDuration, "color", Color.clear);
		HOTween.To (darkImage, fadeDuration, "color", Color.clear);

		Invoke ("Hide", fadeDuration);

	}

	void HandleFightStarting ()
	{
		Show ();

		backGroundImage.color = Color.clear; 
		HOTween.To (backGroundImage, fadeDuration, "color", Color.white);
		HOTween.To (darkImage, fadeDuration, "color", Color.black);
	}

	void Show () {
		group.SetActive (true);
	}

	void Hide () {
		group.SetActive (false);
	}
}
