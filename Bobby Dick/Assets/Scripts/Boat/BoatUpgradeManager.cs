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

	[Header("Name & Level")]
	[SerializeField]
	private Text nameTextUI;
	[SerializeField]
	private Text levelTextUI;
	[SerializeField]
	private Image levelImage;
	private int currentLevel = 1;

	[Header("UI Groups")]
	[SerializeField]
	private GameObject tradingGroup;
	[SerializeField]
	private GameObject infoGroup;
	[SerializeField]
	private GameObject openButton;
	[SerializeField]
	private GameObject menuObj;
	[SerializeField]
	private GameObject closeButton;

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
	[SerializeField]
	private Image[] upgradeImages;
	private int [] upgradeLevels = new int[3] {1,1,1};
	private int upgradeMaxLevel = 5;

	[Header("Sounds")]
	[SerializeField] private AudioClip upgradeSound;

	void Start () {
		Trading = false;

		upgradeLevels = new int[upgradePrices.Length];
		for (int i = 0; i < upgradeLevels.Length; i++) {
			upgradeLevels [i] = 1;
		}

		PlayerLoot.Instance.closeInventory += CloseUpgradeMenu;

		StoryFunctions.Instance.getFunction += HandleGetFunction;
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		if ( func == FunctionType.BoatUpgrades ) {
			ShowUpgradeMenu ();
			Trading = true;
		}
	}

	public void ShowUpgradeMenu () {

		menuObj.SetActive (true);

		UpdateInfo ();

		nameTextUI.text = Boats.Instance.PlayerBoatInfo.Name;

		PlayerLoot.Instance.CloseLoot ();

		Tween.Bounce (menuObj.transform, 0.2f, 1.05f);
		Tween.ClearFade (menuObj.transform);
	}

	public void CloseUpgradeMenu () {
		Tween.Scale (menuObj.transform,0.2f, 0.8f);
		Tween.Fade (menuObj.transform, 0.2f);

		Invoke ("HideMenu",0.2f);
	}

	void HideMenu () {
		menuObj.SetActive (false);
	}

	public void Upgrade ( int i ) {

		if ( !GoldManager.Instance.CheckGold ( upgradePrices[i] ))
			return;

		switch ( (UpgradeType)i ) {
		case UpgradeType.Crew:
			Crews.playerCrew.MemberCapacity += 1;
			break;
		case UpgradeType.Cargo:
			WeightManager.Instance.CurrentCapacity += 50;
			break;
		case UpgradeType.Longview:
			Boats.Instance.PlayerBoatInfo.ShipRange++;
			break;
		}

		GoldManager.Instance.GoldAmount -= (int)upgradePrices[i];

		SoundManager.Instance.PlaySound (upgradeSound);

		++currentLevel;
		++upgradeLevels [i];

		UpdateInfo ();

	}

	public void UpdateInfo () {
		for (int i = 0; i < upgradePrices.Length; ++i ) {
			upgradePrices [i] = upgradePrices [i] * upgradeLevels [i];
		}

		for (int i = 0; i < levelTexts.Length; ++i ) {
			levelTexts [i].text = "" + upgradeLevels [i];
			upgradeImages [i].fillAmount = (float)upgradeLevels [i] / (float)upgradeMaxLevel;
		}

		for (int i = 0; i < goldTexts.Length; ++i)
			goldTexts[i].text = "" + upgradePrices[i];

		levelTextUI.text = "" + currentLevel;
		levelImage.fillAmount = (float)currentLevel / (float)(upgradeMaxLevel*3);

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
