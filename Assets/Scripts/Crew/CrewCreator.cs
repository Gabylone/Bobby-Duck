using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
		"Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado", "Connecticut", "Delaware", "District of Columbia", "Florida", "Georgia", "Hawaii", "Idaho", "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky", "Louisiana", "Maine", "Maryland", "Massachusetts", "Michigan", "Minnesota", "Mississippi", "Missouri", "Montana", "Nebraska", "Nevada", "New Hampshire", "New Jersey", "New Mexico", "New York", "North Carolina", "North Dakota", "Ohio", "Oklahoma", "Oregon", "Pennsylvania", "Rhode Island", "South Carolina", "South Dakota", "Tennessee", "Texas", "Utah", "Vermont", "Virginia", "Washington", "West Virginia", "Wisconsin", "Wyoming"};

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
	
	// Update is called once per frame
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

	public int nameID 	= 0;

	public int lvl 		= 0;

	public int maxHP 	= 0;

	public int attack 	= 0;
	public int constitution = 0;
	public int speed = 0;

	public int bodyColorID = 0;

	public int hairSpriteID = 0;
	public int hairColorID 	= 0;
	public int beardSpriteID = 0;

	public Color clothColor;
	public int clothSpriteID = 0;

	public MemberID () {

		nameID 			= Random.Range (0, CrewCreator.Instance.Names.Length);

		lvl = 1;

		maxHP 			= CrewCreator.Instance.StartHealth;

		attack 			= lvl;
		constitution 	= lvl;
		speed 			= lvl;

		bodyColorID = Random.value < 0.35f ? 0 : 1;

		hairColorID 	= Random.Range ( 0 , CrewCreator.Instance.HairColors.Length );
		hairSpriteID 	= Random.Range (-1 , CrewCreator.Instance.HairSprites.Length	);
		beardSpriteID 	= Random.Range (-1 , CrewCreator.Instance.BeardSprites.Length	);

		clothSpriteID 	= Random.Range ( 0 , CrewCreator.Instance.ClothesSprites.Length	);
		clothColor 		= Random.ColorHSV();

	}

}

public class Crew {

	public int row = 0;
	public int col = 0;

	MemberID[] memberIDs;

	public Crew (int amount, int r , int c) {

		row = r;
		col = c;

		amount = Mathf.Clamp ( amount , 1, amount );
		memberIDs = new MemberID[amount];

		for (int i = 0; i < memberIDs.Length; ++i )
			memberIDs[i] = new MemberID ();
	}

	public MemberID[] MemberIDs {
		get {
			return memberIDs;
		}
	}
}