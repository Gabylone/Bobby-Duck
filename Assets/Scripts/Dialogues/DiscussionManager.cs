using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DiscussionManager : MonoBehaviour {

	public static DiscussionManager Instance;

	[Header("Choices")]
	[SerializeField]
	private GameObject[] choiceButtons;

	[SerializeField]
	private Color[] statColor;

	[Header("Tips")]
	[SerializeField]
	private string[] tips = new string[10] {

		"Un grand vide sépare le nord du sud",
		"Mieux vaut bien se préparer pour aller du nord au sud !",
		"Les pirates se déplacent librement sur les mers",
		"Une bonne longue vue règle les problemes de vision la nuit",
		"Une bonne longue vue règle les problemes de vision les jours de pluie",
		"C'est en discutant avec les gens que vous saurez où chercher le trésor.",
		"le charme du capitaine est important, il permet de se sortir de situations coquasses",
		"La dextérité détermine si un membre attaque en premier, et ses chances d'esquiver.",
		"Vous êtes à l'étroit sur votre navire ? Aggrandissez le pont dans un hangar",
		"Aggrandissez le cargo dans un hangar.Vous pouvez porter plus de choses."

	};

	void Awake () {
		Instance = this;
	}

	public void SetChoices (int amount, string[] content) {

		for (int i = 0; i < amount ; ++i ) {
			choiceButtons[i].SetActive (true);
			choiceButtons [i].GetComponentInChildren<Text> ().text = content [i];
		}
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
		DiscussionManager.Instance.ResetColors ();

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

						DiscussionManager.Instance.TaintChoice (index, i);
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

		DiscussionManager.Instance.SetChoices (amount, choices);
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
