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

    public Transform iconTargetParent;
    public Transform iconInitParent;

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

	public GameObject memberCreatorButtonParent;
	public MemberCreatorButton[] memberCreatorButtons;


	public float tweenDuration = 0.7f;

	public Image rayblocker;

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

		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Combat);

		overall.SetActive (true);
		ShowStep(currentStep);

		UpdateButtons ();

		int ID = Random.Range ( 0, boatNames.Length );

		Boats.playerBoatInfo.Name = captainNames[ID];
		boatName.text = captainNames[ID];

		Crews.playerCrew.captain.MemberID.Name = boatNames [ID];
		captainName.text = Crews.playerCrew.captain.MemberID.Name;

        Crews.playerCrew.captain.Icon.transform.SetParent(iconTargetParent);

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

		HOTween.To (rayblocker , tweenDuration , "color" , Color.clear);

		HideStep (CreationStep.Appearance);

		confirmButtonObj.SetActive (false);

        Crews.playerCrew.captain.Icon.transform.SetParent(iconInitParent);


        Invoke("EndMemberCreationDelay",tweenDuration);
	}
	void EndMemberCreationDelay () {
		Hide ();

		SaveManager.Instance.SaveGameData ();
		StoryLauncher.Instance.PlayStory (Chunk.currentChunk.IslandData.storyManager, StoryLauncher.StorySource.island);
	}

	private void UpdateButtons () {

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

	public void ChangeApparence ( Apparence apparence , int id) {
		
		switch (apparence) {
		case Apparence.genre:
			Crews.playerCrew.captain.MemberID.Male = id == 0;
			Crews.playerCrew.captain.MemberID.hairSpriteID = 0;
			Crews.playerCrew.captain.MemberID.beardSpriteID = -1;
			break;
		case Apparence.bodyColorID:
			Crews.playerCrew.captain.MemberID.bodyColorID = id;
			break;
		case Apparence.hairSpriteID:
			Crews.playerCrew.captain.MemberID.hairSpriteID = id;
			break;
		case Apparence.hairColorID:
			Crews.playerCrew.captain.MemberID.hairColorID = id;
			break;
		case Apparence.eyeSpriteID:
			Crews.playerCrew.captain.MemberID.eyeSpriteID = id;
			break;
		case Apparence.eyeBrowsSpriteID:
			Crews.playerCrew.captain.MemberID.eyebrowsSpriteID = id;
			break;
		case Apparence.beardSpriteID:
			Crews.playerCrew.captain.MemberID.beardSpriteID = id;
			break;
		case Apparence.noseSpriteID:
			Crews.playerCrew.captain.MemberID.noseSpriteID = id;
			break;
		case Apparence.mouthSpriteId:
			Crews.playerCrew.captain.MemberID.mouthSpriteID++;
			break;
		case Apparence.jobID:

			Crews.playerCrew.captain.MemberID.SetJob ((Job)id);
			break;
		default:
			break;
		}

		UpdateButtons ();

		Crews.playerCrew.captain.Icon.InitVisual (Crews.playerCrew.captain.MemberID);

	}

}
