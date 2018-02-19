﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class FightBackground : MonoBehaviour {

	public GameObject group;

	public Image image;

	public float fadeDuration = 1f;

	// Use this for initialization
	void Start () {
		CombatManager.Instance.onFightStart += HandleFightStarting;
		CombatManager.Instance.onFightEnd += HandleFightEnding;
	}

	void HandleFightEnding ()
	{
		HOTween.To (image, fadeDuration, "color", Color.clear);
		Invoke ("Hide", fadeDuration);

	}

	void HandleFightStarting ()
	{
		Show ();

		print ("heh");

		image.color = Color.clear;
		HOTween.To (image, fadeDuration, "color", Color.white);
	}

	void Show () {
		group.SetActive (true);
	}

	void Hide () {
		group.SetActive (false);
	}
}