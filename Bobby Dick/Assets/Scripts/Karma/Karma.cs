using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Karma : MonoBehaviour {

	public static Karma Instance;

	private bool visible = false;

	private int currentKarma = 0;
	private int previousKarma = 0;

	private int bounty = 0;

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
			AddKarma();
			break;
		case FunctionType.RemoveKarma:
			RemoveKarma();
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
		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}

	public void RemoveKarma () {
		--CurrentKarma;

		bounty += bountyStep;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}

	public void PayBounty () {

		StoryReader.Instance.NextCell ();

		if ( GoldManager.Instance.CheckGold (Bounty) ) {

			CurrentKarma = -2;

			GoldManager.Instance.GoldAmount -= Bounty;

		} else {

			StoryReader.Instance.SetDecal (1);

			--CurrentKarma;

		}

		StoryReader.Instance.UpdateStory ();

	}

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

		float fill = ((float)currentKarma / (float)maxKarma);

		if ( fill < 0.5 && fill > -0.5f ) {
			feedbackImage.sprite = sprites [2];
		} else if (fill < 0) {
			feedbackImage.sprite = sprites [1];
		} else {
			feedbackImage.sprite = sprites [0];
		}

		Visible = true;

		float targetFillAmount = ((float)currentKarma / (float)maxKarma);
		progressionImage.fillAmount = targetFillAmount;

		Color endColor = targetFillAmount < 0f ? Color.red : Color.green;
		progressionImage.color = endColor;

		Tween.Bounce (group.transform);

		SoundManager.Instance.PlaySound ( currentKarma < previousKarma ? karmaBadSound : karmaGoodSound );

	}

	public void FeedbackKarma () {
		//
	}


	public int Bounty {
		get {
			return bounty;
		}
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
		SaveManager.Instance.CurrentData.karma = CurrentKarma;
		SaveManager.Instance.CurrentData.bounty = Bounty;
	}

	public void LoadKarma ()
	{
		print ("loading kjar");
		CurrentKarma = SaveManager.Instance.CurrentData.karma;
		bounty = SaveManager.Instance.CurrentData.bounty;
	}
}