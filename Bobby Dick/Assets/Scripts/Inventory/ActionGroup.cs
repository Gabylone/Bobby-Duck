using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionGroup : MonoBehaviour {

	[SerializeField]
	private InventoryActionButton[] inventoryActionButtons;

	public enum ButtonType {
		Eat,
		Equip,
		Throw,
		Purchase,
		Sell,
		PickUp,
		PurchaseAndEquip,
        Unequip,

		None
	}

	bool visible = false;


	public void UpdateButtons (ButtonType[] buttonTypes) {

		foreach ( var item in inventoryActionButtons ) {
			item.gameObject.SetActive (false);
		}

        if ( buttonTypes[0] == ButtonType.Equip)
        {
            switch (LootUI.Instance.SelectedItem.category)
            {
                case ItemCategory.Weapon:
                    if (LootUI.Instance.SelectedItem == CrewMember.GetSelectedMember.GetEquipment(CrewMember.EquipmentPart.Weapon))
                    {
                        buttonTypes[0] = ButtonType.Unequip;
                    }
                    break;
                case ItemCategory.Clothes:
                    if (LootUI.Instance.SelectedItem == CrewMember.GetSelectedMember.GetEquipment(CrewMember.EquipmentPart.Clothes))
                    {
                        buttonTypes[0] = ButtonType.Unequip;
                    }
                    break;
                default:
                    break;
            }
            
        }
		inventoryActionButtons [(int)buttonTypes[0]].gameObject.SetActive (true);
		Tween.Bounce (inventoryActionButtons [(int)buttonTypes [0]].transform);

		if (buttonTypes.Length > 1) {
			inventoryActionButtons [(int)buttonTypes [1]].gameObject.SetActive (true);
			Tween.Bounce (inventoryActionButtons [(int)buttonTypes [1]].transform);
		}

	}

	public bool Visible {
		get {
			return visible;
		}
		set {
			visible = value;

			gameObject.SetActive (value);
		}
	}
}
