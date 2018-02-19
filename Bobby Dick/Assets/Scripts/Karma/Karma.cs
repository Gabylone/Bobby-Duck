using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Karma : MonoBehaviour {

	public static Karma Instance;

	private bool visible = false;

	private int currentKarma = 0;
	private int previousKarma = 0;

	public int bounty = 0;

	[Header("Params")]
	[SerializeField]
	private int bountyStep = 10;

	public int maxKarma = 10;

	[Header("UI")]
	[SerializeField]
	private GameObject group;
	[SerializeField]
	private Sprite[] sprites;
	[SerializeField]
	private Image feedbackImage;
	[SerializeField]
	private Image progressionImage;

	[Header("Sound")]
	[SerializeField] private AudioClip karmaGoodSound;
	[SerializeField] private AudioClip karmaBadSound;


	void Awake () {
		Instance = this;
	}
	void Start () {

//		CrewInventory.Instance.openInventory += HandleOpenInventory;;
//		CrewInventory.Instance.closeInventory += Hide;

		StoryFunctions.Instance.getFunction+= HandleGetFunction;

		UpdateUI ();

	}
//
//	void Update () {
//		if ( Input.GetKeyDown(KeyCode.I) ) {
//			++CurrentKarma;
//		}
//		if ( Input.GetKeyDown(KeyCode.U) ) {
//			--CurrentKarma;
//		}
//	}

	void HandleOpenInventory (CrewMember member)
	{
		Show ();
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.CheckKarma:
			CheckKarma ();
			break;
		case FunctionType.AddKarma:
			AddKarma_Story();
			break;
		case FunctionType.RemoveKarma:
			RemoveKarma_Story();
			break;
		case FunctionType.PayBounty:
			PayBounty();
			break;
		}
	}



	public void CheckKarma () {

		int decal = 0;

		if ( CurrentKarma > (float)(maxKarma / 2) ) {
			decal = 0;
			// un exemple de moralité
		} else if ( CurrentKarma > 0 ) {
			decal = 1;
			// rien à signaler
		} else if ( CurrentKarma > -(float)(maxKarma/2) ) {
			decal = 2;
			// un mec louche
		} else {
			decal = 3;
			// une sous merde
		}

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.SetDecal (decal);

		StoryReader.Instance.UpdateStory ();

	}

	public void AddKarma () {
		
		++CurrentKarma;

		if (onChangeKarma != null)
			onChangeKarma (previousKarma, currentKarma);
		//
	}
	public void RemoveKarma () {
		
		--CurrentKarma;

		bounty += bountyStep;

		if (onChangeKarma != null)
			onChangeKarma (previousKarma, currentKarma);

	}
	public void AddKarma_Story () {

		AddKarma ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}

	public void RemoveKarma_Story () {

		RemoveKarma ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}

	public void PayBounty () {

		StoryReader.Instance.NextCell ();

		if ( GoldManager.Instance.CheckGold (bounty) ) {

			CurrentKarma = -2;

			GoldManager.Instance.RemoveGold (bounty);

		} else {

			StoryReader.Instance.SetDecal (1);

			RemoveKarma ();

		}

		StoryReader.Instance.UpdateStory ();

	}

	public delegate void OnChangeKarma ( int previousKarma , int newKarma );
	public static OnChangeKarma onChangeKarma;
	public int CurrentKarma {
		get {
			return currentKarma;
		}
		set {

			previousKarma = CurrentKarma;

			currentKarma = Mathf.Clamp ( value , -maxKarma , maxKarma);

			UpdateUI ();
		}
	}

	public void UpdateUI () {

		float targetFillAmount = ((float)currentKarma / (float)maxKarma);

		if ( targetFillAmount < 0.5 && targetFillAmount > -0.5f ) {
			feedbackImage.sprite = sprites [2];
		} else if (targetFillAmount < 0) {
			feedbackImage.sprite = sprites [1];
		} else {
			feedbackImage.sprite = sprites [0];
		}

		if (currentKarma < 0) {
			targetFillAmount = -targetFillAmount;
			progressionImage.fillClockwise = false;
			progressionImage.color = Color.red;
			SoundManager.Instance.PlaySound ( karmaBadSound );
		} else {
			progressionImage.fillClockwise = true;
			progressionImage.color = Color.green;
			SoundManager.Instance.PlaySound ( karmaGoodSound );
		}

		progressionImage.fillAmount = targetFillAmount;

		Tween.Bounce (group.transform);


	}

	public void FeedbackKarma () {
		//
	}

	public void Show () {
		Visible = true;
	}

	public void Hide () {
		Visible = false;
	}

	public bool Visible {
		get {
			return visible;
		}
		set {
			visible = value;

			group.SetActive (value);
		}
	}

	public void SaveKarma ()
	{
		SaveManager.Instance.GameData.karma = CurrentKarma;
		SaveManager.Instance.GameData.bounty = bounty;
	}

	public void LoadKarma ()
	{
		CurrentKarma = SaveManager.Instance.GameData.karma;
		bounty = SaveManager.Instance.GameData.bounty;
	}
}