using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcons : MonoBehaviour {

    public static PlayerIcons Instance;

	public Image[] images;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {

		//images = GetComponentsInChildren<Image> ();

		Crews.playerCrew.onChangeCrewMembers += HandleOnChangeCrewMembers;

		InGameMenu.Instance.onOpenMenu += HandleOpenInventory;
		InGameMenu.Instance.onCloseMenu += HandleOnCloseInventory;

        HandleOnChangeCrewMembers();
	}

    public Image GetImage ( int id)
    {
        return images[id];
    }

    private void HandleOnCloseInventory()
    {
        foreach (var item in images)
        {
            item.color = Color.white;
        }
    }

    void HandleOpenInventory ()
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
        //images[id].color = Color.clear;
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
