using UnityEngine;
using System.Collections;

public class StoryFunctions : MonoBehaviour {

	public static StoryFunctions Instance;

	string cellParams = "";

	private string[] functionNames = new string [5] {
		"randomRange",
		"playerSpeak",
		"randomAppear",
		"setChoices",
		"otherSpeak"
	};

	void Awake () {
		Instance = this;
	}

	public void Read ( string content ) {

		foreach ( string functionName in functionNames ) {
			
			if ( content.Contains (functionName) ){
				cellParams = content.Remove (0, functionName.Length);

				SendMessage (functionName);
				break;
			}

		}

	}

	#region functions
	void randomRange () {

		int range = int.Parse (cellParams);
		int random = Random.Range (0, range);

		StoryReader.Instance.NextCell ();

		StoryReader.Instance.SetDecal (random );

	}
	void setChoices () {
			// get amount
		int amount = int.Parse (cellParams);

			// get bubble content
		StoryReader.Instance.NextCell ();

		string[] choices = new string[amount];

		int tmpDecal = 0;
		int a = amount;

		while ( a > 0 ) {

			if ( StoryLoader.Instance.ReadDecal (tmpDecal).Length > 0 ) {
				choices [amount-a] = StoryLoader.Instance.ReadDecal (tmpDecal);

				--a;
			}

			++tmpDecal;

			if ( tmpDecal > 20 ) {
				Debug.LogError ("reached while limit");
				break;
			}

			if (a <= 0)
				break;
		}

		DiscussionManager.Instance.SetChoices (amount, choices);

	}

	void randomAppear () {
		Crews.enemyCrew.CreateRandomMember ();

		StoryReader.Instance.Wait (0.5f);
	}

	void otherSpeak () {

		string phrase = cellParams.Remove (0,1);

		DialogueManager.Instance.SetDialogue (phrase, Crews.enemyCrew.captain.Icon.GetTransform);

		StoryReader.Instance.WaitForInput ();

	}

	void playerSpeak () {
		
		string phrase = cellParams.Remove (0,1);

		DialogueManager.Instance.SetDialogue (phrase, Crews.playerCrew.captain.Icon.GetTransform);

		StoryReader.Instance.WaitForInput ();
	}
	#endregion
}
