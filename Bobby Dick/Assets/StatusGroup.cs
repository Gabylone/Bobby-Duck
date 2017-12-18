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

		statusFeedbacks [(int)status].SetColor (GetStatusColor (status));
		statusFeedbacks [(int)status].SetText ("" + count);

		statusFeedbacks [(int)status].transform.localScale = Vector3.one;
		Tween.Bounce (statusFeedbacks [(int)status].transform);
	}

	public Color goodEffectColor;
	public Color badEffectColor;
	public Color neutralEffectColor;
	Color GetStatusColor ( Fighter.Status status ) {
		
		Color color = Color.white;

		switch (status) {
		case Fighter.Status.KnockedOut:
		case Fighter.Status.Cussed:
		case Fighter.Status.Poisonned:
			
			// red
			color = badEffectColor;

			break;

		case Fighter.Status.PreparingAttack:
		case Fighter.Status.Provoking:
		
			// white
			color = neutralEffectColor;

			break;

		case Fighter.Status.Jagged:
//		case Fighter.Status.Parrying:
		case Fighter.Status.Protected:
		case Fighter.Status.Toasted:
		case Fighter.Status.Enraged:
		case Fighter.Status.BearTrapped:

			// green
			color = goodEffectColor;

			break;

		default:
			break;
		}

		return color;
	}

	void HandleOnRemoveStatus (Fighter.Status status , int count)
	{
		statusFeedbacks [(int)status].SetText ("" + count);

		if (count == 0) {
			statusFeedbacks [(int)status].Hide ();
		}
	}
}
