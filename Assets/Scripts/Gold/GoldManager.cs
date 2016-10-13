using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoldManager : MonoBehaviour {

	public static GoldManager Instance;

	[Header ("UI Elements")]
	[SerializeField] private GameObject goldGroup;
	[SerializeField] private Text goldText;
	[SerializeField] private Image goldImage;

	[Header ("Amounts")]
	[SerializeField]
	private int startValue = 200;
	private int goldAmount = 0;

	void Awake () {
		Instance = this;
	}

	void Start () {
		AddGold (startValue);
	}
	
	public void AddGold ( int i ) {
		GoldAmount +=i;
		UpdateUI ();
	}

	public void RemoveGold ( int i ) {
		GoldAmount -= i; 
		UpdateUI ();
	}

	public void UpdateUI () {
		goldText.text = goldAmount.ToString ();
	}

	public int GoldAmount {
		get {
			return goldAmount;
		}
		set {
			goldAmount = Mathf.Clamp (value, 0, value);
		}
	}
}
