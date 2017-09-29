using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

public class WeightManager : MonoBehaviour {

	public static WeightManager Instance;

	private int currentCapacity = 100;

	[Header("UI Elements")]
	[SerializeField]
	private GameObject weightGroup;
	[SerializeField]
	private Image weightImage;
	[SerializeField]
	private Text currentWeightText;

	private bool displayingFeedback = false;
	private float timer = 0f;
	[SerializeField]
	private float feedbackDuration = 0.3f;
	[SerializeField]
	private float feedbackBounceDuration = 0.3f;
	[SerializeField]
	private float feedbackScaleAmount = 1.3f;

	[Header("Sound")]
	[SerializeField] 
	private AudioClip noRoomSound;

	void Awake () {
		Instance = this;
	}

	public void Init () {
//
		CrewInventory.Instance.openInventory += HandleOpenInventory;;
		CrewInventory.Instance.closeInventory += Hide;

		LootUI.useInventory += UpdateDisplay;

		LootManager.Instance.updateLoot += UpdateDisplay;

		Hide();

		UpdateDisplay ();
	}

	void HandleOpenInventory (CrewMember member)
	{
		Show ();
	}

	void Update () {
		if ( displayingFeedback ) {
			timer += Time.deltaTime;

			if ( timer > feedbackDuration ) {
				HideFeedback ();
			}
		}
	}

	private void Bounce () {
		HOTween.To ( weightGroup.transform , feedbackBounceDuration , "localScale" , Vector3.one * feedbackScaleAmount, false , EaseType.EaseOutBounce , 0f);
		HOTween.To ( weightGroup.transform , feedbackBounceDuration , "localScale" , Vector3.one , false , EaseType.Linear , feedbackBounceDuration );
	}

	#region weight control
	public bool CheckWeight ( int amount ) {

		DisplayFeedback ();

		if ( CurrentWeight + amount > currentCapacity ) {

			SoundManager.Instance.PlaySound ( noRoomSound );

			Bounce ();

//			currentWeightText.color = Color.red;

			return false;
		}

//		currentWeightText.color = Color.white;

		return true;

	}
	public void UpdateDisplay (InventoryActionType inventoryActionType) {
		UpdateDisplay ();
	}
	public void UpdateDisplay () {
//		currentWeightText.text = "" + CurrentWeight;
		weightImage.fillAmount = ((float)CurrentWeight / (float)CurrentCapacity);
	}
	#endregion

	#region feedback
	bool wasActive = false;
	public void DisplayFeedback () {
		
		displayingFeedback = true;
		timer = 0f;

		Bounce ();

		wasActive = Visible;

		Show ();
	}

	public void HideFeedback () {

		Visible = wasActive;

//		currentWeightText.color = Color.white;

		displayingFeedback = false;
	}
	#endregion

	#region properties
	public int CurrentWeight {
		get {
			return LootManager.Instance.getLoot (Crews.Side.Player).weight;
		}
	}

	public int CurrentCapacity {
		get {
			return currentCapacity;
		}
		set {
			currentCapacity = value;
		}
	}

	public void Show () {
		Visible = true;
	}
	public void Hide () {
		return;
		Visible = false;
	}

	public bool Visible {
		get {
			return weightGroup.activeSelf;
		}
		set {
			weightGroup.SetActive (value);
		}
	}
	#endregion
}
