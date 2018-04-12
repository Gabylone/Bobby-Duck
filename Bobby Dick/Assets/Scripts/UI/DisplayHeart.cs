using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class DisplayHeart : MonoBehaviour {

	public Image backGround;
	public Image fillImage;

	// Use this for initialization
	void Start () {

		CrewInventory.Instance.openInventory += HandleOpenInventory;
		LootUI.useInventory += HandleUseInventory;

		UpdateUI ();
	}

	void HandleOpenInventory (CrewMember crewMember)
	{
		UpdateUI ();
	}

	void UpdateUI () {

		CrewMember member = CrewMember.GetSelectedMember;



		Tween.Bounce (transform, 0.2f, 1.05f);
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		if ( actionType == InventoryActionType.Eat ) {
			UpdateUI ();
		}
	}
}
