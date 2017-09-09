using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayItem_Loot : DisplayItem {

	[SerializeField]
	LootUI lootUI;

	public int index = 0;

	public void Select () {
		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Small);

		lootUI.UpdateActionButton (index);
	}
}
