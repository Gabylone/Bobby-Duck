using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

public class MemberCreator : MonoBehaviour {

	public static MemberCreator Instance;

	public enum CreationStep {
		CaptainName,
		BoatName,
		Job,
		Appearance
	}
	public CreationStep currentStep;

	public GameObject confirmButtonObj;

	[SerializeField]
	private GameObject overall;

	public Color initColor;

	public GameObject[] stepObjs;
	public GameObject GetStep ( CreationStep step ) {
		return stepObjs [(int)step];
	}

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

	public float tweenDuration = 0.7f;

	public RayBlocker rayblocker;

	void Awake () {
		Instance = this;
	}

	void Start () {

//		initColor = rayblocker.color;

		Hide ();
	}

	public string[] boatNames;
	public string[] captainNames;

	void Hide ()
	{
		overall.SetActive (false);

		foreach (var item in stepObjs) {
			item.SetActive (false);
		}

	}

	public void HideStep (CreationStep step) {
		HOTween.To (GetStep (step).transform, tweenDuration / 2f, "anchoredPosition", Vector2.right * -1000f, false, EaseType.Linear, 0f);
	}
	public void ShowStep ( CreationStep step ) {

		rayblocker.Show ();

		confirmButtonObj.SetActive (false);

		Invoke ("ShowStepDelay", tweenDuration);

		if ( step > CreationStep.CaptainName ) {
			HideStep (step-1);
		}

		GetStep (step).SetActive (true);
		GetStep (step).transform.localPosition = new Vector3 (1000f , 0f , 0f);
		HOTween.To (GetStep (step).transform, tweenDuration, "anchoredPosition", Vector2.zero, false, EaseType.Linear, 0f);

	}
	void ShowStepDelay () {
		if ( currentStep > CreationStep.CaptainName ) {
			GetStep (currentStep - 1).SetActive(false);
		}
		confirmButtonObj.SetActive (true);
		Tween.Bounce (confirmButtonObj.transform);
	}

	public void Show ()
	{
		Transitions.Instance.ActionTransition.FadeIn (0.5f);

		currentStep = CreationStep.CaptainName;

		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);

		overall.SetActive (true);
		ShowStep(currentStep);

		UpdateButtons ();
		UpdateJob ();

		int ID = Random.Range ( 0, boatNames.Length );

		Boats.playerBoatInfo.Name = captainNames[ID];
		boatName.text = captainNames[ID];

		Crews.playerCrew.captain.MemberID.Name = boatNames [ID];
		captainName.text = Crews.playerCrew.captain.MemberID.Name;

	}

	public void Confirm () {

		if ( currentStep == CreationStep.Appearance ) {


			EndMemberCreation ();

		} else {

			++currentStep;
			ShowStep (currentStep);


		}

		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Big);


	}
	void EndMemberCreation () {

		rayblocker.Hide ();

		HideStep (CreationStep.Appearance);

		confirmButtonObj.SetActive (false);

		Invoke ("EndMemberCreationDelay",tweenDuration);
	}
	void EndMemberCreationDelay () {
		Hide ();

		SaveManager.Instance.SaveGameData ();
		StoryLauncher.Instance.PlayStory (Chunk.currentChunk.IslandData.storyManager, StoryLauncher.StorySource.island);
	}

	void UpdateJob ()
	{
		CrewMember member = Crews.playerCrew.captain;

		jobImage.sprite = SkillManager.jobSprites [(int)member.job];
		jobText.text = SkillManager.jobNames [(int)member.job];

	}

	private void UpdateButtons () {

		foreach (var item in memberCreatorButtons) {
			item.UpdateImage ();
		}

		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Small);

	}

	public void ChangeBoatName () {

		Tween.Bounce ( boatName.transform , 0.2f , 1.05f);

		Boats.playerBoatInfo.Name = boatName.text;
		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Big);
	}

	public void ChangeCaptainName () {

		Tween.Bounce ( captainName.transform , 0.2f , 1.05f);

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

			UpdateJob ();

			break;
		default:
			break;
		}

		UpdateButtons ();

		Crews.playerCrew.captain.Icon.InitVisual (Crews.playerCrew.captain.MemberID);

	}

}
