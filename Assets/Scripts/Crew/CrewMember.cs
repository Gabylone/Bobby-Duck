﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class CrewMember {

	private Crews.Side side;

	public CrewMember (
		string _name,

		int _level,

		int _health,
		int _attackDice,
		int _speedDice,
		int _constitutionDice,

		Crews.Side _side,
	
		GameObject _iconObj )
	{
		memberName 			 = _name;

		health			= _health;
		maxHealth 		= _health;

		attackDice 		 = _attackDice;
		speedDice		 = _speedDice;
		constitutionDice = _constitutionDice;

		side = _side;
		Level = _level;

		iconObj = _iconObj;
		icon = iconObj.GetComponent<CrewIcon> ();

		if (side == Crews.Side.Enemy)
			icon.Overable = false;
	}

	private string memberName;
	private int level;

	private int health;
	private int maxHealth;
	private int attackDice;
	private int speedDice;
	private int constitutionDice;

	private Item[] equipment = new Item[3];

	private CrewIcon icon;
	private GameObject iconObj;

	public void GetHit (int damage) {

		iconObj.GetComponent<CrewInfo> ().DisplayInfo (damage.ToString () + " - " + ConstitutionDice.ToString () + " = " + Health.ToString ());

		Health -= (1);

		CardManager.Instance.UpdateCards ();

	}

	public void Kill () {

		Crews.getCrew(side).RemoveMember (this);
	}

	#region parameters
	public int Health {
		get {
			return health;
		}
		set {
			health = Mathf.Clamp (health , 0 , maxHealth);
		}
	}

	public string MemberName {
		get {
			return memberName;
		}
		set {
			memberName = value;
		}
	}


	public int Level {
		get {
			return level;
		}
		set {
			level = value;
		}
	}

	public int AttackDice {
		get {
			return attackDice;
		}
		set {
			attackDice = value;
		}
	}

	public int SpeedDice {
		get {
			return speedDice;
		}
		set {
			speedDice = value;
		}
	}

	public int ConstitutionDice {
		get {
			return constitutionDice;
		}
		set {
			constitutionDice = value;
		}
	}

	public int[] getDiceValues {
		get {
			return new int[] {
				health,
				attackDice,
				speedDice,
				constitutionDice
			};
		}
	}

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

	public int MaxHealth {
		get {
			return maxHealth;
		}
		set {
			maxHealth = value;
		}
	}

	public Item[] Equipment {
		get {
			return equipment;
		}
		set {
			equipment = value;
		}
	}
}
