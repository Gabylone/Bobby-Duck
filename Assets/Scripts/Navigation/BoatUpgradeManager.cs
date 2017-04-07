using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoatUpgradeManager : MonoBehaviour {

	public static BoatUpgradeManager Instance;

	public bool trading = false;

	public enum UpgradeType {
		Crew,
		Cargo,
		Longview
	}

	void Awake () {
		Instance = this;
	}

	[SerializeField]
	private Text boatLevelText;
	private int boatCurrentLevel = 1;

	[SerializeField]
	private Button[] upgradeButtons;

	[Header("Crew")]
	[SerializeField]
	private Button[] crewButtons;

	[Header("Prices")]
	[SerializeField]
	private Text[] goldTexts;
	[SerializeField]
	private float[] upgradePrices = new float[3];
	private int [] upgradeLevels = new int[3]
	{1,1,1};

	[Header("Sounds")]
	[SerializeField] private AudioClip upgradeSound;

	[SerializeField]
	private UIButton upgradeUIButton;

	void Start () {
		Trading = false;
	}

	public void ShowUpgradeMenu () {

		upgradeUIButton.Opened = true;

		UpdateCrewImages ();
		UpdatePrices ();
		UpdateTexts ();
	}

	public void CloseUpgradeMenu () {
		upgradeUIButton.Opened = false;
	}


	public void Upgrade ( int i ) {

		if ( !GoldManager.Instance.CheckGold ( upgradePrices[i] ))
			return;

		switch ( (UpgradeType)i ) {
		case UpgradeType.Crew:
			Crews.playerCrew.MemberCapacity += 1;
			break;
		case UpgradeType.Cargo:
			WeightManager.Instance.CurrentCapacity *= 2;
			break;
		case UpgradeType.Longview:
			NavigationManager.Instance.ShipRange++;
			break;
		}

		GoldManager.Instance.GoldAmount -= (int)upgradePrices[i];

		SoundManager.Instance.PlaySound (upgradeSound);

		++boatCurrentLevel;
		++upgradeLevels [i];

		UpdatePrices ();
		UpdateTexts ();

	}

	public void UpdatePrices () {
		for (int i = 0; i < upgradePrices.Length; ++i ) {
			upgradePrices [i] = upgradePrices [i] * upgradeLevels [i];
		}
	}

	public void UpdateTexts () {
		for (int i = 0; i < goldTexts.Length; ++i)
			goldTexts[i].text = "" + upgradePrices[i];

		boatLevelText.text = "" + boatCurrentLevel;
	}

	public void UpdateCrewImages () {
		for (int i = 0; i < crewButtons.Length; ++i ) {
			crewButtons [i].gameObject.SetActive (i <= Crews.playerCrew.MemberCapacity);
			crewButtons [i].image.color = i == Crews.playerCrew.MemberCapacity ? Color.white : Color.black;
			crewButtons [i].interactable = i == Crews.playerCrew.MemberCapacity && trading;
		}
	}

	public bool Trading {
		get {
			return trading;
		}
		set {
			trading = value;

			foreach (Button button in upgradeButtons)
				button.interactable = value;

			foreach (Button button in crewButtons)
				button.interactable = value;

		}
	}

	public void Close () {
		if ( Trading == true ) {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();

			Trading = false;
		}

		CloseUpgradeMenu ();
	}
}
