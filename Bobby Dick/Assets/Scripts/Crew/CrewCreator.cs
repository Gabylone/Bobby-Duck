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
	private GameObject[] memberIconPrefabs;


	public string[] maleNames = new string[51] {
		"Jean","Eric", "Nathan", "Jacques", "Benoit", "Jeremy", "Jerome", "Bertrand", "Vladimir", "Dimitri", "Jean-Jacques", "Gérard", "Nestor", "Etienne", "Leon", "Henry", "David", "Esteban", "Louis", "Carles", "Victor", "Michel", "Gabriel", "Pierre", "André", "Fred", "Cassius", "César", "Paul", "Martin", "Claude", "Levis", "Alex", "Olivier", "Mustafa", "Nicolas", "Chris", "Oleg", "Emile", "Richard", "Romulus", "Rufus", "Stan", "Charles", "Quincy", "Antoine", "Virgile", "Boromir", "Archibald", "Eddy", "Kenneth"
	};

	public string[] femaleNames = new string[51] {
		"Jeanne","Erica", "Nathalie", "Jacquelines", "Barbara", "Ella", "Flo", "Laura", "Natasha", "Irene", "Yvonne", "Gérarde", "Nelly", "Elisa", "Adele", "Henriette", "Alice", "Esteban", "Louise", "Carla", "Victoria", "Michelle", "Gabrielle", "Sarah", "Andréa", "Marion", "Valentine", "Cléopatre", "Pauline", "Martine", "Claudette", "Nina", "Alexandra", "Clementine", "Julia", "Olivia", "Christine", "Rose", "Emilia", "Agathe", "Lily", "Claire", "Yasmine", "Charlotte", "Scarlett", "Marina", "Virginie", "Anaïs", "Tatiana", "Cécile", "Marianne"
	};

	[Header("Hair")]
	[SerializeField]
	private Sprite[] hairSprites_Male;
	[SerializeField]
	private Sprite[] hairSprites_Female;
	public int[] femaleHairID 	= new int[0];
	public int[] maleHairID 	= new int[0];

	[SerializeField]
	private Sprite[] bodySprites;


	[Header("FaceParts")]
	[SerializeField]
	private Sprite[] eyesSprites;
	[SerializeField]
	private Sprite[] eyebrowsSprites;
	[SerializeField]
	private Sprite[] noseSprite;
	[SerializeField]
	private Sprite[] mouthSprite;
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

	public Color Beige {
		get {
			return beige;
		}
	}
	#endregion

	void Awake () {
		Instance = this;
	}

	public CrewMember NewMember (Member memberID) {

		CrewMember crewMember = new CrewMember (

			memberID,

			// side
			targetSide,

			NewIcon(memberID)

		);

		return crewMember;
	}

	#region icons
	public MemberIcon NewIcon (Member memberID) {

		GameObject iconObj = Instantiate (memberIconPrefabs[(int)targetSide]) as GameObject;
		MemberIcon icon = iconObj.GetComponent<MemberIcon> ();

		// set object transform
		iconObj.transform.SetParent (crewParent);
		iconObj.transform.localScale = Vector3.one;
		iconObj.transform.position = Crews.getCrew (targetSide).CrewAnchors [(int)Crews.PlacingType.Hidden].position;

		Vector3 scale = new Vector3 ( TargetSide == Crews.Side.Enemy ? 1 : -1 , 1 , 1);

		icon.group.transform.localScale = scale;

		icon.HideBody ();


		return iconObj.GetComponent<MemberIcon> ();
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
	public Sprite[] HairSprites_Male {
		get {
			return hairSprites_Male;
		}
	}
	public Sprite[] HairSprites_Female {
		get {
			return hairSprites_Female;
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

	public Sprite[] EyesSprites {
		get {
			return eyesSprites;
		}
	}

	public Sprite[] EyebrowsSprites {
		get {
			return eyebrowsSprites;
		}
	}

	public Color[] HairColors {
		get {
			return hairColors;
		}
	}

	public Sprite[] MouthSprites {
		get {
			return mouthSprite;
		}
	}

	public Sprite[] NoseSprites {
		get {
			return noseSprite;
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
	#endregion
}

