using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

		float health_Width = backGround.rectTransform.sizeDelta.x * (float)member.Health / (float)member.MemberID.maxHealth;
		fillImage.rectTransform.sizeDelta = new Vector2 ( health_Width , fillImage.rectTransform.sizeDelta.y);

		BounceHeart ();
	}

	void BounceHeart() {
//		Tween.Bounce (backGround.transform);
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		if ( actionType == InventoryActionType.Eat ) {
			UpdateUI ();
		}
	}
}
