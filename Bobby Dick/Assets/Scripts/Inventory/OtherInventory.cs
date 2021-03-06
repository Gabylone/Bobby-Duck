﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

public class OtherInventory : MonoBehaviour {

	public static OtherInventory Instance;

	public enum Type {
		None,

		Loot,
		Trade,
	}

	public Type type = Type.None;

    public float lootTransition_Duration = 0.5f;
    public float lootTransition_Decal = 100f;

	void Awake () {
		Instance = this;
	}

	void Start () {

		StoryFunctions.Instance.getFunction += HandleGetFunction;
		LootUI.useInventory += HandleUseInventory;
	}

	// 
	public void SwitchToPlayer ()
    {
        StartCoroutine(SwitchSideCoroutine());
    }

    //
    public void SwitchToOther()
    {
        StartCoroutine(SwitchSideCoroutine());
    }

    public void LerpIn()
    {
        LootUI.Instance.transform.position = Vector3.right * lootTransition_Decal;

        LootUI.Instance.HideAllSwitchButtons();
        LootUI.Instance.closeButton.SetActive(false);

        LootUI.Instance.transform.DOMove(Vector3.zero, lootTransition_Duration);

        CancelInvoke("ShowButtons");
        Invoke("ShowButtons",lootTransition_Duration);
    }

    public void LerpOut()
    {
        CancelInvoke("ShowButtons");

        LootUI.Instance.transform.DOMove(Vector3.right * lootTransition_Decal, lootTransition_Duration);

        LootUI.Instance.HideAllSwitchButtons();
        LootUI.Instance.closeButton.SetActive(false);

    }

    void SwitchInventorySide()
    {
        LootUI.Instance.currentSide = LootUI.Instance.currentSide == Crews.Side.Player ? Crews.Side.Enemy : Crews.Side.Player;
        ShowLoot();
    }

    void ShowButtons()
    {
        LootUI.Instance.InitButtons();
        LootUI.Instance.closeButton.SetActive(true);
    }

    IEnumerator SwitchSideCoroutine()
    {
        LerpOut();

        ///
        yield return new WaitForSeconds(lootTransition_Duration);
        ///

        LerpIn();

        SwitchInventorySide();
    }


	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.Loot:
			StartLooting ();
			break;
		case FunctionType.Trade:
			StartTrade ();
			break;

		}
	}

    void ShowLoot()
    {
        if (LootUI.Instance.currentSide == Crews.Side.Player)
        {
            switch (type)
            {
                case Type.None:
                    break;

                case Type.Loot:
                    LootUI.Instance.Show(CategoryContentType.PlayerLoot, Crews.Side.Player);
                    break;
                case Type.Trade:
                    LootUI.Instance.Show(CategoryContentType.PlayerTrade, Crews.Side.Player);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case Type.None:
                    break;

                case Type.Loot:
                    LootUI.Instance.Show(CategoryContentType.OtherLoot, Crews.Side.Enemy);
                    break;
                case Type.Trade:
                    LootUI.Instance.Show(CategoryContentType.OtherTrade, Crews.Side.Enemy);
                    break;
                default:
                    break;
            }
        }
    }

	void HandleUseInventory (InventoryActionType actionType)
	{
		switch (actionType) {
		case InventoryActionType.Buy:
			PurchaseItem ();
			break;
		case InventoryActionType.PurchaseAndEquip:
			PuchaseAndEquip ();
			break;
		case InventoryActionType.PickUp:
			PickUp (LootUI.Instance.SelectedItem);
			break;
		default:
			break;
		}
	}

	#region trade
	public void StartTrade () {

			// get loot
		Loot loot = LootManager.Instance.GetIslandLoot (2);

		if ( loot.IsEmpty () ) {
			DialogueManager.Instance.SetDialogue ("Ah désolé, vous m'avez déjà tout pris", Crews.enemyCrew.captain);
			StoryInput.Instance.WaitForInput ();
			return;
		}

        LootUI.Instance.currentSide = Crews.Side.Enemy;
		LootManager.Instance.SetLoot ( Crews.Side.Enemy, loot);

		InGameMenu.Instance.Open ();

		type = Type.Trade;

        ShowLoot();
	}
	#endregion

	#region looting
	public void StartLooting () {

		Loot loot = LootManager.Instance.GetIslandLoot (1);

		if ( loot.IsEmpty () ) {
			DialogueManager.Instance.SetDialogue ("Il n'y a plus rien", Crews.playerCrew.captain);
			StoryInput.Instance.WaitForInput ();
			return;
		}

        LootUI.Instance.currentSide = Crews.Side.Enemy;
        LootManager.Instance.SetLoot ( Crews.Side.Enemy, loot);

		InGameMenu.Instance.Open ();

		type = Type.Loot;

        ShowLoot();

    }
	#endregion

	public void PurchaseItem () {

		if (!GoldManager.Instance.CheckGold (LootUI.Instance.SelectedItem.price)) {
			return;
		}

		if (!WeightManager.Instance.CheckWeight (LootUI.Instance.SelectedItem.weight)) {
			return;
		}

		GoldManager.Instance.RemoveGold (LootUI.Instance.SelectedItem.price);

		LootManager.Instance.PlayerLoot.AddItem (LootUI.Instance.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (LootUI.Instance.SelectedItem);

	}

	public void PuchaseAndEquip () {

		Item item = LootUI.Instance.SelectedItem;

		if (!GoldManager.Instance.CheckGold (item.price)) {
			print (" l'objet est torp cher ?, retour");
			return;
		}

		if (!WeightManager.Instance.CheckWeight (item.weight)) {
			print (" l'objet est torp lourd, retour");
			return;
		}

		if ( !CrewMember.GetSelectedMember.CheckLevel (item.level) ) {
			return;
		}

		GoldManager.Instance.RemoveGold (item.price);

		LootManager.Instance.OtherLoot.RemoveItem (item);

		if (CrewMember.GetSelectedMember.GetEquipment (item.EquipmentPart) != null)
			LootManager.Instance.PlayerLoot.AddItem (CrewMember.GetSelectedMember.GetEquipment (item.EquipmentPart));
		
		CrewMember.GetSelectedMember.SetEquipment (item);

	}

	public void PickUp ( Item item ) {
		
		if (!WeightManager.Instance.CheckWeight (item.weight))
			return;

		LootManager.Instance.PlayerLoot.AddItem (item);
		LootManager.Instance.OtherLoot.RemoveItem (item);
	}
}
