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
	private int maxHP = 10;

	[SerializeField]
	private int partAmount = 3;

	[Header("Face")]
	[SerializeField]
	private Sprite[] faceSprites;

	[Header("Hair & Beard")]
	[SerializeField]
	private Sprite[] hairSprites;

	[SerializeField]
	private Sprite[] beardSprites;

	[Header ("Clothes")]
	[SerializeField]
	private Sprite[] clothesSprites;

	[Header ("Colors")]
	[SerializeField] private Color lightBrown;
	[SerializeField] private Color darkSkin;
	[SerializeField] private Color darkHair;
	[SerializeField] private Color beige;
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

	public CrewMember NewMember () {
		
		CrewMember crewMember = new CrewMember (
			// name
			names[Random.Range (0,names.Length)],

			// lvl
			1,

			// health
			maxHP,
			// attack
			1,
			// constitution
			0,
			// speed
			1,

			// side
			targetSide,

			/// icon
			NewIcon ()
		);

		crewMember.IconObj.GetComponent<CrewIcon> ().Member = crewMember;

		return crewMember;
	}

	#region icons
	public GameObject NewIcon() {

		GameObject icon = Instantiate (memberPrefab) as GameObject;

		icon.transform.SetParent (crewParent);
		icon.transform.localScale = Vector3.one;
		icon.transform.localPosition = Vector2.zero;
		// get images

		Image[] images = new Image[icon.GetComponentsInChildren<Image>().Length];

		int a = 0;
		foreach (Image image in icon.GetComponentsInChildren<Image>()) {
			images[a] = image;
			++a;
		}

		// body
		Color bodyColor = Random.value < 0.35f ? darkSkin : beige;

		images[(int)Parts.Face].color = bodyColor;
		images[(int)Parts.Body].color = bodyColor;

		// beard && hair
		Color[] colors = new Color [7] {
			Color.red,
			Color.white,
			Color.black,
			Color.yellow,
			Color.gray,
			lightBrown,
			darkHair,
		};
		Color hairColor = colors [Random.Range (0, colors.Length)];

		Sprite[][] sprites = new Sprite[2][] {
			beardSprites,
			hairSprites,
		};

		int spritesIndex = 0;
		for (int i = (int)Parts.Beard; i <= (int)Parts.Hair; ++i ) {
			
			int index = Random.Range ( -1, sprites.Length );

			if (index > -1) {
//				images [i].sprite = sprites[spritesIndex][Random.Range (0, sprites.Length)];
				images [i].sprite = sprites[spritesIndex][Random.Range (0, sprites[spritesIndex].Length)];
				images [i].color = hairColor;
			} else {
				images [i].enabled = false;
			}

			spritesIndex++;
		}

		// clothes
		Color clothesColor = randomColor;
		images[(int)Parts.Clothes].sprite = clothesSprites[Random.Range (0,clothesSprites.Length)];
		images[(int)Parts.Clothes].color = clothesColor;

		for (int i = (int)Parts.LeftArm; i <= (int)Parts.RightArm; ++i )
			images[i].color = bodyColor;

		for (int i = (int)Parts.LeftFoot; i <= (int)Parts.RightFoot; ++i )
			images[i].color = clothesColor;

		icon.GetComponent<CrewIcon> ().HideBody ();


		return icon;
	}
	#endregion

	public Color randomColor {
		get {

			float r = Random.value;
			float g = Random.value;
			float b = Random.value;

			Color rdmColor = new Color (r,g,b);

			return rdmColor;
		}
	}

	public Crews.Side TargetSide {
		get {
			return targetSide;
		}
		set {
			targetSide = value;
		}
	}

	public int MaxHP {
		get {
			return maxHP;
		}
	}
}
