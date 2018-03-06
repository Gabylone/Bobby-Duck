using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItem_Crew : DisplayItem {

	public delegate void OnRemoveItemFromMember(Item item);
	public static OnRemoveItemFromMember onRemoveItemFromMember;

	public GameObject group;

	public CrewMember.EquipmentPart part;

	public Image itemImage;

	void Start () {
		CrewInventory.Instance.openInventory += HandleOpenInventory;
		LootUI.useInventory+= HandleUseInventory;

		Display ();
	}

	void HandleUseInventory (InventoryActionType actionType)
	{

		switch (actionType) {
		case InventoryActionType.Equip:
		case InventoryActionType.PurchaseAndEquip:
			Display ();
			break;
		default:
			break;
		}
	}

	void HandleOpenInventory (CrewMember member)
	{
		Display ();
	}

	void Display () {

		if ( CrewMember.GetSelectedMember.GetEquipment(part)!= null) {
			HandledItem = CrewMember.GetSelectedMember.GetEquipment (part);
		} else {
			Clear ();
		}
	}

	public void RemoveItem () {

		if ( HandledItem== null ) {
			return;
		}

		LootManager.Instance.getLoot (Crews.Side.Player).AddItem (HandledItem);

		CrewMember.GetSelectedMember.RemoveEquipment (part);

		if ( onRemoveItemFromMember != null )
			onRemoveItemFromMember (HandledItem);


		Clear ();

	}
	public override void Clear ()
	{
		base.Clear ();

		Invoke ("Hide",0.3f);
	}

	void Hide () {
		group.SetActive (false);
	}

	public override Item HandledItem {
		get {
			return base.HandledItem;
		}
		set {

			base.HandledItem = value;

			if (value == null) {
				itemImage.enabled = false;
				return;
			}

			group.SetActive (true);
			Tween.ClearFade (transform);

			if (value.spriteID < 0) {
				itemImage.enabled = false;
			} else {
				itemImage.enabled = true;
				itemImage.sprite = LootManager.Instance.getItemSprite (value.category, value.spriteID);
			}

			itemImage.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, Random.Range (-30, 30)));


		}
	}
}
