using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChoiceManager : MonoBehaviour {

	public static ChoiceManager Instance;

	[Header("Choices")]
	[SerializeField]
	private GameObject[] choiceButtons;

	[SerializeField]
	private Color[] statColor;

	[Header("Tips")]
	[SerializeField]
	private string[] tips;

	void Awake () {
		Instance = this;
	}

	void Start () {
		StoryFunctions.Instance.getFunction+= HandleGetFunction;
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
			choiceButtons [i].GetComponentInChildren<Text> ().text = content [i];
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

		int tmpDecal = StoryReader.Instance.Decal;
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

		ChoiceManager.Instance.SetChoices (amount, choices);
	}
	#endregion

	#region tips
	public void GiveTip () {
		DialogueManager.Instance.SetDialogue (tips[Random.Range (0,tips.Length)], Crews.enemyCrew.captain);

		StoryReader.Instance.WaitForInput ();
	}
	#endregion

	public GameObject[] ChoiceButtons {
		get {
			return choiceButtons;
		}
	}
}
