using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnUI : MonoBehaviour {

	public void EndTurn () {
		Game.Instance.EndPlayer ();

		Tween.Bounce (transform);
	}
}
