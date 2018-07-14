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

    public Animator animator;

	void Awake () {
		Instance = this;
	}

	void Start () {

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
		HOTween.To (GetStep (step).transform, tweenDuration / 2f, "anchoredPosition", Vector2.up * -1000f, false, EaseType.Linear, 0f);
	}
	public void ShowStep ( CreationStep step ) {

		confirmButtonObj.SetActive (false);

		Invoke ("ShowStepDelay", tweenDuration);

		if ( step > CreationStep.CaptainName ) {
			HideStep (step-1);
		}

		GetStep (step).SetActive (true);
		GetStep (step).GetComponent<RectTransform>().anchoredPosition = new Vector3 (0f , 1000f , 0f);
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

        CrewInventory.Instance.canOpen = false;

        Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Combat);

		overall.SetActive (true);
		ShowStep(currentStep);

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

        animator.SetTrigger("close");

		HOTween.To (rayblocker , tweenDuration , "color" , Color.clear);

		HideStep (CreationStep.Appearance);

		confirmButtonObj.SetActive (false);

        Crews.playerCrew.captain.Icon.transform.SetParent(iconInitParent);
        Crews.playerCrew.captain.Icon.MoveToPoint(Crews.PlacingType.Map);
        CrewInventory.Instance.canOpen = true;

        Invoke("EndMemberCreationDelay",tweenDuration);

	}
	void EndMemberCreationDelay () {

		Hide ();

		SaveManager.Instance.SaveGameData ();
		StoryLauncher.Instance.PlayStory (Chunk.currentChunk.IslandData.storyManager, StoryLauncher.StorySource.island);
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

	public void ChangeApparence ( ApparenceType apparence , int id) {

		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Small);

        Crews.playerCrew.captain.Icon.InitVisual (Crews.playerCrew.captain.MemberID);

	}

}
