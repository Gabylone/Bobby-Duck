using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusGroup : MonoBehaviour {

	int statusCount = 0;

	StatusFeedback [] statusFeedbacks;

	// Use this for initialization
	void Start () {
		
		GetComponentInParent<Card> ().linkedFighter.onAddStatus += HandleOnAddStatus;
		GetComponentInParent<Card> ().linkedFighter.onRemoveStatus += HandleOnRemoveStatus;
		GetComponentInParent<Card> ().linkedFighter.onShowInfo += HandleOnShowInfo;
		GetComponentInParent<Card> ().onHideInfo += HandleOnHideInfo;

		statusFeedbacks = GetComponentsInChildren<StatusFeedback> (true);

		foreach (var item in statusFeedbacks) {
			item.gameObject.SetActive(false);
		}
	}

	void HandleOnShowInfo ()
	{
		Hide ();
	}

	void HandleOnHideInfo ()
	{
		Show ();
	}

	void Show() {
		gameObject.SetActive (true);
	}
	void Hide () {
		gameObject.SetActive (false);
		//
	}

	void HandleOnAddStatus (Fighter.Status status, int count)
	{
		if ((int)status >= statusFeedbacks.Length) {
			print (status.ToString () + " doesct fit in feedbacks ( L : " + statusFeedbacks.Length + ")");
		}
		statusFeedbacks [(int)status].gameObject.SetActive (true);

//		statusFeedbacks [(int)status].SetSprite (CombatManager.statusSprites [(int)status]);

		statusFeedbacks [(int)status].SetText ("" + count);

		Tween.Bounce (statusFeedbacks [(int)status].transform);
	}

	void HandleOnRemoveStatus (Fighter.Status status , int count)
	{
		statusFeedbacks [(int)status].SetText ("" + count);

		if (count == 0) {
			statusFeedbacks [(int)status].gameObject.SetActive (false);
		}
	}
}
