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

	void Awake () {
		Instance = this;
	}

	void Start () {
		AddGold (startValue);
	}

	void Update () {
		if ( feedbackActive ) {
			timer += Time.deltaTime;

			if ( timer > feedbackDuration ) {
				HideFeedback ();
			}
		}
	}
	
	public void AddGold ( int i ) {
		GoldAmount +=i;
		UpdateUI ();
	}

	public void RemoveGold ( int i ) {
		GoldAmount -= i; 
		UpdateUI ();
	}

	public bool CheckGold ( float amount ) {
		if ( amount > GoldAmount ) {
			SoundManager.Instance.PlaySound (noGoldSound);
			DisplayFeedback ();
			return false;
		}

		return true;
	}

	private void DisplayFeedback () {

		feedbackActive = true;
		timer = 0f;

		goldImage.color = Color.red;
		goldText.color = Color.red;

	}
	private void HideFeedback () {
		feedbackActive = false;

		goldImage.color = Color.white;
		goldText.color = Color.yellow;
	}

	public void UpdateUI () {
		goldText.text = goldAmount.ToString ();
	}

	public int GoldAmount {
		get {
			return goldAmount;
		}
		set {
			goldAmount = value;
		}
	}
}
