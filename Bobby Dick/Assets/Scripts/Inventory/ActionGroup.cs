using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionGroup : MonoBehaviour {

	[SerializeField]
	private InventoryActionButton[] inventoryActionButtons;

	public enum ButtonType {
		Eat,
		Equip,

        PurchaseAndEquip,
        Unequip,
        Throw,
		Purchase,
		Sell,
		PickUp,

		None
	}

	bool visible = false;


	public void UpdateButtons (ButtonType[] buttonTypes) {

        Debug.Log("" + LootUI.Instance.SelectedItem.name);

		foreach ( var item in inventoryActionButtons ) {
			item.gameObject.SetActive (false);
		}

        switch (LootUI.Instance.SelectedItem.category)
        {
            case ItemCategory.Weapon:
            case ItemCategory.Clothes:
                if (LootUI.Instance.SelectedItem == CrewMember.GetSelectedMember.GetEquipment(LootUI.Instance.SelectedItem.EquipmentPart))
                {
                    Debug.Log("setting equipement to unqueipt");
                    buttonTypes[0] = ButtonType.Unequip;
                }
                else
                {
                    buttonTypes[0] = ButtonType.Equip;

                }
                break;
            default:
                break;
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
