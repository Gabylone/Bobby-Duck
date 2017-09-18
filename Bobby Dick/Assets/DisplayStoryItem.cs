using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayStoryItem : MonoBehaviour {

	[SerializeField]
	DisplayItem_Loot displayItem_Loot;

	[SerializeField]
	private GameObject group;

	// Use this for initialization
	void Start () {
		LootManager.onAddToInventory += HandleOnAddToInventory;

		StoryInput.onPressInput += HandleOnPressInput;

		group.SetActive (false);
	}

	void HandleOnPressInput ()
	{
		group.SetActive (false);
	}

	void HandleOnAddToInventory (Item item)
	{
		group.SetActive (true);
		displayItem_Loot.HandledItem = item;

		Tween.Bounce (displayItem_Loot.transform);
	}
}
