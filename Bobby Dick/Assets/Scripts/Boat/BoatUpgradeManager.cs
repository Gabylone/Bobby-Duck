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
	private Text boatNameText;
	[SerializeField]
	private Text boatLevelText;
	private int boatCurrentLevel = 1;

	[SerializeField]
	private GameObject tradingGroup;

	[SerializeField]
	private GameObject infoGroup;

	[Header("Crew")]
	[SerializeField]
	private GameObject[] crewIcons;

	[Header("Prices")]
	[SerializeField]
	private Text[] goldTexts;
	[SerializeField]
	private Text[] levelTexts;
	[SerializeField]
	private float[] upgradePrices = new float[3];
	private int [] upgradeLevels = new int[3] {1,1,1};

	[Header("Sounds")]
	[SerializeField] private AudioClip upgradeSound;

	[SerializeField]
	private UIButton upgradeUIButton;

	void Start () {
		Trading = false;

		upgradeLevels = new int[upgradePrices.Length];
		for (int i = 0; i < upgradeLevels.Length; i++) {
			upgradeLevels [i] = 1;
		}

		PlayerLoot.Instance.closeInventory += CloseUpgradeMenu;
	}

	public void ShowUpgradeMenu () {

		upgradeUIButton.Opened = true;

		UpdateInfo ();

		boatNameText.text = Boats.Instance.PlayerBoatInfo.Name;
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
			Boats.Instance.PlayerBoatInfo.ShipRange++;
			break;
		}

		GoldManager.Instance.GoldAmount -= (int)upgradePrices[i];

		SoundManager.Instance.PlaySound (upgradeSound);

		++boatCurrentLevel;
		++upgradeLevels [i];

		UpdateInfo ();

	}

	public void UpdateInfo () {
		for (int i = 0; i < upgradePrices.Length; ++i ) {
			upgradePrices [i] = upgradePrices [i] * upgradeLevels [i];
		}

		for (int i = 0; i < levelTexts.Length; ++i ) {
			levelTexts [i].text = "" + upgradeLevels [i];
		}

		for (int i = 0; i < goldTexts.Length; ++i)
			goldTexts[i].text = "" + upgradePrices[i];

		boatLevelText.text = "" + boatCurrentLevel;

		for (int i = 0; i < crewIcons.Length; ++i ) {
			crewIcons [i].SetActive (i <= Crews.playerCrew.MemberCapacity);
			crewIcons [i].GetComponentInChildren<Image>().color = i == Crews.playerCrew.MemberCapacity ? Color.white : Color.black;
		}
	}

	public bool Trading {
		get {
			return trading;
		}
		set {
			trading = value;

			infoGroup.SetActive (!value);
			tradingGroup.SetActive (value);

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
