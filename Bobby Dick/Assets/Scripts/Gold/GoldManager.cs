using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

public class GoldManager : MonoBehaviour {

	public static GoldManager Instance;

	[Header ("UI Elements")]
	[SerializeField] private GameObject goldGroup;
	[SerializeField] private Text goldText;
	[SerializeField] private Image goldImage;

	[SerializeField]
	private float feedbackDuration = 1.5f;
	[SerializeField]
	private float feedbackScaleAmount = 1.5f;
	[SerializeField]
	private float feedbackBounceDuration = 0.3f;
	private bool feedbackActive = false;
	private float timer = 0f;

	[Header ("Amounts")]
	[SerializeField]
	private int startValue = 200;
	private int goldAmount = 0;

	[Header("Sound")]
	[SerializeField] private AudioClip noGoldSound;
	[SerializeField] private AudioClip buySound;

	void Awake () {
		Instance = this;

	}

	void Start () {

		StoryFunctions.Instance.getFunction += HandleGetFunction;
//		PlayerLoot.Instance.openInventory += Show;
//		PlayerLoot.Instance.closeInventory += Hide;

		CombatManager.Instance.onFightStart += Hide;
		CombatManager.Instance.onFightEnd+= Show;
	}

	public void InitGold ()
	{
		GoldAmount = startValue;
	}

	public void LoadGold ()
	{
		GoldAmount = SaveManager.Instance.CurrentData.playerGold;
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.CheckGold:
			SetGoldDecal ();
			break;
		case FunctionType.RemoveGold:
			RemoveGold (cellParameters);
			break;
		case FunctionType.AddGold:
			AddGold (cellParameters);
			break;
		}
	}

	#region timed feedback
	void Update () {
		
		if ( feedbackActive ) {
			
			timer += Time.deltaTime;

			if ( timer > feedbackDuration ) {
				HideFeedback ();
			}
		}
	}

	private void Bounce () {
		HOTween.To ( goldGroup.transform , feedbackBounceDuration , "localScale" , Vector3.one * feedbackScaleAmount, false , EaseType.EaseOutBounce , 0f);
		HOTween.To ( goldGroup.transform , feedbackBounceDuration , "localScale" , Vector3.one , false , EaseType.Linear , feedbackBounceDuration );
	}
	private void DisplayFeedback () {


		feedbackActive = true;
		timer = 0f;

		Bounce();
//		Show ();

	}
	private void HideFeedback () {
		feedbackActive = false;

		goldImage.color = Color.white;
		goldText.color = Color.white;

//		Hide ();
	}
	#endregion

	public void SetGoldDecal () {
		
		int amount = int.Parse (StoryFunctions.Instance.CellParams);

		if (GoldManager.Instance.CheckGold (amount)) {
			StoryReader.Instance.NextCell ();
		} else {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.SetDecal (1);
		}

		DisplayFeedback ();

		StoryReader.Instance.UpdateStory ();
	}

	public bool CheckGold ( float amount ) {

		Bounce();

		if ( amount > GoldAmount ) {

//			print ("amount : " + amount);
//			print ("gold : " + GoldAmount);

			goldImage.color = Color.red;
			goldText.color = Color.red;
			DisplayFeedback ();
//
			SoundManager.Instance.PlaySound (noGoldSound);
			return false;
		}

		SoundManager.Instance.PlaySound (buySound);

		goldImage.color = Color.white;
		goldText.color = Color.white;

		return true;
	}

	public void UpdateUI () {
		goldText.text = goldAmount.ToString ();
	}

	void RemoveGold(string cellParams) {
		
		int amount = int.Parse (cellParams);
		GoldAmount -= amount;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait ( 0.3f );

		DisplayFeedback ();

	}
	void AddGold(string cellParams) {
		
		int amount = int.Parse (cellParams);
		GoldAmount += amount;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (0.3f);

		DisplayFeedback ();

	}

	public int GoldAmount {
		get {
			return goldAmount;
		}
		set {
			goldAmount = Mathf.Clamp (value, 0 , value );
			UpdateUI ();

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
			return goldGroup.activeSelf;
		}
		set {
			goldGroup.SetActive (value);
		}
	}
}
