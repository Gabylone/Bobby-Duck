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
	public int cargoAugmentation = 50;

	[Header("UI Groups")]
	[SerializeField]
	private GameObject menuObj;

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
	private Image[] upgradeImages;

	[Header("Sounds")]
	[SerializeField] private AudioClip upgradeSound;

	void Start () {
		
		Trading = false;

		CrewInventory.Instance.closeInventory += HandleCloseInventory;;

		StoryFunctions.Instance.getFunction += HandleGetFunction;
	}

	void HandleCloseInventory ()
	{
		Hide ();
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		if ( func == FunctionType.BoatUpgrades ) {
			Show ();
			Trading = true;
		}
	}

	#region show / hide
	public delegate void OnOpenBoatUpgrade ();
	public static OnOpenBoatUpgrade onOpenBoatUpgrade;
	public void Show () {

		menuObj.SetActive (true);

		UpdateInfo ();

		CrewInventory.Instance.HideMenuButtons();

		Tween.Bounce (menuObj.transform, 0.2f, 1.05f);

		if (onOpenBoatUpgrade != null)
			onOpenBoatUpgrade ();
	}

	void Hide () {

		menuObj.SetActive (false);

	}
	#endregion

	public int GetPrice ( UpgradeType upgradeType ) {

		switch (upgradeType) {
		case UpgradeType.Crew:
			return Boats.playerBoatInfo.crewCapacity * 100;
			break;
		case UpgradeType.Cargo:
			return Boats.playerBoatInfo.cargoLevel * 150;
			break;
		case UpgradeType.Longview:
			return Boats.playerBoatInfo.shipRange * 200;
			break;
		default:
			return 666;
			break;
		}

	}

	public delegate void OnUpgradeBoat ( UpgradeType upgradeType );
	public static OnUpgradeBoat onUpgradeBoat;
	public void Upgrade ( int i ) {

		if ( !GoldManager.Instance.CheckGold ( GetPrice((UpgradeType)i) ))
			return;

		switch ( (UpgradeType)i ) {
		case UpgradeType.Crew:
			Crews.playerCrew.CurrentMemberCapacity++;
			break;
		case UpgradeType.Cargo:
			Boats.playerBoatInfo.cargoLevel++;
			break;
		case UpgradeType.Longview:
			Boats.playerBoatInfo.shipRange++;
			break;
		}

		GoldManager.Instance.RemoveGold (GetPrice((UpgradeType)i));

		SoundManager.Instance.PlaySound (upgradeSound);

		++currentLevel;

		UpdateInfo ();

		if (onUpgradeBoat != null)
			onUpgradeBoat ((UpgradeType)i);
		
	}

	public void UpdateInfo () {

		nameTextUI.text = Boats.playerBoatInfo.Name;

		for (int i = 0; i < 3; i++) {

			switch ((UpgradeType)i) {
			case UpgradeType.Crew:
				
//				levelTexts [(int)UpgradeType.Crew].text = "" + Boats.playerBoatInfo.crewCapacity * 1;
				upgradeImages [i].fillAmount = (float)(Boats.playerBoatInfo.crewCapacity - 1) / 3f;
				if ( Boats.playerBoatInfo.crewCapacity == 4 ) {
					goldButtons [i].interactable = false;
					goldTexts [i].text = "MAX";
				} else {
					goldTexts [i].text = "" + GetPrice ((UpgradeType)i);
				}

				break;
			case UpgradeType.Cargo:

				upgradeImages [i].fillAmount = (float)Boats.playerBoatInfo.cargoLevel / 4f;
				if ( Boats.playerBoatInfo.cargoLevel == 4 ) {
					goldButtons [i].interactable = false;
					goldTexts [i].text = "MAX";
				} else {
					goldTexts [i].text = "" + GetPrice ((UpgradeType)i);
				}

				break;
			case UpgradeType.Longview:

				upgradeImages [i].fillAmount = (float)Boats.playerBoatInfo.shipRange / 3f;
				if ( Boats.playerBoatInfo.shipRange == 3 ) {
					goldButtons [i].interactable = false;
					goldTexts [i].text = "MAX";
				} else {
					goldTexts [i].text = "" + GetPrice ((UpgradeType)i);
				}

				break;
			default:
				break;
			}

		}

		levelTextUI.text = "" + currentLevel;
//		levelImage.fillAmount = (float)currentLevel / (float)(upgradeMaxLevel*3);

		for (int i = 0; i < crewIcons.Length; ++i ) {
//			crewIcons [i].SetActive (i <= Crews.playerCrew.currentMemberCapacity);
			if ( i < Crews.playerCrew.CurrentMemberCapacity ) {
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

			foreach (var item in goldButtons) {
				item.gameObject.SetActive (value);
			}
		}
	}

	public void Close () {
		
		if ( Trading == true ) {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();

			Trading = false;
		} else {
			CrewInventory.Instance.ShowMenuButtons();
		}

		Hide ();


	}
}
