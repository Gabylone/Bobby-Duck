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

	float timer = 0f;
	[SerializeField]
	private float lerpDuration = 0.5f;
	private Color initColor;

	[Header("Sound")]
	[SerializeField] private AudioClip karmaGoodSound;
	[SerializeField] private AudioClip karmaBadSound;

	bool lerping = false;

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

	void Update () {

		UpdateLerp ();
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

		initColor = progressionImage.color;
		lerping = true;
		timer = 0f;

		Tween.Bounce (group.transform);

		SoundManager.Instance.PlaySound ( currentKarma < previousKarma ? karmaBadSound : karmaGoodSound );

	}

	public void FeedbackKarma () {
		//
	}


	void UpdateLerp ()
	{
		if (Visible && lerping) {

			if (timer < lerpDuration) {

				float targetFillAmount = ((float)currentKarma / (float)maxKarma);

				float lerp = timer / lerpDuration;

				float bef = (float)previousKarma / (float)maxKarma;
				float currentFillAmount = Mathf.Lerp (bef, targetFillAmount, lerp);

				progressionImage.fillClockwise = currentFillAmount > 0f;

				Color endColor = currentFillAmount < 0f ? Color.red : Color.green;

				if (currentFillAmount < 0)
					currentFillAmount = -currentFillAmount;

				Color targetColor = Color.Lerp (Color.white, endColor, currentFillAmount);
				progressionImage.color = Color.Lerp (initColor, targetColor, lerp);

				progressionImage.fillAmount = currentFillAmount;

			}

			if (timer >= lerpDuration) {
				lerping = false;
			}
			timer += Time.deltaTime;

		}
			
	}

	public int Bounty {
		get {
			return bounty;
		}
	}

	public void Show () {
		lerping = false;
		Visible = true;
	}
	public void Hide () {
		lerping = false;
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
}