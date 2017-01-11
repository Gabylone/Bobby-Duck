using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CrewCreator : MonoBehaviour {

	public static CrewCreator Instance;

	private Crews.Side targetSide;

	public enum Parts {
		Face,
		Beard,
		Hair,
		Body,
		Clothes,
		LeftArm,
		Sword,
		RightArm,
		LeftFoot,
		RightFoot,
	}

	#region declaration
	[Header("General")]
	[SerializeField]
	private Transform crewParent;
	[SerializeField]
	private GameObject memberPrefab;


	private string[] maleNames = new string[51] {
		"Jean","Eric", "Nathan", "Jacques", "Benoit", "Jeremy", "Jerome", "Bertrand", "Vladimir", "Dimitri", "Jean-Jacques", "Gérard", "Nestor", "Etienne", "Leon", "Henry", "David", "Esteban", "Louis", "Carles", "Victor", "Michel", "Gabriel", "Pierre", "André", "Fred", "Cassius", "César", "Paul", "Martin", "Claude", "Levis", "Alex", "Olivier", "Mustafa", "Nicolas", "Chris", "Oleg", "Emile", "Richard", "Romulus", "Rufus", "Stan", "Charles", "Quincy", "Antoine", "Virgile", "Boromir", "Archibald", "Eddy", "Kenneth"
	};

	private string[] femaleNames = new string[51] {
		"Jeanne","Erica", "Nathalie", "Jacquelines", "Barbara", "Ella", "Flo", "Laura", "Natasha", "Irene", "Yvonne", "Gérarde", "Nelly", "Elisa", "Adele", "Henriette", "Alice", "Esteban", "Louise", "Carla", "Victoria", "Michelle", "Gabrielle", "Sarah", "Andréa", "Marion", "Valentine", "Cléopatre", "Pauline", "Martine", "Claudette", "Nina", "Alexandra", "Clementine", "Julia", "Olivia", "Christine", "Rose", "Emilia", "Agathe", "Lily", "Claire", "Yasmine", "Charlotte", "Scarlett", "Marina", "Virginie", "Anaïs", "Tatiana", "Cécile", "Marianne"
	};

	[SerializeField]
	private int startHealth = 10;

	[SerializeField]
	private int partAmount = 3;

	[Header("Sprites")]
	[SerializeField]
	private Sprite[] faceSprites;

	[Header("Hair")]
	[SerializeField]
	private Sprite[] hairSprites;
	public int[] femaleHairID 	= new int[0];
	public int[] maleHairID 	= new int[0];

	[SerializeField]
	private Sprite[] bodySprites;

	[Header("Beard")]
	[SerializeField]
	private Sprite[] beardSprites;

	[Header("Clothe")]
	[SerializeField]
	private Sprite[] clothesSprites;
	public int[] femaleClothesID 	= new int[0];
	public int[] maleClothesID 	= new int[0];

	[Header ("Colors")]
	[SerializeField] private Color lightBrown;
	[SerializeField] private Color darkSkin;
	[SerializeField] private Color darkHair;
	[SerializeField] private Color beige;
	[SerializeField] private Color[] hairColors = new Color [7] {
		Color.red,
		Color.white,
		Color.black,
		Color.yellow,
		Color.gray,
		Color.gray,
		Color.gray,
	};
	#endregion

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
	}

	void Update () {

	}

	public CrewMember NewMember (MemberID memberID) {

		CrewMember crewMember = new CrewMember (

			memberID,

			// side
			targetSide,

			/// icon
			NewIcon (memberID)
		);

		crewMember.IconObj.GetComponent<CrewIcon> ().Member = crewMember;

		return crewMember;
	}

	#region icons
	public GameObject NewIcon (MemberID memberID) {

		GameObject iconObj = Instantiate (memberPrefab) as GameObject;
		CrewIcon icon = iconObj.GetComponent<CrewIcon> ();

			// set object transform
		iconObj.transform.SetParent (crewParent);
		iconObj.transform.localScale = Vector3.one;
		iconObj.transform.position = Crews.getCrew (targetSide).CrewAnchors [(int)Crews.PlacingType.Hidden].position;

		Vector3 scale = new Vector3 ( TargetSide == Crews.Side.Enemy ? 1 : -1 , 1 , 1);

		icon.ControllerTransform.transform.localScale = scale;

		icon.HideBody ();

		// appearence

			// hair
		int hearIndex = memberID.hairSpriteID;
		if (hearIndex > -1)
			icon.HairImage.sprite = hairSprites [hearIndex];
		else
			icon.HairImage.enabled = false;
		icon.HairImage.color = hairColors [memberID.hairColorID];

			// body
		icon.BodyImage.sprite = bodySprites[memberID.male ? 0:1];


		return iconObj;
	}
	#endregion

	public Crews.Side TargetSide {
		get {
			return targetSide;
		}
		set {
			targetSide = value;
		}
	}

	#region sprites
	public Sprite[] HairSprites {
		get {
			return hairSprites;
		}
	}

	public Sprite[] BodySprites {
		get {
			return bodySprites;
		}
	}

	public Sprite[] BeardSprites {
		get {
			return beardSprites;
		}
	}

	public Sprite[] ClothesSprites {
		get {
			return clothesSprites;
		}
	}

	public Color[] HairColors {
		get {
			return hairColors;
		}
	}

	public string[] MaleNames {
		get {
			return maleNames;
		}
	}

	public string[] FemaleNames {
		get {
			return femaleNames;
		}
	}

	public int StartHealth {
		get {
			return startHealth;
		}
	}
	#endregion
}

public class MemberID {

		// name
	public int nameID 	= 0;

	public bool male = false;

		// lvl
	public int lvl 		= 0;

		// hp
	public int maxHP 	= 0;

		// stats
	public int str = 1;
	public int dex = 1;
	public int cha = 1;
	public int con = 1;

		// icon index
	public int bodyColorID = 0;

	public int hairSpriteID = 0;
	public int hairColorID 	= 0;
	public int beardSpriteID = 0;

	public Color clothColor;
	public int clothSpriteID = 0;

	public int voiceID = 0;

		// equipment
	public int weaponID = 0;
	public int clothesID = 0;
	public int shoesID = 0;

	public MemberID (CrewParams crewParams) {

		if (crewParams.overideGenre) {
			male = crewParams.male;
		} else {
			male = Random.value < 0.65f;
		}

		nameID 	= Random.Range (0, male ? CrewCreator.Instance.MaleNames.Length : CrewCreator.Instance.FemaleNames.Length );

		if (Crews.playerCrew.CrewMembers.Count > 0) {
			lvl = Random.Range (Crews.playerCrew.captain.Level - 1, Crews.playerCrew.captain.Level + 2);
//			Debug.Log ("random level given : " + lvl);
		} else {
			lvl = 1;
		}

		lvl = Mathf.Clamp ( lvl , 1 , 10 );

		maxHP 			= CrewCreator.Instance.StartHealth;

		int stats = lvl - 1;

		while ( stats > 0 )  {

			switch (Random.Range (0, 4)) {
			case 0:
				++str;
				break;
			case 1:
				++dex;
				break;
			case 2:
				++cha;
				break;
			case 3:
				++con;
				break;
			}

			--stats;
		}

		// il a 35% de chance d'être noir
		bodyColorID 	= Random.value < 0.35f ? 0 : 1;

		hairColorID 	= Random.Range ( 0 , CrewCreator.Instance.HairColors.Length  );
		int[] hairIDs 	= male ? CrewCreator.Instance.maleHairID : CrewCreator.Instance.femaleHairID;
		if (male) {

			hairSpriteID = Random.Range (-1, hairIDs.Length);
			if (hairSpriteID > -1)
				hairSpriteID = hairIDs [hairSpriteID];
		} else {
			hairSpriteID = hairIDs [Random.Range (0, hairIDs.Length)];
		}

		beardSpriteID 	= male ? Random.Range (-1 , CrewCreator.Instance.BeardSprites.Length) : -1;

		int[] clothIDs 	= male ? CrewCreator.Instance.maleClothesID : CrewCreator.Instance.femaleClothesID;
		clothSpriteID 	= clothIDs [Random.Range (0, clothIDs.Length)];
		clothColor 		= Random.ColorHSV();

		voiceID 		= Random.Range ( 0 , DialogueManager.Instance.SpeakSounds.Length );

		weaponID = ItemLoader.Instance.getRandomIDSpecLevel (ItemCategory.Weapon, lvl);
		clothesID = ItemLoader.Instance.getRandomIDSpecLevel (ItemCategory.Clothes, lvl);

	}

}

public struct CrewParams {
	public int amount;

	public bool overideGenre;
	public bool male;

	public CrewParams (
		int _amount,
		bool _overideGenre = false,
		bool _male = false
	)
	{
		amount = _amount;
		overideGenre = _overideGenre;
		male = _male;
	}

}

public class Crew {

	public bool hostile = false;

	public int InitCount = 0;

	public int Value = 0;

	public int row = 0;
	public int col = 0;

	List<MemberID> memberIDs = new List<MemberID>();

	public Crew (CrewParams crewParams, int r , int c) {

		row = r;
		col = c;

		InitCount = crewParams.amount;

		InitCount = Mathf.Clamp ( InitCount , 1, InitCount );
		for (int i = 0; i < InitCount; ++i) {
			MemberID id = new MemberID (crewParams);
			if (crewParams.overideGenre) {
				id.male = crewParams.male;
			}

			memberIDs.Add (id);
		}
		foreach (MemberID mID in MemberIDs)
			Value += mID.lvl;

	}

	public void Add ( MemberID id ) {
		memberIDs.Add (id);
	}

	public void Remove ( MemberID id ) {
		memberIDs.Remove (id);
	}

	public List<MemberID> MemberIDs {
		get {
			return memberIDs;
		}
	}
}