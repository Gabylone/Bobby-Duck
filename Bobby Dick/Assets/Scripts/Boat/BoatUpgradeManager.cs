using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoatUpgradeManager : MonoBehaviour {

	public static BoatUpgradeManager Instance;

	public bool trading = false;

	public bool opened = false;

	public enum UpgradeType {
		Crew,
		Cargo,
		Longview
	}

	public float timeBetweenButtonDisplay = 0.2f;

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

	[SerializeField]
	public GameObject[] buttonObjs;

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

		StoryFunctions.Instance.getFunction += HandleGetFunction;

		RayBlocker.onTouchRayBlocker += HandleOnTouchRayBlocker;

		Hide ();
	}

	void HandleOnTouchRayBlocker ()
	{
		if (opened)
			Close ();
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

		ShowAllButtons ();

		UpdateInfo ();

		opened = true;

		CrewInventory.Instance.HideMenuButtons();

		if (onOpenBoatUpgrade != null)
			onOpenBoatUpgrade ();
	}
	void ShowAllButtons () {
		StartCoroutine (ShowAllButtonsCoroutine ());
	}
	void HideAllButtons () {
//		StartCoroutine (HideAllButtonsCoroutine ());
		Hide();
	}

	IEnumerator ShowAllButtonsCoroutine () {

		foreach (var item in buttonObjs) {
			item.SetActive (false);
			Tween.ClearFade (item.transform);
		}

		foreach (var item in buttonObjs) {
			item.SetActive (true);
			Tween.Bounce (item.transform);
			yield return new WaitForSeconds ( timeBetweenButtonDisplay );

		}

		yield return new WaitForEndOfFrame ();
	}


	IEnumerator HideAllButtonsCoroutine () {

		for (int i = 0; i < buttonObjs.Length; i++) {

			int index = buttonObjs.Length -1 - i;

//			Tween.Bounce (buttonObjs [index].transform, timeBetweenButtonDisplay , 0.f);
			buttonObjs [index].SetActive (false);
//			Tween.Fade (buttonObjs[index].transform, timeBetweenButtonDisplay);

			yield return new WaitForSeconds ( timeBetweenButtonDisplay );

			if (index > 0) {
			}

		}

		yield return new WaitForEndOfFrame ();

		Hide ();
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

		GoldManager.Instance.RemoveGold (GetPrice((UpgradeType)i));

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
		
		opened = false;

		if ( Trading == true ) {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();

			Trading = false;
		} else {
			Invoke ("CloseDelay",0.01f);
		}

		HideAllButtons ();


	}

	void CloseDelay () {
		CrewInventory.Instance.ShowMenuButtons();
	}
}
