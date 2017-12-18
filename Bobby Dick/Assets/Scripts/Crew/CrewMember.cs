using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CrewMember {

	// SELECTED MEMBER IN INVENTORY
	public static CrewMember selectedMember;
	public static void setSelectedMember (CrewMember crewMember) {
		
		if (selectedMember != null) {
			if (selectedMember.Icon == null) {
				Debug.LogError ("le probleme de la mort après combat sur : " + selectedMember.MemberName);
				return;
			} else {
				selectedMember.Icon.Down ();
			}
		}

		selectedMember = crewMember;

		selectedMember.Icon.Up ();
	}

	// STATS
	public int maxLevel = 10;

	// EXPERIENCE
	public int xpToLevelUp = 100;

	// COMPONENTS
	public Crews.Side side;
	private Member memberID;

	// HUNGER
	private int stepsToHunger = 5;
	private int hungerDamage = 5;
	public int maxHunger = 100;

	// JOB
	public Job job {
		get {
			return memberID.job;
		}
	}

	public List<Skill> specialSkills = new List<Skill>();
//
//	public List<Skill> defaultSkills {
//		get {
//			return memberID.defaultSkills;
//		}
//	}

	/// <summary>
	/// energy
	/// </summary>
	public int energy = 0;
	public int energyPerTurn = 6;
	public int maxEnergy = 10;

	public void AddEnergy (int _energy)
	{
		energy += _energy;

		energy = Mathf.Clamp (energy, 0, maxEnergy);
	}

	// ICON
	public MemberIcon memberIcon;

	// CONSTRUCTOR
	public CrewMember (Member _memberID, Crews.Side _side , MemberIcon memberIcon )
	{
		memberID = _memberID;

		side = _side;

		this.memberIcon = memberIcon;

		this.memberIcon.SetMember (this);

		// skill place holder
		foreach (var item in memberID.specialSkillsIndexes) {
			Debug.Log (SkillManager.skills.Length);
			specialSkills.Add (SkillManager.skills [item]);
		}

	}

	#region level
	public void AddXP ( int _xp ) {

		if ( Level == maxLevel ) {
			return;
		}

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

		CurrentXp = CurrentXp - xpToLevelUp;

		++StatPoints;

		if ( Level == maxLevel ) {
			CurrentXp = 0;
		}

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

	#region health
	public float getDamage ( float incomingAttack ) {
//		float maxHits = 16;
		float maxHits = 10;
		float minHits = 2;

		float dif = (Defense - incomingAttack) / 10f;
//				Debug.Log("dif : " + dif);

		float lerp = (dif + (maxHits / 2f)) / maxHits;
//				Debug.Log ("lerp : " + lerp);

		float hits = minHits + ((maxHits - minHits) * lerp);
//				Debug.Log ("hits : " + hits);

		float damageTaken = 100f / hits;
//				Debug.Log ("damage : " + damageTaken);

		int roundedDamage = Mathf.CeilToInt(damageTaken);

		return roundedDamage;
//				Debug.Log ("rounded damage : " + roundedDamage);
	}
	public void AddHealth (float incomingDamage) {
		Health += Mathf.RoundToInt(incomingDamage);
	}
	public void RemoveHealth (float incomingDamage) {
		Health -= Mathf.RoundToInt(incomingDamage);
	}

	public void Kill () {
		Crews.getCrew(side).RemoveMember (this);

	}
	#endregion

	#region states
	public void UpdateHunger () {

		AddXP (4);

		CurrentHunger += stepsToHunger;

		if ( CurrentHunger >= maxHunger ) {

			if ( Health - hungerDamage <= 0 )
			{
				Narrator.Instance.ShowNarratorTimed (" Après " + daysOnBoard + " jours à bord, " + MemberName + " est mort d'une faim atroce");
			}

			RemoveHealth(hungerDamage);

			if (Health <= 0) {
				Kill ();
			}

		}

		++daysOnBoard;

	}
	private int daysOnBoard {
		get {
			return memberID.daysOnBoard;
		}
		set {
			memberID.daysOnBoard = value;
		}
	}
	#endregion

	#region parameters
	public int Health {
		get {
			return memberID.health;
		}
		set {
			memberID.health = Mathf.Clamp (value , 0 , memberID.maxHealth);
//
//			if (memberID.health <= 0)
//				Kill ();
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
	public MemberIcon Icon {
		get {
			return memberIcon;
		}
	}
	public int GetIndex {
		get {
			return Crews.getCrew (side).CrewMembers.FindIndex (x => x == this);
		}
	}
	#endregion

	#region properties
	public Member MemberID {
		get {
			return memberID;
		}
	}
	#endregion

	#region equipment
	public enum EquipmentPart {
		Weapon,
		Clothes,
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
			memberID.currentHunger = Mathf.Clamp (value, 0, maxHunger);
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

public enum Job {
	Brute,
	Surgeon,
	Cook,
	Flibuster,
	Gambler,

	None,
}