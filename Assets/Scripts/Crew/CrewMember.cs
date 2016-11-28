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
	private int xp = 0;
	private int stepToNextLevel = 10;

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

		// health
		health = MaxHealth;

		// icon
		icon = iconObj.GetComponent<CrewIcon> ();
		info = iconObj.GetComponent<CrewInfo> ();

		// side
		if (side == Crews.Side.Enemy)
			icon.Overable = false;

		// equipment
		if (memberID.weaponID > -1)
			SetEquipment (EquipmentPart.Weapon, 	ItemLoader.Instance.getItem (ItemCategory.Weapon, memberID.weaponID));
		if (memberID.clothesID > -1)
			SetEquipment (EquipmentPart.Clothes, 	ItemLoader.Instance.getItem (ItemCategory.Clothes, memberID.clothesID));
		if (memberID.shoesID > -1)
			SetEquipment (EquipmentPart.Shoes, 		ItemLoader.Instance.getItem (ItemCategory.Shoes, memberID.shoesID));

	}

	#region health
	public void GetHit (int damage) {

		float damageTaken = damage - Random.Range (ConstitutionDice, ConstitutionDice - 1.5f);
		damageTaken = Mathf.Clamp ( damage , Random.Range (1 , 3) , damageTaken );

		damageTaken = Mathf.CeilToInt (damageTaken);

		string smallText = damage + " / " + ConstitutionDice;
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

	public void AddXP ( int _xp ) {
		
		xp += _xp;

		if ( xp >= stepToNextLevel ) {
			++Level;
			xp = stepToNextLevel - xp;

			MaxHealth += 20;
			health = MaxHealth;

			stepsToCold -= 0.1f;
		}
	}

	#region states
	public void AddToStates () {

		CurrentHunger += StepsToHunger;

		if ( CurrentHunger >= maxState ) {

			DialogueManager.Instance.SetDialogue ("J'ai faim !", this);

			--Health;
			if ( health == 0 )
			{
				DialogueManager.Instance.ShowNarrator (" Après " + daysOnBoard + " jours à bord, " + MemberName + " est mort d'une faim atroce");
				Kill ();
				return;
			}
		}

//		if ( MapManager.Instance.PosY > 0 )
//			CurrentCold += StepsToCold;

		if ( CurrentCold >= maxState ) {
			--Health;
			if ( health == 0 )
			{
				DialogueManager.Instance.ShowNarrator (" Après " + daysOnBoard + " jours à bord, " + MemberName + " meurt d'un froid mordant");
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
			return CrewCreator.Instance.Names[memberID.nameID];
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

	public int AttackDice {
		get {
			return memberID.attack;
		}
		set {
			memberID.attack = value;
		}
	}

	public int SpeedDice {
		get {
			return memberID.speed;
		}
		set {
			memberID.speed = value;
		}
	}

	public int ConstitutionDice {
		get {
			return memberID.constitution;
		}
		set {
			memberID.constitution = value;
		}
	}

	public int[] getDiceValues {
		get {
			return new int[] {
				Health,
				AttackDice,
				SpeedDice,
				ConstitutionDice
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
			return memberID.maxHP;
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
		Shoes
	}
	public void SetRandomEquipment () {

		ItemCategory[] equipmentCategories = new ItemCategory[3] {
			ItemCategory.Weapon,
			ItemCategory.Clothes,
			ItemCategory.Shoes
		};

		for (int i = 0; i < 3; ++i) {
			
			if (Random.value < 0.5f) {
				Item equipmentItem = ItemLoader.Instance.getRandomItem (equipmentCategories [i]);
				SetEquipment ((EquipmentPart)i, equipmentItem);
			}
		}

	}
	public void SetEquipment ( EquipmentPart part , Item item ) {
		
		equipment [(int)part] = item;

		if (item.category == ItemCategory.Weapon)
			AttackDice = item.value;

		if (item.category == ItemCategory.Clothes)
			ConstitutionDice = item.value;

		if (item.category == ItemCategory.Shoes)
			SpeedDice = item.value;
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