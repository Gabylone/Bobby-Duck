using UnityEngine;

public class MemberID {

	// name
	public string Name = "";

	public bool Male = false;

	// lvl
	public int Lvl 		= 0;

	// stats
	public int Str = 1;
	public int Dex = 1;
	public int Cha = 1;
	public int Con = 1;


	public MemberID () {

	}

	public MemberID (CrewParams crewParams) {

		if (crewParams.overideGenre) {
			Male = crewParams.male;
		} else {
			Male = Random.value < 0.5f;
		}

		if (Male) {
			Name = CrewCreator.Instance.MaleNames[Random.Range (0, CrewCreator.Instance.MaleNames.Length)];
		} else {
			Name = CrewCreator.Instance.FemaleNames[Random.Range (0, CrewCreator.Instance.FemaleNames.Length)];
		}

		if (crewParams.level > 0) {
			Lvl = crewParams.level;
		} else {

			if ( StoryReader.Instance.CurrentStoryHandler.storyType == StoryType.Quest ) {
				Debug.Log ("l'histoire est une quete, donc la crew est du meme niveau que ma quete");
			}

			Lvl = Random.Range (Crews.playerCrew.captain.Level - 3, Crews.playerCrew.captain.Level + 3);
		}

		Lvl = Mathf.Clamp ( Lvl , 1 , 10 );

		int stats = Lvl - 1;

		while ( stats > 0 )  {

			switch (Random.Range (0, 4)) {
			case 0:
				++Str;
				break;
			case 1:
				++Dex;
				break;
			case 2:
				++Cha;
				break;
			case 3:
				++Con;
				break;
			}

			--stats;
		}

		// il a 35% de chance d'être noir
		BodyColorID 	= Random.value < 0.35f ? 0 : 1;

		HairColorID 	= Random.Range ( 0 , CrewCreator.Instance.HairColors.Length  );
		if (Male) {
			HairSpriteID = Random.value > 0.2f ? Random.Range (0, CrewCreator.Instance.HairSprites_Male.Length) : -1;
		} else {
			HairSpriteID = Random.Range (0, CrewCreator.Instance.HairSprites_Female.Length);
		}

		BeardSpriteID 	= Male ? (Random.value > 0.2f ? Random.Range (0 , CrewCreator.Instance.BeardSprites.Length) : -1) : -1;
		EyeSpriteID 	= Random.Range (0 , CrewCreator.Instance.EyesSprites.Length);
		EyebrowsSpriteID= Random.Range (0 , CrewCreator.Instance.EyebrowsSprites.Length);
		NoseSpriteID 	= Random.Range (0 , CrewCreator.Instance.NoseSprites.Length);
		MouthSpriteID 	= Random.Range (0 , CrewCreator.Instance.MouthSprites.Length);

		VoiceID 		= Random.Range ( 0 , DialogueManager.Instance.SpeakSounds.Length );

		WeaponID = ItemLoader.Instance.getRandomIDSpecLevel (ItemCategory.Weapon, Lvl);
		ClothesID = ItemLoader.Instance.getRandomIDSpecLevel (ItemCategory.Clothes, Lvl);

	}

	// icon index
	private int bodyColorID = 0;

	public int BodyColorID {
		get {
			return bodyColorID;
		}
		set {

			bodyColorID = value;
		}
	}

	private int hairSpriteID = 0;

	public int HairSpriteID {
		get {
			return hairSpriteID;
		}
		set {

			int l = Male ? CrewCreator.Instance.HairSprites_Male.Length : CrewCreator.Instance.HairSprites_Female.Length;

			if (value < -1)
				value = l;

			if (value == l)
				value = 0;

			hairSpriteID = value;
		}
	}

	private int eyeSpriteID = 0;

	public int EyeSpriteID {
		get {
			return eyeSpriteID;
		}
		set {
			int l = CrewCreator.Instance.EyesSprites.Length;

			if (value < 0)
				value = l;

			if (value == l)
				value = 0;

			eyeSpriteID = value;
		}
	}

	private int eyebrowsSpriteID = 0;

	public int EyebrowsSpriteID {
		get {
			return eyebrowsSpriteID;
		}
		set {
			int l = CrewCreator.Instance.EyebrowsSprites.Length;

			if (value < 0)
				value = l;

			if (value == l)
				value = 0;

			eyebrowsSpriteID = value;
		}
	}

	private int hairColorID = 0;

	public int HairColorID {
		get {
			return hairColorID;
		}
		set {
			int l = CrewCreator.Instance.HairColors.Length;

			if (value < 0)
				value = l;

			if (value == l)
				value = -1;

			hairColorID = value;
		}
	}

	private int beardSpriteID 		= 0;

	public int BeardSpriteID {
		get {
			return beardSpriteID;
		}
		set {

			int l = CrewCreator.Instance.BeardSprites.Length;

			if (value < -1)
				value = l;

			if (value == l)
				value = -1;

			beardSpriteID = value;
		}
	}

	private int noseSpriteID 		= 0;

	public int NoseSpriteID {
		get {
			return noseSpriteID;
		}
		set {
			int l = CrewCreator.Instance.NoseSprites.Length;

			if (value < 0)
				value = l;

			if (value == l)
				value = 0;

			noseSpriteID = value;
		}
	}

	private int mouthSpriteID 		= 0;

	public int MouthSpriteID {
		get {
			return mouthSpriteID;
		}
		set {

			int l = CrewCreator.Instance.MouthSprites.Length;

			if (value < 0)
				value = l;

			if (value == l)
				value = 0;

			mouthSpriteID = value;
		}
	}

	private int voiceID 			= 0;

	public int VoiceID {
		get {
			return voiceID;
		}
		set {
			voiceID = value;
		}
	}

	private int weaponID 			= 0;

	public int WeaponID {
		get {
			return weaponID;
		}
		set {
			weaponID = value;
		}
	}

	private int clothesID 			= 0;

	public int ClothesID {
		get {
			return clothesID;
		}
		set {
			clothesID = value;
		}
	}

	private int shoesID 			= 0;

	public int ShoesID {
		get {
			return shoesID;
		}
		set {
			shoesID = value;
		}
	}
}