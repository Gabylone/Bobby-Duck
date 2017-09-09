using UnityEngine;
using System.Collections;

public enum FunctionType {

	Leave,
	ChangeStory,
	Fade,
	CheckFirstVisit,
	RandomPercent,
	RandomRange,
	RandomRedoPercent,
	RandomRedoRange,
	NewCrew,
	ShowPlayer,
	ShowOther,
	HidePlayer,
	HideOther,
	AddMember,
	RemoveMember,
	Narrator,
	SetChoices,
	PlayerSpeak,
	OtherSpeak,
	GiveTip,
	CheckGold,
	RemoveGold,
	AddGold,
	AddToInventory,
	RemoveFromInventory,
	CheckInInventory,
	Loot,
	Trade,
	BoatUpgrades,
	LaunchCombat,
	CheckClues,
	SetWeather,
	ChangeTimeOfDay,
	CheckDay,
	Node,
	Switch,
	CheckStat,
	AddHealth,
	RemoveHealth,
	AddKarma,
	RemoveKarma,
	CheckKarma,
	PayBounty,
	NewQuest,
	CheckQuest,
	SendPlayerBackToGiver,
	FinishQuest,
	ShowQuestOnMap,

}

public class StoryFunctions : MonoBehaviour {

	public static StoryFunctions Instance;

	string cellParams = "";

	float waitDuration = 0.35f;

	public delegate void GetFunction (FunctionType func, string cellParameters );
	public GetFunction getFunction;

	public string CellParams {
		get {
			return cellParams;
		}
	}

	void Awake () {
		Instance = this;
	}

	public void Read ( string content ) {

		if (content.Length == 0) {
			
			string text = "cell is empty on story " + StoryReader.Instance.CurrentStoryHandler.Story.name + "" +
				"\n at row : " + (StoryReader.Instance.Index+2) + "" +
				"\n and collumn : " + StoryReader.Instance.Decal;

			Debug.LogError (text);

			StoryLauncher.Instance.EndStory ();
			return;
		}
	
		if ( content[0] == '[' ) {

			string nodeName = content.Remove (0, 1);
			nodeName = nodeName.Remove (nodeName.Length-1);

			Node node = StoryReader.Instance.GetNodeFromText (nodeName);

			StoryReader.Instance.NextCell ();

			if (node.decal > 0)
				StoryReader.Instance.SetDecal (node.decal);

			StoryReader.Instance.UpdateStory ();
			return;
		}

		foreach ( FunctionType func in System.Enum.GetValues(typeof(FunctionType)) ) {

			if ( content.Contains (func.ToString()) ){

				cellParams = content.Remove (0, func.ToString().Length);

				if (getFunction != null)
					getFunction (func,cellParams);

				return;
			}

		}

		Debug.LogError (
			"cell returns no function at decal\n" + StoryReader.Instance.Decal + "\n" +
			"index : " + StoryReader.Instance.Index + "\n" +
			"qui contient : " + content);

		StoryLauncher.Instance.EndStory ();

	}


}
