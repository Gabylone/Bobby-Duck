using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayItem_Crew : DisplayItem {

	public delegate void OnRemoveItemFromMember(Item item);
	public static OnRemoveItemFromMember onRemoveItemFromMember;

	public CrewMember.EquipmentPart part;

	void Start () {
		PlayerLoot.Instance.openInventory += HandleOpenInventory;
		PlayerLoot.Instance.LootUI.useInventory+= HandleUseInventory;

		Display ();
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		if (actionType == InventoryActionType.Equip) {
			Display ();
		}
	}

	void HandleOpenInventory (CrewMember member)
	{
		Display ();
	}

	void Display () {

		CrewMember member = PlayerLoot.Instance.SelectedMember;

		if ( member.GetEquipment(part)!= null) {
			HandledItem = member.GetEquipment (part);
		} else {
			Clear ();
		}
	}

	public void RemoveItem () {

		if ( onRemoveItemFromMember != null )
			onRemoveItemFromMember (HandledItem);

		PlayerLoot.Instance.SelectedMember.SetEquipment (part, null);

		Clear ();

	}
}
