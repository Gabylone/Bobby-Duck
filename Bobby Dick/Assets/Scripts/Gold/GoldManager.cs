using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoldManager : MonoBehaviour {

	public static GoldManager Instance;

	[Header ("UI Elements")]
	[SerializeField] private GameObject goldGroup;
	[SerializeField] private Text goldText;
	[SerializeField] private Image goldImage;

	[SerializeField] private float feedbackDuration = 0.2f;
	bool feedbackActive = false;
	float timer = 0f;

	[Header ("Amounts")]
	[SerializeField]
	private int startValue = 200;
	private int goldAmount = 0;

	[Header("Sound")]
	[SerializeField] private AudioClip noGoldSound;
	[SerializeField] private AudioClip buySound;

	void Awake () {
		Instance = this;
	}

	void Start () {
		GoldAmount = startValue;

		PlayerLootUI.Instance.openInventory += Show;
		PlayerLootUI.Instance.closeInventory += Hide;

		Hide ();
	}

	void Update () {
		if ( feedbackActive ) {
			timer += Time.deltaTime;

			if ( timer > feedbackDuration ) {
				HideFeedback ();
			}
		}
	}

	public void SetGoldDecal () {
		int amount = int.Parse (StoryFunctions.Instance.CellParams);

		if (GoldManager.Instance.CheckGold (amount)) {
			StoryReader.Instance.NextCell ();
		} else {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.SetDecal (1);
		}

		StoryReader.Instance.UpdateStory ();
	}

	public bool CheckGold ( float amount ) {
		
		if ( amount > GoldAmount ) {
			SoundManager.Instance.PlaySound (noGoldSound);
			DisplayFeedback ();
			return false;
		}

		SoundManager.Instance.PlaySound (buySound);

		return true;
	}

	private void DisplayFeedback () {

		feedbackActive = true;
		timer = 0f;

		Visible = true;

	}
	private void HideFeedback () {
		feedbackActive = false;

		Visible = false;
	}

	public void UpdateUI () {
		goldText.text = goldAmount.ToString ();
	}

	public int GoldAmount {
		get {
			return goldAmount;
		}
		set {
			goldAmount = Mathf.Clamp (value, 0 , value );
			UpdateUI ();

			DisplayFeedback ();
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
			return goldGroup.activeSelf;
		}
		set {
			goldGroup.SetActive (value);
		}
	}
}
