using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcons : MonoBehaviour {

	Image[] images;

	// Use this for initialization
	void Start () {

		images = GetComponentsInChildren<Image> ();

		Crews.playerCrew.onChangeCrewMembers += HandleOnChangeCrewMembers;


		CrewInventory.Instance.onOpenInventory += HandleOpenInventory;


	}

	void HandleOpenInventory (CrewMember member)
	{
		foreach (var item in images) {


		}
	}

	void HandleOnChangeCrewMembers ()
	{
		int i = 0;

		foreach (var item in images) {

			if (i < Crews.playerCrew.CrewMembers.Count) {
				item.enabled = true;
			} else {
				item.enabled = false;
			}

			i++;

		}
	}
}
