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


	private string[] names = new string[51] {
		"Jean", "Eric", "Nathan", "Jacques", "Benoit", "Jeremy", "Flo", "Bertrand", "Vladimir", "Dimitri", "Jean-Jacques", "Gérard", "Nestor", "Etienne", "Leon", "Henry", "David", "Esteban", "Louis", "Carles", "Victor", "Michel", "Gabriel", "Pierre", "André", "Fred", "Cassius", "César", "Paul", "Martin", "Claude", "Levis", "Alex", "Olivier", "Mustafa", "Nicolas", "Chris", "Oleg", "Emile", "Richard", "Romulus", "Rufus", "Stan", "Charles", "Quincy", "Antoine", "Virgile", "Boromir", "Archibald", "Eddy", "Kenneth"
	};

	[SerializeField]
	private int startHealth = 10;

	[SerializeField]
	private int partAmount = 3;

	[Header("Sprites")]
	[SerializeField]
	private Sprite[] faceSprites;
	[SerializeField]
	private Sprite[] hairSprites;
	[SerializeField]
	private Sprite[] beardSprites;
	[SerializeField]
	private Sprite[] clothesSprites;

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
	public GameObject NewIcon(MemberID memberID) {

		GameObject icon = Instantiate (memberPrefab) as GameObject;

		icon.transform.SetParent (crewParent);

		icon.transform.localScale = Vector3.one;

		icon.transform.localPosition = Vector2.zero;

		Image[] images = icon.GetComponentsInChildren<Image> ();

		// body
		Color bodyColor = memberID.bodyColorID == 1 ? darkSkin : beige;

		images[(int)Parts.Face].color = bodyColor;
		images[(int)Parts.Body].color = bodyColor;

		int beardIndex = memberID.beardSpriteID;
		if (beardIndex > -1)
			images[(int)Parts.Beard].sprite = beardSprites [beardIndex];
		else
			images[(int)Parts.Beard].enabled = false;
		images[(int)Parts.Beard].color = hairColors [memberID.hairColorID];

		int hearIndex = memberID.hairSpriteID;
		if (hearIndex > -1)
			images[(int)Parts.Hair].sprite = hairSprites [hearIndex];
		else
			images[(int)Parts.Hair].enabled = false;
		images[(int)Parts.Hair].color = hairColors [memberID.hairColorID];

		// clothes ( needs to be an int from set of color )
		Color clothesColor = memberID.clothColor;
		images[(int)Parts.Clothes].sprite = clothesSprites[memberID.clothSpriteID];
		images[(int)Parts.Clothes].color 	= clothesColor;

		images[(int)Parts.LeftFoot].color = clothesColor;
		images[(int)Parts.RightFoot].color 	= clothesColor;

		images[(int)Parts.LeftArm].color = bodyColor;
		images[(int)Parts.RightArm].color = bodyColor;

		images[(int)Parts.Sword].color 	= Color.grey;

		icon.GetComponent<CrewIcon> ().HideBody ();
		Vector3 scale = new Vector3 ( TargetSide == Crews.Side.Player ? 1 : -1 , 1 , 1);

		icon.GetComponent<CrewIcon> ().FaceObj.transform.localScale = scale;
		icon.GetComponent<CrewIcon> ().BodyObj.transform.localScale = scale;

		return icon;
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

	public Sprite[] HairSprites {
		get {
			return hairSprites;
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

	public string[] Names {
		get {
			return names;
		}
	}

	public int StartHealth {
		get {
			return startHealth;
		}
	}
}

public class MemberID {

		// name
	public int nameID 	= 0;

		// lvl
	public int lvl 		= 0;

		// hp
	public int maxHP 	= 0;

		// stats
	public int str = 0;
	public int dex = 0;
	public int cha = 0;
	public int con = 0;

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

	public MemberID () {

		nameID 			= Random.Range (0, CrewCreator.Instance.Names.Length);

		if (Crews.playerCrew.CrewMembers.Count > 0)
			lvl = Random.Range ( Crews.playerCrew.captain.Level -1 , Crews.playerCrew.captain.Level + 2 );
		else
			lvl = 1;

		lvl = Mathf.Clamp ( lvl , 1 , 10 );

		maxHP 			= CrewCreator.Instance.StartHealth;

		str = lvl;
     	dex = lvl;
     	cha = lvl;
		con = lvl;

		// il a 35% de chance d'être noir
		bodyColorID 	= Random.value < 0.35f ? 0 : 1;

		hairColorID 	= Random.Range ( 0 , CrewCreator.Instance.HairColors.Length  );
		hairSpriteID 	= Random.Range (-1 , CrewCreator.Instance.HairSprites.Length );
		beardSpriteID 	= Random.Range (-1 , CrewCreator.Instance.BeardSprites.Length);

		clothSpriteID 	= Random.Range ( 0 , CrewCreator.Instance.ClothesSprites.Length	);
		clothColor 		= Random.ColorHSV();

		voiceID 		= Random.Range ( 0 , DialogueManager.Instance.SpeakSounds.Length );

		weaponID = ItemLoader.Instance.getRandomIDSpecLevel (ItemCategory.Weapon, lvl);
		clothesID = ItemLoader.Instance.getRandomIDSpecLevel (ItemCategory.Clothes, lvl);
//		shoesID = ItemLoader.Instance.getRandomIDSpecLevel (ItemCategory.Shoes, lvl);

	}

}

public class Crew {

	public bool hostile = false;

	public int InitCount = 0;

	public int Value = 0;

	public int row = 0;
	public int col = 0;

	List<MemberID> memberIDs = new List<MemberID>();

	public Crew (int amount, int r , int c) {

		row = r;
		col = c;

		InitCount = amount;


		amount = Mathf.Clamp ( amount , 1, amount );
		for (int i = 0; i < amount; ++i)
			memberIDs.Add (new MemberID ());

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