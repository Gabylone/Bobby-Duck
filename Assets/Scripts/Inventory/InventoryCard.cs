using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryCard : Card {

	[Header("Deploy")]
	[SerializeField]
	private float fullScale = 200f;
	private float initScale = 100f;

	[SerializeField]
	private RectTransform backGroundTransform;

	private Vector3[] stats_InitPos = new Vector3[3];
	[SerializeField]
	private Transform[] stats_DeployedAnchors;

	[SerializeField]
	private GameObject itemParent;
	private ItemButton[] itemButtons;

	[SerializeField]
	private Transform[] stats_Transforms;

	private bool deployed = false;

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

	void Start () {
		
		itemButtons = itemParent.GetComponentsInChildren<ItemButton> ();

		Init ();

		initScale = backGroundTransform.sizeDelta.y;

		int a = 0;
		foreach ( Transform statTransform in stats_Transforms ) {
			stats_InitPos[a] = statTransform.localPosition;
			++a;
		}

		Deployed = false;

	}

	void Update () {
		
	}

	public void Select () {
		PlayerLoot.Instance.SelectedMemberIndex = MemberIndex;
	}

	public override void UpdateMember (CrewMember member)
	{
		currentMember = member;

		base.UpdateMember (member);

		strenghtImage.GetComponentInChildren<Text> ().text 		= member.Strenght.ToString();
		dexterityImage.GetComponentInChildren<Text> ().text 	= member.Dexterity.ToString();
		charismaImage.GetComponentInChildren<Text> ().text 		= member.Charisma.ToString();
		constitutionImage.GetComponentInChildren<Text> ().text 	= member.Constitution.ToString();

//		strenghtImage.GetComponentInChildren<Animator> ().SetBool 		("Warning" , member.LevelsUp > 0);
//		dexterityImage.GetComponentInChildren<Animator> ().SetBool 		("Warning" , member.LevelsUp > 0);
//		charismaImage.GetComponentInChildren<Animator> ().SetBool 		("Warning" , member.LevelsUp > 0);
//		constitutionImage.GetComponentInChildren<Animator> ().SetBool 	("Warning" , member.LevelsUp > 0);

		strenghtImage.GetComponentInChildren<Button> ().interactable 		= member.LevelsUp > 0;
		dexterityImage.GetComponentInChildren<Button> ().interactable 		= member.LevelsUp > 0;
		charismaImage.GetComponentInChildren<Button> ().interactable 		= member.LevelsUp > 0;
		constitutionImage.GetComponentInChildren<Button> ().interactable 	= member.LevelsUp > 0;

	}

	public void Deploy () {

		Vector2 scale = backGroundTransform.sizeDelta;
		scale.y = fullScale;
		backGroundTransform.sizeDelta = scale;

		CrewMember crewMember = PlayerLoot.Instance.SelectedMember;

		itemParent.SetActive (true);

		int a = 0;
		foreach (ItemButton itemButton in itemButtons) {

			if ( crewMember.Equipment [a] != null ) {
				
				itemButton.Name = crewMember.Equipment [a].name;
				itemButton.Param = crewMember.Equipment [a].value;
				itemButton.Price = crewMember.Equipment [a].price;
				itemButton.Price = crewMember.Equipment [a].level;

			} else {
				
				itemButton.Name = "";
				itemButton.Param = 0;
				itemButton.Price = 0;
				itemButton.Weight = 0;
				itemButton.Level = 0;

			}

			itemButton.Enabled = crewMember.Equipment [a] != null;

			++a;
		}
	}

	public void Reset () {
		
		Vector2 scale = backGroundTransform.sizeDelta;
		scale.y = initScale;
		backGroundTransform.sizeDelta = scale;

		int a = 0;
		foreach (ItemButton itemButton in itemButtons) {


			itemButton.Enabled = false;

			++a;
		}

		itemParent.SetActive (false);
	}

	public void RemoveItem (int i) {

		LootManager.Instance.PlayerLoot.AddItem (PlayerLoot.Instance.SelectedMember.Equipment [i]);

		PlayerLoot.Instance.SelectedMember.Equipment [i] = null;
		PlayerLoot.Instance.SelectedCard.Deploy ();
		PlayerLoot.Instance.LootUI.UpdateLootUI ();

		UpdateMember (currentMember);
	}

	public bool Deployed {
		get {
			return deployed;
		}
		set {
			deployed = value;

			if (deployed == true)
				Deploy ();
			else
				Reset ();
		}
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
			++currentMember.MemberID.str;
			break;
		case 1:
			++currentMember.MemberID.dex;
			break;
		case 2:
			++currentMember.MemberID.cha;
			break;
		case 3:
			++currentMember.MemberID.con;
			break;
		}

		--currentMember.LevelsUp;

		UpdateMember (currentMember);

	}
}
