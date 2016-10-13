using UnityEngine;
using System.Collections;

public class StoryFunctions : MonoBehaviour {

	public static StoryFunctions Instance;

	string cellParams = "";

	private string[] functionNames = new string [15] {
		"randomRange",
		"PlayerSpeak",
		"OtherSpeak",
		"randomAppear",
		"setChoices",
		"launchCombat",
		"randomCrew",
		"removeGold",
		"addGold",
		"none",
		"none",
		"none",
		"none",
		"none",
		"end"
	};

	void Awake () {
		Instance = this;
	}

	public void Read ( string content ) {

		foreach ( string functionName in functionNames ) {
			
			if ( content.Contains (functionName) ){

				cellParams = content.Remove (0, functionName.Length);

				SendMessage (functionName);
				return;

			}

		}

		Debug.LogError ("cell returns no function : " + content);

	}

	#region functions
	void randomRange () {

		int range = int.Parse (cellParams);
		int random = Random.Range (0, range);

		StoryReader.Instance.NextCell ();

		StoryReader.Instance.SetDecal (random );

		StoryReader.Instance.UpdateStory ();


	}
	void setChoices () {

			// get amount
		int amount = int.Parse (cellParams);

			// get bubble content
		StoryReader.Instance.NextCell ();

		string[] choices = new string[amount];

		int tmpDecal = StoryReader.Instance.Decal;
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

		StoryReader.Instance.Wait ( Crews.playerCrew.captain.Icon.MoveDuration );
	}

	void randomCrew () {
		Crews.enemyCrew.CreateRandomCrew ();
		Crews.enemyCrew.UpdateCrew (Crews.PlacingType.Combat);

		Crews.enemyCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);

		StoryReader.Instance.Wait ( Crews.playerCrew.captain.Icon.MoveDuration );

	}

	void OtherSpeak () {

		string phrase = cellParams.Remove (0,2);

		DialogueManager.Instance.SetDialogue (phrase, Crews.enemyCrew.captain.Icon.GetTransform);

		StoryReader.Instance.WaitForInput ();

	}

	void PlayerSpeak () {
		
		string phrase = cellParams.Remove (0,2);

		DialogueManager.Instance.SetDialogue (phrase, Crews.playerCrew.captain.Icon.GetTransform);

		StoryReader.Instance.WaitForInput ();
	}

	void launchCombat () {
		CombatManager.Instance.StartCombat ();
	}

	void end () {
		IslandManager.Instance.Leave ();
	}
	#endregion

	#region gold
	void removeGold () {
		int amount = int.Parse (cellParams);
		GoldManager.Instance.RemoveGold (amount);
		StoryReader.Instance.Wait ( 1f );
	}
	void addGold () {
		int amount = int.Parse (cellParams);
		GoldManager.Instance.AddGold (amount);
		StoryReader.Instance.Wait ( 1f );
	}
	#endregion
}
