﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrewInventory : MonoBehaviour {

	public static CrewInventory Instance;

	public delegate void OpenInventory (CrewMember member);
	public OpenInventory openInventory;
	public delegate void CloseInventory ();
	public CloseInventory closeInventory;

	private CrewMember selectedMember;

	[Header("Groups")]
	[SerializeField]
	private GameObject crewGroup;
	public bool canOpen = true;

	public GameObject characterStatGroup;
	public GameObject menuButtonsObj;

	public void Lock () {
		canOpen = false;
	}
	public void Unlock () {
		canOpen = true;
	}

	[SerializeField]
	private Transform crewCanvas;

	private bool ateSomething = false;

	[SerializeField]
	private ActionGroup actionGroup;

	bool opened = false;

	void Awake () {
		Instance = this;
	}

	void Start () {
		HideCharacterStats ();
	}

	public void Init () {

		LootUI.useInventory += HandleUseInventory;

		StoryLauncher.Instance.playStoryEvent += HandlePlayStory;
		StoryLauncher.Instance.endStoryEvent += HandleEndStory;

	}

	#region crew group
	public void ShowCrewGroup () {
		crewGroup.SetActive (true);
		//
	}
	public void HideCrewGroup () {
		crewGroup.SetActive (false);
		//
	}
	#endregion

	#region events
	void HandleEndStory ()
	{
//		canOpen = true;
	}

	void HandlePlayStory ()
	{
//		canOpen = false;
		HideInventory ();
	}
	#endregion

	#region event handling
	void HandleUseInventory (InventoryActionType actionType)
	{
		switch (actionType) {
		case InventoryActionType.Eat:
			EatItem ();
			break;
		case InventoryActionType.Equip:
			EquipItem ();
			break;
		case InventoryActionType.Throw:
			ThrowItem ();
			break;
		case InventoryActionType.Sell:
			SellItem ();
			break;
		default:
			break;
		}

	}
	#endregion

	#region button action
	public void EatItem () {

		CrewMember.GetSelectedMember.AddHealth (LootUI.Instance.SelectedItem.value);

		int i = (int)(LootUI.Instance.SelectedItem.value * 1.5f);

		CrewMember.GetSelectedMember.CurrentHunger -= i;

		if ( OtherInventory.Instance.type == OtherInventory.Type.None )
			LootManager.Instance.PlayerLoot.RemoveItem (LootUI.Instance.SelectedItem);
		else
			LootManager.Instance.OtherLoot.RemoveItem (LootUI.Instance.SelectedItem);

	}

	void EquipItem ()
	{
		Item item = LootUI.Instance.SelectedItem;

		if ( CrewMember.GetSelectedMember.CheckLevel (item.level) == false ) {
			return;
		}

		if (CrewMember.GetSelectedMember.GetEquipment(item.EquipmentPart) != null)
			LootManager.Instance.PlayerLoot.AddItem (CrewMember.GetSelectedMember.GetEquipment(item.EquipmentPart) );

		CrewMember.GetSelectedMember.SetEquipment (item);

		if ( OtherInventory.Instance.type == OtherInventory.Type.None )
			LootManager.Instance.PlayerLoot.RemoveItem (item);
		else 
			LootManager.Instance.OtherLoot.RemoveItem (item);

	}

	public void ThrowItem () {

		LootManager.Instance.PlayerLoot.RemoveItem (LootUI.Instance.SelectedItem);
	}

	public void SellItem () {

		GoldManager.Instance.GoldAmount += LootUI.Instance.SelectedItem.price;

		LootManager.Instance.OtherLoot.AddItem (LootUI.Instance.SelectedItem);
		LootManager.Instance.PlayerLoot.RemoveItem (LootUI.Instance.SelectedItem);

	}
	#endregion

	#region properties
	public void ShowInventory ( CategoryContentType catContentType ) {
		ShowInventory (catContentType, Crews.playerCrew.captain);
	}
	public void ShowInventory ( CategoryContentType catContentType , CrewMember crewMember ) {

		if (!canOpen) {
			return;
		}

		if (catContentType == CategoryContentType.Inventory) {
			LootUI.Instance.Hide ();
			HideCharacterStats ();
			ShowMenuButtons ();
		}

		// return
		CrewMember.SetSelectedMember (crewMember);

			// set bool
		opened = true;

			// event
		openInventory (crewMember);


			// show elements
		ShowCrewGroup();

	}
	public void HideInventory () {

		if (Opened == false)
			return;

			// set bool
		opened = false;

			// event
		closeInventory();

			// hide elements
		HideCrewGroup();

		LootUI.Instance.Hide ();

	}
	#endregion

	#region character stats
	public delegate void OnShowCharacterStats();
	public static OnShowCharacterStats onShowCharacterStats;
	public void ShowCharacterStats () {
		characterStatGroup.SetActive (true);
		HideMenuButtons ();

		if (onShowCharacterStats != null)
			onShowCharacterStats ();
	}
	public delegate void OnHideCharacterStats ();
	public static OnHideCharacterStats onHideCharacterStats;
	public void HideCharacterStats () {
		characterStatGroup.SetActive (false);
		ShowMenuButtons ();

		if (onHideCharacterStats != null)
			onHideCharacterStats ();
	}
	#endregion

	#region menu buttons
	public void ShowMenuButtons () {
		menuButtonsObj.SetActive (true);
	}
	public void HideMenuButtons () {
		menuButtonsObj.SetActive (false);
	}
	#endregion

	#region inventory button
	public void Open () {
		
		LootUI.Instance.Show (CategoryContentType.Inventory,Crews.Side.Player);

		QuestMenu.Instance.Close ();
		BoatUpgradeManager.Instance.CloseUpgradeMenu ();

		HideMenuButtons ();
	}

	public void CloseLoot () {
		LootUI.Instance.Hide ();
	}

	public bool Opened {
		get {
			return opened;
		}
	}
	#endregion

}