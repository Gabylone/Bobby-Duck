using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class KarmaFeedback : InfoFeedbacks {

	public override void Start ()
	{
		base.Start ();

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
