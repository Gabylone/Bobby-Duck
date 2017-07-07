using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryCard : Card {

	[SerializeField]
	private RectTransform backGroundTransform;

	[SerializeField]
	private GameObject itemParent;
	private ItemButton[] itemButtons;

	private int memberIndex = 0;

	private CrewMember currentMember;

	[SerializeField]
	private Image strenghtImage;
	[SerializeField]
	private Image dexterityImage;
	[SerializeField]
	private Image charismaImage;
	[SerializeField]
	private Image constitutionImage;

	public override void Init ()
	{
		base.Init ();

		itemButtons = itemParent.GetComponentsInChildren<ItemButton> ();

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

		strenghtImage.GetComponentInChildren<Button> ().interactable 		= member.LevelsUp > 0 && member.Strenght < 7;
		dexterityImage.GetComponentInChildren<Button> ().interactable 		= member.LevelsUp > 0 && member.Dexterity < 7;
		charismaImage.GetComponentInChildren<Button> ().interactable 		= member.LevelsUp > 0 && member.Charisma < 7;
		constitutionImage.GetComponentInChildren<Button> ().interactable 	= member.LevelsUp > 0 && member.Constitution  < 7;

		int a = 0;
		foreach (ItemButton itemButton in itemButtons) {

			if ( member.Equipment [a] != null ) {

				itemButton.Name = member.Equipment [a].name;
				itemButton.Param = member.Equipment [a].value;
				itemButton.Price = member.Equipment [a].price;
				itemButton.Level = member.Equipment [a].level;
				itemButton.Weight = member.Equipment [a].weight;

			} else {

				itemButton.Name = "";
				itemButton.Param = 0;
				itemButton.Price = 0;
				itemButton.Weight = 0;
				itemButton.Level = 0;

			}

			itemButton.Enabled = member.Equipment [a] != null;

			++a;
		}

	}

	public void RemoveItem (int i) {

		LootManager.Instance.PlayerLoot.AddItem (PlayerLoot.Instance.SelectedMember.Equipment [i]);

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
			break;
		case 1:
			++currentMember.MemberID.Dex;
			break;
		case 2:
			++currentMember.MemberID.Cha;
			break;
		case 3:
			++currentMember.MemberID.Con;
			break;
		}

		--currentMember.LevelsUp;

		UpdateMember (currentMember);

	}
}
