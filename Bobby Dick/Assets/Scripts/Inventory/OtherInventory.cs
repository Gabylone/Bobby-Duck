using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class OtherInventory : MonoBehaviour {

	public static OtherInventory Instance;

	public enum Type {
		None,

		Loot,
		Trade,
	}

	public Type type = Type.None;

	void Awake () {
		Instance = this;
	}

	void Start () {

		StoryFunctions.Instance.getFunction += HandleGetFunction;
		LootUI.useInventory += HandleUseInventory;
	}

	// 
	public void SwitchToPlayer () {

		switch (type) {
		case Type.None:
			break;
		case Type.Loot:
			LootUI.Instance.Show (CategoryContentType.PlayerLoot, Crews.Side.Player);
			break;
		case Type.Trade:
			LootUI.Instance.Show (CategoryContentType.PlayerTrade, Crews.Side.Player);
			break;
		default:
			break;
		}

	}

	public void SwitchToOther () {

		CrewInventory.Instance.characterStatGroup.SetActive (false);

//		HOTween.To (LootUI.Instance.transform , 0.2f , "position" , targetPos_Other.position, false , EaseType.Linear,0f);
//		HOTween.To (LootUI.Instance.transform , 0.5f , "position" , targetPos_Other.position, false , EaseType.EaseOutBounce,0f);

		switch (type) {
		case Type.None:
			break;
		case Type.Loot:
			LootUI.Instance.Show (CategoryContentType.OtherLoot, Crews.Side.Enemy);
			break;
		case Type.Trade:
			LootUI.Instance.Show (CategoryContentType.OtherTrade, Crews.Side.Enemy);
			break;
		default:
			break;
		}
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
			PickUp ();
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

		LootManager.Instance.setLoot ( Crews.Side.Enemy, loot);

		CrewInventory.Instance.ShowInventory (CategoryContentType.PlayerLoot);

		type = Type.Trade;

		SwitchToOther ();
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

		LootManager.Instance.setLoot ( Crews.Side.Enemy, loot);

		CrewInventory.Instance.ShowInventory (CategoryContentType.PlayerLoot);

		type = Type.Loot;

		SwitchToOther ();

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

	public void PickUp () {
		
		if (!WeightManager.Instance.CheckWeight (LootUI.Instance.SelectedItem.weight))
			return;

		LootManager.Instance.PlayerLoot.AddItem (LootUI.Instance.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (LootUI.Instance.SelectedItem);
	}

	#region open / close
	public void Close () {

		LootUI.Instance.Hide ();

		if (type == Type.Loot || type == Type.Trade) {
			
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();
			CrewInventory.Instance.HideInventory ();
			Crews.getCrew (Crews.Side.Player).captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);

		} else {
			CrewInventory.Instance.ShowMenuButtons ();
		}

		type = Type.None;

//		else {
//			CrewInventory.Instance.ShowMenuButtons ();
//		}

	}
	#endregion
}
