using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayStoryItem : MonoBehaviour {

	[SerializeField]
	DisplayItem_Selected displayItem;

	[SerializeField]
	private GameObject group;

	// Use this for initialization
	void Start () {
		LootManager.onAddToInventory += HandleOnAddToInventory;

		StoryInput.onPressInput += HandleOnPressInput;

        group.SetActive(false); 
	}

	void HandleOnPressInput ()
	{
		group.SetActive (false);
	}

	void HandleOnAddToInventory (Item item)
	{
		group.SetActive (true);

		displayItem.HandledItem = item;

		Tween.Bounce (displayItem.transform);
	}
}
