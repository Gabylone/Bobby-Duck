using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHeart : MonoBehaviour {

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

		CrewMember member = CrewMember.selectedMember;

		fillImage.fillAmount = (float)member.Health / (float)member.MemberID.maxHealth;
//		text.text = member.Health.ToString ();

		BounceHeart ();
	}

	void BounceHeart() {
		Tween.Bounce (fillImage.transform);
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		if ( actionType == InventoryActionType.Eat ) {
			UpdateUI ();
		}
	}
}
