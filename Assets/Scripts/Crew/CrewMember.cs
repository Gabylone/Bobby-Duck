using UnityEngine;
using System.Collections;

[System.Serializable]
public class CrewMember {

	private Crews.Side side;
	private MemberID memberID;

	public CrewMember (MemberID _memberID, Crews.Side _side,GameObject _iconObj )
	{
		memberID = _memberID;

		iconObj = _iconObj;

		side = _side;

		Init ();
	}

	private int health = 0;

		// level
	int levelsUp = 0;

	private int xp = 0;
	private int stepToNextLevel = 100;

	private int daysOnBoard = 0;

	private Item[] equipment = new Item[3];

	private CrewIcon icon;
	private CrewInfo info;
	private GameObject iconObj;

	private int currentCold = 0;
	private int stepsToCold = 4;

	private int currentHunger = 0;
	private int stepsToHunger = 1;

	private int maxState = 100;


	private void Init () {

		// health
		health = MaxHealth;

		// icon
		icon = iconObj.GetComponent<CrewIcon> ();
		info = iconObj.GetComponent<CrewInfo> ();

		// side
		if (side == Crews.Side.Enemy)
			icon.Overable = false;

		// equipment
		SetEquipment (EquipmentPart.Weapon, 	ItemLoader.Instance.getItem (ItemCategory.Weapon, memberID.weaponID));
		SetEquipment (EquipmentPart.Clothes, 	ItemLoader.Instance.getItem (ItemCategory.Clothes, memberID.clothesID));

	}

	#region health
	public void GetHit (int damage) {
		float damageTaken = ( ((float)damage) / ((float)Defense) );
		damageTaken *= 10;

		damageTaken = Mathf.CeilToInt (damageTaken);
		damageTaken = Mathf.Clamp ( damageTaken , 1 , 100 );

		string smallText = damage + " / " + Defense;
		string bigText = damageTaken.ToString ();

		info.DisplayInfo (smallText, bigText , Color.red);
		Icon.Animator.SetTrigger ("GetHit");

		Health -= (int)damageTaken;

		CardManager.Instance.UpdateCards ();

	}

	public void Kill () {

		Crews.getCrew(side).RemoveMember (this);

	}
	#endregion

	#region level
	public void AddXP ( int _xp ) {
		
		xp += _xp;

		if ( xp >= stepToNextLevel ) {
			++Level;
			xp = stepToNextLevel - xp;

			health = MaxHealth;

			++levelsUp;
		}
	}
	public bool CheckLevel ( int lvl ) {

		if (lvl > Level) {

			DialogueManager.Instance.SetDialogue ("Je sais pas porter ça moi...", this);

			return false;
		}

		return true;

	}
	#endregion

	#region states
	public void AddToStates () {

		AddXP (1);

		CurrentHunger += StepsToHunger;

		if ( CurrentHunger >= maxState ) {

			DialogueManager.Instance.SetDialogue ("J'ai faim !", this);

			Health -= StepsToHunger;
			if ( health == 0 )
			{
				DialogueManager.Instance.ShowNarrator (" Après " + daysOnBoard + " jours à bord, " + MemberName + " est mort d'une faim atroce");
				Kill ();
				return;
			}
		}

		++daysOnBoard;

	}
	#endregion

	#region parameters
	public int Health {
		get {
			return health;
		}
		set {
			health = Mathf.Clamp (value , 0 , MaxHealth);
		}
	}

	public string MemberName {
		get {
			return Male ? CrewCreator.Instance.MaleNames [memberID.nameID] : CrewCreator.Instance.FemaleNames [memberID.nameID];
		}
	}


	public int Level {
		get {
			return memberID.lvl;
		}
		set {
			memberID.lvl = value;
		}
	}

	public bool Male {
		get { return memberID.male; }
	}
	#endregion

	#region stats
	public int Attack {
		get {

			if (GetEquipment (EquipmentPart.Weapon) != null)
				return Strenght + GetEquipment (EquipmentPart.Weapon).value;

			return Strenght;
		}
	}


	public int Defense {
		get {

			if (GetEquipment (EquipmentPart.Clothes) != null)
				return Constitution + GetEquipment (EquipmentPart.Clothes).value;

			return Constitution;
		}
	}

	public int Strenght {
		get {
			return memberID.str;
		}
		set {
			memberID.str = value;
		}
	}

	public int Dexterity {
		get {
			return memberID.dex;
		}
		set {
			memberID.dex = value;
		}
	}

	public int Charisma {
		get {
			return memberID.cha;
		}
		set {
			memberID.cha = value;
		}
	}

	public int Constitution {
		get {
			return memberID.con;
		}
		set {
			memberID.con = value;
		}
	}
	#endregion

	#region icon
	public GameObject IconObj {
		get {
			return iconObj;
		}
		set {
			iconObj = value;
		}
	}

	public Vector3 IconPos {
		get {
			return iconObj.transform.position;
		}
	}

	public CrewIcon Icon {
		get {
			return icon;
		}
		set {
			icon = value;
		}
	}
	public int GetIndex {
		get {
			return Crews.getCrew (side).CrewMembers.FindIndex (x => x == this);
		}
	}

	public Crews.Side Side {
		get {
			return side;
		}
		set {
			side = value;
		}
	}
	#endregion

	#region properties
	public int MaxHealth {
		get {
			return memberID.maxHP + (memberID.con*10);
		}
		set {
			memberID.maxHP = value;
		}
	}

	public MemberID MemberID {
		get {
			return memberID;
		}
	}

	public CrewInfo Info {
		get {
			return info;
		}
	}
	#endregion

	#region equipment
	public enum EquipmentPart {
		Weapon,
		Clothes,
	}
	public void SetRandomEquipment () {

		const int l = 2;

		ItemCategory[] equipmentCategories = new ItemCategory[l] {
			ItemCategory.Weapon,
			ItemCategory.Clothes
		};

		for (int i = 0; i < l; ++i) {

			Item equipmentItem = ItemLoader.Instance.getRandomItem (equipmentCategories [i]);
			SetEquipment ((EquipmentPart)i, equipmentItem);
		}

	}
	public void SetEquipment ( EquipmentPart part , Item item ) {
		
		equipment [(int)part] = item;

		CardManager.Instance.UpdateCards ();
		
	}
	public Item GetEquipment ( EquipmentPart part ) {
		return equipment [(int)part];
	}

	public Item[] Equipment {
		get {
			return equipment;
		}
		set {
			equipment = value;
		}
	}
	#endregion

	#region states properties
	public int StepsToHunger {
		get {
			return stepsToHunger;
		}
		set {
			stepsToHunger = value;
		}
	}

	public int CurrentHunger {
		get {
			return currentHunger;
		}
		set {
			currentHunger = Mathf.Clamp (value, 0, maxState);
		}
	}

	public int StepsToCold {
		get {
			return stepsToCold;
		}
		set {
			stepsToCold = value;
		}
	}

	public int CurrentCold {
		get {
			return currentCold;
		}
		set {
			currentCold = Mathf.Clamp (value, 0, maxState);
		}
	}

	public int MaxState {
		get {
			return maxState;
		}
		set {
			maxState = value;
		}
	}
	#endregion

	#region level
	public int Xp {
		get {
			return xp;
		}
	}

	public int StepToNextLevel {
		get {
			return stepToNextLevel;
		}
	}

	public int LevelsUp {
		get {
			return levelsUp;
		}
		set {
			levelsUp = value;
		}
	}
	#endregion
}