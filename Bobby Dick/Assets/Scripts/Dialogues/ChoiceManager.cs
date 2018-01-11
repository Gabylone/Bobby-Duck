using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChoiceManager : MonoBehaviour {

	public static ChoiceManager Instance;

	public static Sprite[] feedbackSprites;
	public Sprite[] bubbleSprites;

	public static string[] bubblePhrases = new string[8] {
		"(partir)",
		"(attaquer)",
		"(trade)",
		"(autre)",
		"(dormir)",
		"(nouveau membre)",
		"(loot)",
		"(quete)"
	};

	[Header("Choices")]
	[SerializeField]
	private GameObject[] choiceButtons;

	[SerializeField]
	private Color[] statColor;

	[Header("Tips")]
	[SerializeField]
	private string[] tips;

	[SerializeField]
	private GameObject choiceGroup;

	void Awake () {
		Instance = this;
	}

	void Start () {

		feedbackSprites = Resources.LoadAll<Sprite> ("Graph/ChoiceBubbleFeedbackSprites");

		StoryFunctions.Instance.getFunction+= HandleGetFunction;

		CrewInventory.Instance.openInventory += HandleOpenInventory;
		CrewInventory.Instance.closeInventory += HandleCloseInventory;
	}

	bool previousActive = false;
	void HandleOpenInventory (CrewMember member)
	{
		if (choiceGroup.activeSelf) {

			choiceGroup.SetActive (false);

			previousActive = true;
		}
	}

	void HandleCloseInventory ()
	{
		if ( previousActive ) {

			choiceGroup.SetActive (true);

			previousActive = false;	
		}
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.SetChoices:
			GetChoices ();
			break;
		case FunctionType.GiveTip:
			GiveTip ();
			break;
		}
	}

	public void SetChoices (int amount, string[] content) {

		for (int i = 0; i < amount ; ++i ) {
			choiceButtons [i].SetActive (true);

//			string str = content [i];
			string str = FitText(content [i]);
			str = choiceButtons [i].GetComponentInChildren<ChoiceBubbleFeedback> ().SetSprite(str);

			if (content [i].StartsWith ("(")) {
				choiceButtons [i].GetComponent<Image> ().sprite = bubbleSprites [0];
			} else {
				choiceButtons [i].GetComponent<Image> ().sprite = bubbleSprites[1];

			}

			choiceButtons [i].GetComponentInChildren<Text> ().text = str;

			Tween.Bounce ( choiceButtons[i].transform , 0.2f , 1.1f );
		}

		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);

	}

	public void ResetColors () {
		foreach ( GameObject buttonObj in choiceButtons )
			buttonObj.GetComponentInChildren<Image> ().color = Color.white;
	}

	public void TaintChoice (int buttonIndex , int statIndex) {

		choiceButtons [buttonIndex].GetComponentInChildren<Image> ().color = statColor [statIndex];

	}

	public void Choose (int i) {

		StoryReader.Instance.SetDecal (i);

		foreach ( GameObject button in choiceButtons ) {
			button.SetActive (false);
		}

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}

	#region dialogues choices
	public void GetChoices () {
		ChoiceManager.Instance.ResetColors ();

		// get amount
		int amount = int.Parse (StoryFunctions.Instance.CellParams);

		// get bubble content
		StoryReader.Instance.NextCell ();

		string[] choices = new string[amount];

		int tmpDecal = StoryReader.Instance.Row;
		int a = amount;

		int index = 0;
		while ( a > 0 ) {

			if ( StoryReader.Instance.ReadDecal (tmpDecal).Length > 0 ) {

				string choice = StoryReader.Instance.ReadDecal (tmpDecal);

				choice = choice.Remove (0, 9);

				int i = 0;

				string[] stats = new string[] { "(str)", "(dex)", "(cha)", "(con)" };
				foreach ( string stat in stats ) {

					if ( choice.Contains ( stat ) ) {

						ChoiceManager.Instance.TaintChoice (index, i);
						choice = choice.Replace (stat, "");

					}

					++i;

				}

				choices [amount - a] = choice;

				--a;
				++index;
			}

			++tmpDecal;

			if ( tmpDecal > 60 ) {
				Debug.LogError ("set choice reached limit");
				break;
			}

			if (a <= 0)
				break;
		}

		SetChoices (amount, choices);
	}
	#endregion

	#region tips
	public void GiveTip () {
		DialogueManager.Instance.SetDialogue (tips[Random.Range (0,tips.Length)], Crews.enemyCrew.captain);
	}
	#endregion

	public GameObject[] ChoiceButtons {
		get {
			return choiceButtons;
		}
	}

	public int startIndex = 20;


	// DE LA MERDE
	string FitText (string str)
	{
		int currStartIndex = startIndex;

		if (currStartIndex >= str.Length)
			return str;

		int spaceIndex = str.IndexOf (" ", currStartIndex);

		while (spaceIndex >= startIndex) {

			str = str.Insert (spaceIndex, "\n");

			currStartIndex += startIndex;

			if (currStartIndex >= str.Length) {
				break;
			}

			spaceIndex = str.IndexOf (" ", currStartIndex);

			//			print (spaceIndex);

			if (startIndex >= 100) {
				break;
			}
		}

		return str;
	}
}
