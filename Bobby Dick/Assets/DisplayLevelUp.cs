using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLevelUp : MonoBehaviour {

	[SerializeField]
	private GameObject group;

	[SerializeField]
	private Text statText;

	// Use this for initialization
	void Start () {

		group.SetActive (false);

		GetComponentInParent<MemberIcon> ().Member.onLevelUp += HandleOnLevelUp;
		GetComponentInParent<MemberIcon> ().Member.onLevelUpStat += HandleOnLevelUpStat;

	}

	void HandleOnLevelUp (CrewMember member)
	{
		Show ();

		statText.text = member.StatPoints.ToString();
	}

	void HandleOnLevelUpStat (CrewMember member)
	{
		statText.text = member.StatPoints.ToString();

		if (member.StatPoints == 0) {
			Hide ();
		}
	}

	void Show () {
		group.SetActive (true);
	}

	void Hide () {
		group.SetActive (false);
	}
}
