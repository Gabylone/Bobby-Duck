using System;
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
		CrewInventory.Instance.onCloseInventory += HandleOnCloseInventory;

        HandleOnChangeCrewMembers();
	}

    private void HandleOnCloseInventory()
    {
        foreach (var item in images)
        {
            item.color = Color.white;
        }
    }

    void HandleOpenInventory (CrewMember member)
	{
		foreach (var item in images) {
            item.color = Color.grey;
		}


        int id = Crews.playerCrew.CrewMembers.FindIndex(x => x.MemberID.SameAs(CrewMember.GetSelectedMember.MemberID));
        if ( id < 0)
        {
            Debug.Log("pas select");
        }

        images[id].color = Color.white;
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
