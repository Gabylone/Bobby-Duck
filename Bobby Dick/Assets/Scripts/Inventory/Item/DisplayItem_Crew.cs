using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayItem_Crew : DisplayItem {

	public delegate void OnRemoveItemFromMember(Item item);
	public static OnRemoveItemFromMember onRemoveItemFromMember;

	public CrewMember.EquipmentPart part;

	void Start () {
		CrewInventory.Instance.openInventory += HandleOpenInventory;
		LootUI.useInventory+= HandleUseInventory;

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

		if ( CrewMember.selectedMember.GetEquipment(part)!= null) {
			HandledItem = CrewMember.selectedMember.GetEquipment (part);
		} else {
			Clear ();
		}
	}

	public void RemoveItem () {

		LootManager.Instance.getLoot(Crews.Side.Player).AddItem (HandledItem);

		CrewMember.selectedMember.SetEquipment (part, null);

		Clear ();

		if ( onRemoveItemFromMember != null )
			onRemoveItemFromMember (HandledItem);

	}
}
