using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatButtons : MonoBehaviour {

	public GameObject feedbackGroup;
	public Text feedbackText;

	public GameObject group;

	// Use this for initialization
	void Start () {
		CombatManager.Instance.onChangeState += HandleOnChangeState;

		group.SetActive (false);
		feedbackGroup.SetActive (false);
	}

	void HandleOnChangeState (CombatManager.States currState, CombatManager.States prevState)
	{
		group.SetActive (false);
		feedbackGroup.SetActive (false);

		if ( currState == CombatManager.States.PlayerAction ) {
			Tween.Bounce (group.transform);
			group.SetActive (true);

			DisplayFeedback ("Choisir action");
		}

		if ( currState == CombatManager.States.PlayerMemberChoice ) {
			DisplayFeedback ("Choisir cible");
		}
	}

	void DisplayFeedback (string str) {
		feedbackGroup.SetActive (true);
		feedbackText.text = str;
		Tween.Bounce (feedbackGroup.transform);
	}
}
