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

	public Transform targetPos_Player;
	public Transform targetPos_Other;

	void Awake () {
		Instance = this;
	}

	void Start () {

		StoryFunctions.Instance.getFunction += HandleGetFunction;
		LootUI.useInventory += HandleUseInventory;
	}

	// 
	public void SwitchToPlayer () {

		HOTween.To (LootUI.Instance.transform , 0.5f , "position" , targetPos_Player.position, false , EaseType.EaseOutBounce,0f);

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

		CrewInventory.Instance.menuGroup.SetActive (false);

		HOTween.To (LootUI.Instance.transform , 0.5f , "position" , targetPos_Other.position, false , EaseType.EaseOutBounce,0f);

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
			Buy ();
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
		LootManager.Instance.setLoot ( Crews.Side.Enemy, loot);

		CrewInventory.Instance.ShowInventory (CategoryContentType.PlayerLoot);

		type = Type.Trade;

		SwitchToOther ();
	}
	#endregion

	#region looting
	public void StartLooting () {

		Loot loot = LootManager.Instance.GetIslandLoot ();
		LootManager.Instance.setLoot ( Crews.Side.Enemy, loot);

		CrewInventory.Instance.ShowInventory (CategoryContentType.PlayerLoot);

		type = Type.Loot;

		SwitchToOther ();

	}
	#endregion

	public void Buy () {

		if (!GoldManager.Instance.CheckGold (LootUI.Instance.SelectedItem.price)) {
			return;
		}

		if (!WeightManager.Instance.CheckWeight (LootUI.Instance.SelectedItem.weight)) {
			return;
		}

		GoldManager.Instance.GoldAmount -= LootUI.Instance.SelectedItem.price;

		LootManager.Instance.PlayerLoot.AddItem (LootUI.Instance.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (LootUI.Instance.SelectedItem);

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

		// SI ON FERME DE L'INVENTAIRE ( YA EU MERDAGE LA )
		if (type == Type.None ) {
			CrewInventory.Instance.CloseLoot ();
			return;
		}

		type = Type.None;

		CrewInventory.Instance.HideInventory ();

		Crews.getCrew (Crews.Side.Player).captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);

		if ( StoryLauncher.Instance.PlayingStory ) {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();

			if ( CombatManager.Instance.Fighting ) {
				CombatManager.Instance.EndFight ();
			}
		}

	}
	#endregion
}
