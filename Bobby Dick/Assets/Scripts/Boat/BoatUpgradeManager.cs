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
	public Button[] goldButtons;
	[SerializeField]
	private Text[] goldTexts;
	[SerializeField]
	private Text[] levelTexts;
	[SerializeField]
	private float[] upgradePrices = new float[3];
	[SerializeField]
	private Image[] upgradeImages;
	private int [] upgradeLevels = new int[3] {1,1,1};
	public int[] upgradeMaxLevel;

	[Header("Sounds")]
	[SerializeField] private AudioClip upgradeSound;

	void Start () {
		Trading = false;

		upgradeLevels = new int[upgradePrices.Length];
		for (int i = 0; i < upgradeLevels.Length; i++) {
			upgradeLevels [i] = 1;
		}

		CrewInventory.Instance.openInventory += HandleOpenInventory;

		goldButtons = tradingGroup.GetComponentsInChildren<Button> ();

		StoryFunctions.Instance.getFunction += HandleGetFunction;
	}

	void HandleOpenInventory (CrewMember member)
	{
		CloseUpgradeMenu ();
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

		nameTextUI.text = Boats.PlayerBoatInfo.Name;

		CrewInventory.Instance.HideInventory ();

		QuestMenu.Instance.Close ();

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
			Crews.playerCrew.currentMemberCapacity += 1;
			break;
		case UpgradeType.Cargo:
			WeightManager.Instance.CurrentCapacity += 50;
			break;
		case UpgradeType.Longview:
			Boats.PlayerBoatInfo.ShipRange++;
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
			upgradeImages [i].fillAmount = (float)upgradeLevels [i] / (float)upgradeMaxLevel[i];
		}

		for (int i = 0; i < goldButtons.Length; ++i) {
			if ( upgradeLevels[i] >= upgradeMaxLevel[i] ) {
				goldButtons [i].interactable = false;
				goldTexts [i].text = "MAX";
			} else {
				goldTexts[i].text = "" + upgradePrices[i];
			}
		}


		levelTextUI.text = "" + currentLevel;
//		levelImage.fillAmount = (float)currentLevel / (float)(upgradeMaxLevel*3);

		for (int i = 0; i < crewIcons.Length; ++i ) {
//			crewIcons [i].SetActive (i <= Crews.playerCrew.currentMemberCapacity);
			if ( i < Crews.playerCrew.currentMemberCapacity ) {
				crewIcons [i].GetComponentInChildren<Image> ().color = Color.white;
			} else {
				crewIcons [i].GetComponentInChildren<Image> ().color = Color.black;
			}
		}
	}

	public bool Trading {
		get {
			return trading;
		}
		set {
			trading = value;

//			infoGroup.SetActive (!value);
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
