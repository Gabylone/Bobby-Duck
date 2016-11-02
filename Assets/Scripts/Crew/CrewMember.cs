using UnityEngine;
using System.Collections;

[System.Serializable]
public class CrewMember {

	private Crews.Side side;

	public CrewMember (
		string _name,

		int _level,

		int _health,
		int _attackDice,
		int _constitutionDice,
		int _speedDice,

		Crews.Side _side,
	
		GameObject _iconObj )
	{
			// assign stats
		memberName 			 = _name;

		health			= _health;
		maxHealth 		= _health;

		attackDice 		 = _attackDice;
		constitutionDice = _constitutionDice;
		speedDice		 = _speedDice;

		side = _side;
		Level = _level;

		iconObj = _iconObj;

			// initialization
		Init ();
	}

	private string memberName;

	private int level;
	private int xp = 0;
	private int stepToNextLevel = 10;

	private int health;
	private int maxHealth;
	private int attackDice;
	private int speedDice;
	private int constitutionDice;

	private int daysOnBoard = 0;

	private Item[] equipment = new Item[3];

	private CrewIcon icon;
	private CrewInfo info;
	private GameObject iconObj;

	private float currentCold = 0;
	private float stepsToCold = 1;

	private float currentHunger = 0;
	private float stepsToHunger = 1;

	private int maxState = 20;

	private void Init () {

		icon = iconObj.GetComponent<CrewIcon> ();
		info = iconObj.GetComponent<CrewInfo> ();

		if (side == Crews.Side.Enemy)
			icon.Overable = false;
	}

	#region health
	public void GetHit (int damage) {

		string smallText = damage.ToString () + " - " + constitutionDice.ToString () + " = ";
		string bigText = (damage-constitutionDice).ToString ();

		info.DisplayInfo (smallText, bigText , Color.red);
		Icon.Animator.SetTrigger ("GetHit");

		Health -= (damage-constitutionDice);

		CardManager.Instance.UpdateCards ();

	}

	public void Kill () {

		Crews.getCrew(side).RemoveMember (this);
	}
	#endregion

	public void AddXP ( int _xp ) {
		xp += _xp;
		if ( xp >= stepToNextLevel ) {
			Debug.Log (memberName + " nest level");
			++level;
			xp = stepToNextLevel - xp;

			maxHealth += 5;
			health = maxHealth;

			stepsToCold -= 0.1f;
		}
	}

	#region states
	public void AddToStates () {

		CurrentHunger += StepsToHunger;

		if ( CurrentHunger >= maxState ) {
			--Health;
			if ( health == 0 )
			{
				DialogueManager.Instance.ShowNarrator (" Après " + daysOnBoard + " jours à bord, " + memberName + " est mort d'une faim atroce");
				Kill ();
			}
		}

//		if ( MapManager.Instance.PosY > 0 )
//			CurrentCold += StepsToCold;

		if ( CurrentCold >= maxState ) {
			--Health;
			if ( health == 0 )
			{
				DialogueManager.Instance.ShowNarrator (" Après " + daysOnBoard + " jours à bord, " + memberName + " meurt d'un froid mordant");
				Kill ();
			}
		}

	}
	#endregion

	#region parameters
	public int Health {
		get {
			return health;
		}
		set {
			health = Mathf.Clamp (value , 0 , maxHealth);
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

	#region properties
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

	public CrewInfo Info {
		get {
			return info;
		}
	}
	#endregion

	#region states properties
	public float StepsToHunger {
		get {
			return stepsToHunger;
		}
		set {
			stepsToHunger = value;
		}
	}

	public float CurrentHunger {
		get {
			return currentHunger;
		}
		set {
			currentHunger = Mathf.Clamp (value, 0, maxState);
		}
	}

	public float StepsToCold {
		get {
			return stepsToCold;
		}
		set {
			stepsToCold = value;
		}
	}

	public float CurrentCold {
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
}
