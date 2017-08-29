using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

public class InventoryCard : Card {

	[SerializeField]
	private RectTransform backGroundTransform;

	[SerializeField]
	private GameObject itemParent;
	private ItemButton[] itemButtons;

	private int memberIndex = 0;

	private CrewMember currentMember;

	[SerializeField]
	private GameObject hungerGroup;
	[SerializeField]
	private Image hungerFeedback;

	[SerializeField]
	private Image strenghtImage;
	[SerializeField]
	private Image dexterityImage;
	[SerializeField]
	private Image charismaImage;
	[SerializeField]
	private Image constitutionImage;

	void Start () {
		PlayerLoot.Instance.LootUI.useInventory += HandleUseInventory;
	}

	public override void Init ()
	{
		base.Init ();

		itemButtons = itemParent.GetComponentsInChildren<ItemButton> ();

	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		UpdateMember (currentMember);

		switch (actionType) {
		case InventoryActionType.Eat:
			Tween.Bounce (hungerGroup.transform, 0.2f, 1.25f);
			break;
		case InventoryActionType.Equip:
			Tween.Bounce (defenseText.transform, 0.2f, 1.25f);
			Tween.Bounce (attackText.transform, 0.2f, 1.25f);
			break;
		case InventoryActionType.Throw:
			break;
		case InventoryActionType.Sell:
			break;
		case InventoryActionType.Buy:
			break;
		case InventoryActionType.PickUp:
			break;
		default:
			break;
		}
	}

	public override void UpdateMember (CrewMember member)
	{
		currentMember = member;

		member.Icon.HideBody ();

		base.UpdateMember (member);

		strenghtImage.GetComponentInChildren<Text> ().text 		= member.Strenght.ToString();
		dexterityImage.GetComponentInChildren<Text> ().text 	= member.Dexterity.ToString();
		charismaImage.GetComponentInChildren<Text> ().text 		= member.Charisma.ToString();
		constitutionImage.GetComponentInChildren<Text> ().text 	= member.Constitution.ToString();

//		strenghtImage.GetComponentInChildren<Animator> ().SetBool 		("Warning" , member.LevelsUp > 0);
//		dexterityImage.GetComponentInChildren<Animator> ().SetBool 		("Warning" , member.LevelsUp > 0);
//		charismaImage.GetComponentInChildren<Animator> ().SetBool 		("Warning" , member.LevelsUp > 0);
//		constitutionImage.GetComponentInChildren<Animator> ().SetBool 	("Warning" , member.LevelsUp > 0);

		strenghtImage.GetComponentInChildren<Button> ().interactable 		= member.StatPoints > 0 && member.Strenght < 7;
		dexterityImage.GetComponentInChildren<Button> ().interactable 		= member.StatPoints > 0 && member.Dexterity < 7;
		charismaImage.GetComponentInChildren<Button> ().interactable 		= member.StatPoints > 0 && member.Charisma < 7;
		constitutionImage.GetComponentInChildren<Button> ().interactable 	= member.StatPoints > 0 && member.Constitution  < 7;

		hungerFeedback.fillAmount = (float)member.CurrentHunger / (float)member.MaxState;

		int a = 0;
		foreach (ItemButton itemButton in itemButtons) {

			if ( member.Equipment [a] != null ) {
				itemButton.HandledItem = member.Equipment [a];
			} else {
				itemButton.Clear ();
			}

//			itemButton.Enabled = member.Equipment [a] != null;

			++a;
		}

	}

	public void RemoveItem (int i) {

		LootManager.Instance.getLoot(Crews.Side.Player).AddItem (PlayerLoot.Instance.SelectedMember.Equipment [i]);

		PlayerLoot.Instance.SelectedMember.Equipment [i] = null;
		PlayerLoot.Instance.LootUI.UpdateLootUI ();

		UpdateMember (currentMember);
	}

	public int MemberIndex {
		get {
			return memberIndex;
		}
		set {
			memberIndex = value;
		}
	}

	public void LevelUpStat ( int i ) {

		switch (i) {
		case 0:
			++currentMember.MemberID.Str;
			Tween.Bounce (strenghtImage.transform);
			break;
		case 1:
			++currentMember.MemberID.Dex;
			Tween.Bounce (dexterityImage.transform);
			break;
		case 2:
			++currentMember.MemberID.Cha;
			Tween.Bounce (charismaImage.transform);
			break;
		case 3:
			++currentMember.MemberID.Con;
			Tween.Bounce (constitutionImage.transform);
			break;
		}

		--currentMember.StatPoints;

		UpdateMember (currentMember);

	}


}
