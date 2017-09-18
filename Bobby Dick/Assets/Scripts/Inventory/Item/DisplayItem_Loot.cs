using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItem_Loot : DisplayItem {

	[SerializeField]
	LootUI lootUI;

	[SerializeField]
	private Image itemImage;

	public int index = 0;

	public void Select () {
		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Small);

		lootUI.UpdateActionButton (index);
	}

	public override Item HandledItem {
		get {
			return base.HandledItem;
		}
		set {
			base.HandledItem = value;

			if (value.spriteID < 0) {
				itemImage.enabled = false;
			} else {
				itemImage.enabled = true;
				itemImage.sprite = LootManager.Instance.getItemSprite (value.category, value.spriteID);
			}

			itemImage.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, Random.Range (-20, 20)));
		}
	}
}
