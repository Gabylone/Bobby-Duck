using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class KarmaFeedback : InfoFeedbacks {

	// Use this for initialization
	public override void Print (string str, Color color)
	{
		base.Print (str, color);

		Karma.onChangeKarma += HandleOnChangeKarma;
	}

	void HandleOnChangeKarma (int previousKarma, int newKarma)
	{
		if (newKarma > previousKarma) {
			Print ("Bonne Action", Color.green);
		} else {
			Print ("Mauvaise Action", Color.red);
		}

	}
}
