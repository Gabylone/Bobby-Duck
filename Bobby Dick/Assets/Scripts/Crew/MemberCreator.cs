using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MemberCreator : MonoBehaviour {

	public static MemberCreator Instance;

	[SerializeField]
	private GameObject overall;

	public Sprite femaleSprite;
	public Sprite maleSprite;

	[SerializeField]
	InputField captainName;

	[SerializeField]
	InputField boatName;

	public Image jobImage;
	public Text jobText;

	public GameObject memberCreatorButtonParent;
	public MemberCreatorButton[] memberCreatorButtons;

	void Awake () {
		Instance = this;

		memberCreatorButtons = memberCreatorButtonParent.GetComponentsInChildren<MemberCreatorButton> ();
	}

	public string[] boatNames;
	public string[] captainNames;

	public void Show ()
	{
		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);

		overall.SetActive (true);

		UpdateButtons ();
		UpdateJob ();

		int ID = Random.Range ( 0, boatNames.Length );

		Boats.PlayerBoatInfo.Name = captainNames[ID];
		boatName.text = captainNames[ID];

		Crews.playerCrew.captain.MemberID.Name = boatNames [ID];
		captainName.text = Crews.playerCrew.captain.MemberID.Name;

	}

	public void Confirm () {

		overall.SetActive (false);

		StoryLauncher.Instance.PlayStory (Chunk.currentChunk.IslandData.storyManager, StoryLauncher.StorySource.island);
		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Big);
	}

	void UpdateJob ()
	{
		CrewMember member = Crews.playerCrew.captain;

		jobImage.sprite = SkillManager.jobSprites [(int)member.job];
		jobText.text = SkillManager.jobNames [(int)member.job];

		if (member.job == Job.Flibuster) {
			Item anyGun = System.Array.Find(ItemLoader.Instance.getItems(ItemCategory.Weapon), x => x.spriteID == 0 );
//			print ("setting any gun : " + anyGun.name);
			member.SetEquipment (CrewMember.EquipmentPart.Weapon, anyGun);
			Crews.playerCrew.captain.Icon.GetComponent<IconVisual> ().UpdateWeaponSprite (0);
		}

		if (member.job == Job.Brute) {
			Item anySword = System.Array.Find(ItemLoader.Instance.getItems(ItemCategory.Weapon), x => x.spriteID == 1 );
//			print ("setting any sword : " + anySword.name);
			member.SetEquipment (CrewMember.EquipmentPart.Weapon, anySword);
			Crews.playerCrew.captain.Icon.GetComponent<IconVisual> ().UpdateWeaponSprite (1);
		}



//		Tween.Boun		ce (jobImage.transform);
	}

	private void UpdateButtons () {

		foreach (var item in memberCreatorButtons) {
			item.UpdateImage ();
		}

		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Small);

	}

	public void ChangeBoatName () {

		Tween.Bounce ( boatName.transform );

		Boats.PlayerBoatInfo.Name = boatName.text;
		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Big);
	}

	public void ChangeCaptainName () {

		Tween.Bounce ( captainName.transform );

		Crews.playerCrew.captain.MemberID.Name = captainName.text;
		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Big);
	}


	public enum Apparence {
		genre,
		bodyColorID,
		hairSpriteID,
		hairColorID,
		eyeSpriteID,
		eyeBrowsSpriteID,
		beardSpriteID,
		noseSpriteID,
		mouthSpriteId,
		jobID,
	}

	public void ChangeApparence ( Apparence apparence ) {
		
		switch (apparence) {
		case Apparence.genre:
			Crews.playerCrew.captain.MemberID.Male = !Crews.playerCrew.captain.MemberID.Male;
			Crews.playerCrew.captain.MemberID.hairSpriteID = 0;
			Crews.playerCrew.captain.MemberID.beardSpriteID = -1;
			break;
		case Apparence.bodyColorID:
			Crews.playerCrew.captain.MemberID.bodyColorID++;
			break;
		case Apparence.hairSpriteID:
			
			Crews.playerCrew.captain.MemberID.hairSpriteID++;

			if (Crews.playerCrew.captain.MemberID.Male) {
				if (Crews.playerCrew.captain.MemberID.hairSpriteID == CrewCreator.Instance.HairSprites_Male.Length)
					Crews.playerCrew.captain.MemberID.hairSpriteID = -1;
			} else {
				if (Crews.playerCrew.captain.MemberID.hairSpriteID == CrewCreator.Instance.HairSprites_Female.Length)
					Crews.playerCrew.captain.MemberID.hairSpriteID = 0;
			}


			break;
		case Apparence.hairColorID:

			Crews.playerCrew.captain.MemberID.hairColorID++;

			if ( Crews.playerCrew.captain.MemberID.hairColorID == CrewCreator.Instance.HairColors.Length )
				Crews.playerCrew.captain.MemberID.hairColorID = 0;
			
			break;
		case Apparence.eyeSpriteID:
			Crews.playerCrew.captain.MemberID.eyeSpriteID++;

			if ( Crews.playerCrew.captain.MemberID.eyeSpriteID == CrewCreator.Instance.EyesSprites.Length )
				Crews.playerCrew.captain.MemberID.eyeSpriteID = 0;
			break;
		case Apparence.eyeBrowsSpriteID:
			Crews.playerCrew.captain.MemberID.eyebrowsSpriteID++;

			if ( Crews.playerCrew.captain.MemberID.eyebrowsSpriteID == CrewCreator.Instance.EyebrowsSprites.Length )
				Crews.playerCrew.captain.MemberID.eyebrowsSpriteID = 0;

			break;
		case Apparence.beardSpriteID:
			Crews.playerCrew.captain.MemberID.beardSpriteID++;

			if ( Crews.playerCrew.captain.MemberID.beardSpriteID == CrewCreator.Instance.BeardSprites.Length )
				Crews.playerCrew.captain.MemberID.beardSpriteID = -1;

			break;
		case Apparence.noseSpriteID:
			Crews.playerCrew.captain.MemberID.noseSpriteID++;

			if ( Crews.playerCrew.captain.MemberID.noseSpriteID == CrewCreator.Instance.NoseSprites.Length )
				Crews.playerCrew.captain.MemberID.noseSpriteID = 0;
			break;
		case Apparence.mouthSpriteId:
			Crews.playerCrew.captain.MemberID.mouthSpriteID++;

			if ( Crews.playerCrew.captain.MemberID.mouthSpriteID == CrewCreator.Instance.MouthSprites.Length )
				Crews.playerCrew.captain.MemberID.mouthSpriteID = 0;
			break;
		case Apparence.jobID:

			int jobIndex = (int)Crews.playerCrew.captain.MemberID.job + 1;

			if ((int)Crews.playerCrew.captain.MemberID.job == 4)
				jobIndex = 0;

			Crews.playerCrew.captain.MemberID.SetJob ((Job)jobIndex);
			Crews.playerCrew.captain.InitJob ();

			UpdateJob ();

			break;
		default:
			break;
		}

		UpdateButtons ();

		Crews.playerCrew.captain.Icon.UpdateVisual (Crews.playerCrew.captain.MemberID);

	}

}
