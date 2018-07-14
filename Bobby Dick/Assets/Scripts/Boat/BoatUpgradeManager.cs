using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoatUpgradeManager : MonoBehaviour {

	public static BoatUpgradeManager Instance;

	public bool trading = false;

	public bool opened = false;


    public delegate void OnOpenBoatUpgrade();
    public OnOpenBoatUpgrade onOpenBoatUpgrade;

    public delegate void OnCloseBoatUpgrade();
    public OnCloseBoatUpgrade onCloseBoatUpgrade;

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
	private Image[] upgradeImages;
    [SerializeField]
    private RectTransform[] backgroundImages;

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
	public void Show () {

		menuObj.SetActive (true);

		UpdateInfo ();

		opened = true;

		CrewInventory.Instance.HideMenuButtons();

		if (onOpenBoatUpgrade != null)
			onOpenBoatUpgrade ();
	}

	void Hide () {
		menuObj.SetActive (false);

        if (onCloseBoatUpgrade != null)
            onCloseBoatUpgrade();
	}
	#endregion

	public int GetPrice ( UpgradeType upgradeType ) {

		switch (upgradeType) {
		case UpgradeType.Crew:
			return Boats.playerBoatInfo.crewCapacity * 100;
		case UpgradeType.Cargo:
			return Boats.playerBoatInfo.cargoLevel * 150;
		case UpgradeType.Longview:
			return Boats.playerBoatInfo.shipRange * 200;
		default:
			return 666;
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

    public void UpdateJauge ( int id , int fill, int max )
    {
        float l = (float)fill / max;
        float width = -backgroundImages[id].rect.width + backgroundImages[id].rect.width * l;

        Vector2 v = new Vector2(width, upgradeImages[id].rectTransform.sizeDelta.y);
        upgradeImages[id].rectTransform.sizeDelta = v;
    }

	public void UpdateInfo () {

		nameTextUI.text = Boats.playerBoatInfo.Name;

        int[] ehs = new int[3]
       {
            4,4,3
       };

        int[] maxes = new int[3]
        {
            3,4,3
        };

        int[] ids = new int[3]
        {
            Boats.playerBoatInfo.crewCapacity - 1,
            Boats.playerBoatInfo.cargoLevel,
            Boats.playerBoatInfo.shipRange
        };


        for (int i = 0; i < 3; i++) {

            UpdateJauge(i, ids[i], maxes[i]);

            if (ids[i] == ehs[i])
            {
                goldButtons[i].interactable = false;
                goldTexts[i].text = "MAX";
            }
            else
            {
                goldTexts[i].text = "" + GetPrice((UpgradeType)i);
            }

		}

		levelTextUI.text = "" + currentLevel;
//		levelImage.fillAmount = (float)currentLevel / (float)(upgradeMaxLevel*3);

		/*for (int i = 0; i < crewIcons.Length; ++i ) {
//			crewIcons [i].SetActive (i <= Crews.playerCrew.currentMemberCapacity);
			if ( i < Crews.playerCrew.CurrentMemberCapacity ) {
				crewIcons [i].GetComponentInChildren<Image> ().color = Color.white;
			} else {
				crewIcons [i].GetComponentInChildren<Image> ().color = Color.black;
			}
		}*/
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

        Hide();


	}

	void CloseDelay () {
		CrewInventory.Instance.ShowMenuButtons();
	}
}
