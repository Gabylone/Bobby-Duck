using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHeart : MonoBehaviour {

	Image fillImage;
	Text text;

	// Use this for initialization
	void Start () {

		fillImage = GetComponent<Image> ();
		text = GetComponentInChildren<Text> ();

		CrewInventory.Instance.openInventory += HandleOpenInventory;
		LootUI.useInventory += HandleUseInventory;

		HandleOpenInventory (CrewMember.selectedMember);

	}

	void HandleOpenInventory (CrewMember crewMember)
	{
		fillImage.fillAmount = (float)crewMember.Health / (float)crewMember.MemberID.maxHealth;
		text.text = crewMember.Health.ToString ();
	}

	void BounceHeart() {
		Tween.Bounce (fillImage.transform);
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		if ( actionType == InventoryActionType.Eat ) {
			BounceHeart ();
		}
	}
}
