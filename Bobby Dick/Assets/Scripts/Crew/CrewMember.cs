using UnityEngine;
using System.Collections;

[System.Serializable]
public class CrewMember {

	private Crews.Side side;
	private MemberID memberID;

	public int maxStat = 6;

	public CrewMember (MemberID _memberID, Crews.Side _side, GameObject _iconObj )
	{
		memberID = _memberID;

		iconObj = _iconObj;

		side = _side;

		Init ();
	}

		// level
	private int xpToLevelUp = 100;

	private int daysOnBoard {
		get {
			return memberID.daysOnBoard;
		}
		set {
			memberID.daysOnBoard = value;
		}
	}

	private CrewIcon icon;
	private MemberFeedback info;
	private GameObject iconObj;

//	private int stepsToHunger = 0;
	private int stepsToHunger = 5;
	private int hungerDamage = 5;

	private int maxState = 100;

	private void Init () {

		// icon
		icon = iconObj.GetComponent<CrewIcon> ();
		icon.Member = this;

	}

	#region health
	public void GetHit (float damage) {
//		float damageTaken = ( ((float)damage) / ((float)Defense) );
//		damageTaken *= 10;
		float damageTaken = damage;

		damageTaken = Mathf.CeilToInt (damageTaken);
		damageTaken = Mathf.Clamp ( damageTaken , 1 , 200 );

		string smallText = damage + " / " + Defense;
		string bigText = damageTaken.ToString ();

		Health -= (int)damageTaken;

	}

	public void Kill () {
		Crews.getCrew(side).RemoveMember (this);
	}
	#endregion

	#region level


	public void AddXP ( int _xp ) {
		
		CurrentXp += _xp;

		if ( CurrentXp >= xpToLevelUp ) {
			LevelUp ();
		}
	}

	public delegate void OnLevelUpStat (CrewMember member);
	public OnLevelUpStat onLevelUpStat;

	public void HandleOnLevelUpStat (Stat stat)
	{
		int newValue = GetStat (stat) + 1;

		SetStat(stat, newValue);

		--StatPoints;

		if (onLevelUp != null) {
			onLevelUpStat (this);
		}

	}

	public delegate void OnLevelUp (CrewMember member);
	public OnLevelUp onLevelUp;

	public void LevelUp () {
		++Level;
		CurrentXp = xpToLevelUp - CurrentXp;

		++StatPoints;

		if (onLevelUp != null) {
			onLevelUp (this);
		}
	}
	public bool CheckLevel ( int lvl ) {

		if (lvl > Level) {
			LootManager.Instance.OnWrontLevel ();
			return false;
		}

		return true;

	}
	#endregion

	#region states
	public void AddToStates () {

		AddXP (3);

		CurrentHunger += stepsToHunger;

		if ( CurrentHunger >= maxState ) {

//			DialogueManager.Instance.SetDialogueTimed ("J'ai faim !", this);

			Health -= hungerDamage;

			if ( Health == 0 )
			{
				Narrator.Instance.ShowNarratorTimed (" Après " + daysOnBoard + " jours à bord, " + MemberName + " est mort d'une faim atroce");
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
			return memberID.health;
		}
		set {
			memberID.health = Mathf.Clamp (value , 0 , memberID.maxHealth);

			if (memberID.health <= 0)
				Kill ();


		}
	}

	public string MemberName {
		get {
			return memberID.Name;
		}
	}

	public bool Male {
		get { return memberID.Male; }
	}
	#endregion

	#region stats
	private int currentAttack = 0;

	public int CurrentAttack {
		get {
			return currentAttack;
		}
		set {
			currentAttack = value;
		}
	}

	public int Attack {
		get {

			int i = GetStat(Stat.Strenght) * 5;

			if (GetEquipment (EquipmentPart.Weapon) != null)
				return i + GetEquipment (EquipmentPart.Weapon).value;

			return i;
		}
	}

	public int Defense {
		get {

			int i = GetStat(Stat.Constitution) * 5;

			if (GetEquipment (EquipmentPart.Clothes) != null)
				return i + GetEquipment (EquipmentPart.Clothes).value;

			return i;
		}
	}

	public int GetStat (Stat stat) {
		return memberID.stats [(int)stat];
	}

	public void SetStat (Stat stat, int value) {
		memberID.stats [(int)stat] = value;
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
	public MemberID MemberID {
		get {
			return memberID;
		}
	}

	public MemberFeedback Info {
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
		switch (part) {
		case EquipmentPart.Weapon:
			memberID.equipedWeapon = item;
			break;
		case EquipmentPart.Clothes:
			memberID.equipedCloth = item;
			break;
		default:
			Debug.LogError ("whut...");
			break;
		}
	}
	public Item GetEquipment ( EquipmentPart part ) {
		switch (part) {
		case EquipmentPart.Weapon:
			return memberID.equipedWeapon;
			break;
		case EquipmentPart.Clothes:
			return memberID.equipedCloth;
			break;
		default:
			return memberID.equipedWeapon;
			break;
		}
	}
	#endregion

	#region states properties
	public int CurrentHunger {
		get {
			return memberID.currentHunger;
		}
		set {
			memberID.currentHunger = Mathf.Clamp (value, 0, maxState);
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
	public int Level {
		get {
			return memberID.Lvl;
		}
		set {
			memberID.Lvl = value;
		}
	}
	public int CurrentXp {
		get {
			return memberID.xp;
		}
		set {
			memberID.xp = value;
		}
	}

	public int XpToLevelUp {
		get {
			return xpToLevelUp;
		}
	}

	public int StatPoints {
		get {
			return memberID.statPoints;
		}
		set {
			memberID.statPoints = value;
		}
	}
	#endregion
}

public enum Stat {
	Strenght,
	Dexterity,
	Charisma,
	Constitution
}