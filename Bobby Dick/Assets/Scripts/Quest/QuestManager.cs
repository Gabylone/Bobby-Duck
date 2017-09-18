using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour {

	public static QuestManager Instance;

	private List<Quest> currentQuests = new List<Quest>();
	private List<Quest> finishedQuests = new List<Quest>();

	public delegate void NewQuestEvent ();
	public NewQuestEvent newQuestEvent;

	void Awake () {
		Instance = this;
	}

	void Start () {
		StoryFunctions.Instance.getFunction+= HandleGetFunction;
	}

	#region event
	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.NewQuest:
			HandleNewQuest ();
			break;
		case FunctionType.CheckQuest:
			ContinueQuest ();
			break;
		case FunctionType.SendPlayerBackToGiver:
			SendPlayerBackToGiver ();
			break;
		case FunctionType.ShowQuestOnMap:
			ShowQuestOnMap();
			break;
		case FunctionType.FinishQuest:
			FinishQuest ();
			break;
		}
	}
	#endregion

	#region new quest
	void HandleNewQuest () {

		Quest quest = Coords_CheckForFinishedQuest;

		// check if quest is already finished
		if ( quest != null ) {
			HandleCompletedQuest ();
			return;
		}

		// check if player already has quest
		quest = Coords_CheckForStartQuest;

		if (quest != null) {
			quest.ReturnToGiver ();
		} else {
			CreateNewQuest ();
		}
	}
	void CreateNewQuest () {

		// create quest
		Quest newQuest = new Quest ();

		currentQuests.Add (newQuest);

		newQuest.Init ();

		if (newQuestEvent != null)
			newQuestEvent ();
	}
	#endregion

	#region completed quest
	void HandleCompletedQuest ()
	{
		string phrase = "Merci beaucoup de m'avoir aidé !";
		DialogueManager.Instance.SetDialogueTimed (phrase, Crews.enemyCrew.captain);
		Invoke ("GoToFallbackNode", 0.5f);
	}
	private void GoToFallbackNode () {
		string s = StoryFunctions.Instance.CellParams;

		s = s.Remove (0, 1);
		s = s.Remove (s.Length - 1);
		string nodeText = s;

		Node fallbackNode = StoryReader.Instance.GetNodeFromText (nodeText);
		StoryReader.Instance.GoToNode (fallbackNode);
	}
	#endregion

	void ContinueQuest () {

		Quest quest = CurrentQuests.Find (x => x.targetCoords == NavigationManager.CurrentCoords);


		if ( quest != null) {

			bool targetIslandIsOriginIsland = quest.targetCoords == quest.originCoords;

			if (!targetIslandIsOriginIsland) {

				quest.Continue ();
				return;
			}
		}
			
		StoryReader.Instance.NextCell();
		StoryReader.Instance.UpdateStory ();
	}

	public delegate void OnFinishQuest (Quest quest);
	public static OnFinishQuest onFinishQuest;
	void FinishQuest ()
	{
		Quest quest = Coords_CheckForTargetQuest;

		if (quest == null)
			quest = Coords_CheckForStartQuest;
		if (quest == null) {
			print ("quest arr pourq uoi tu fai aàarret...");
			return;
		}
		
		GoldManager.Instance.GoldAmount += quest.goldValue;

		foreach ( CrewMember member in Crews.playerCrew.CrewMembers ) {
			member.AddXP (quest.experience);
		}

		Karma.Instance.CurrentKarma += 3;

		finishedQuests.Add (quest);
		currentQuests.Remove (quest);

		if (onFinishQuest != null) {
			onFinishQuest (quest);
		}

		if ( quest.nodeWhenCompleted != null ) {
			StoryReader.Instance.CurrentStoryHandler.fallbackNode = quest.nodeWhenCompleted;
		}

		StoryReader.Instance.NextCell();
		StoryReader.Instance.UpdateStory ();
	}

	public delegate void OnGiveUpQuest ( Quest quest );
	public static OnGiveUpQuest onGiveUpQuest;
	public void GiveUpQuest (Quest quest) {
		
		currentQuests.Remove (quest);

		if (onGiveUpQuest != null) {
			onGiveUpQuest(quest);
		}

	}

	#region map stuff
	public void ShowQuestOnMap () {
		Quest quest = Coords_CheckForStartQuest;

		if (quest == null) {
			Debug.Log ("SHOW QUEST ON MAP : coudn't find quest");
		} else {
			quest.ShowOnMap ();
		}
	}

	public void SendPlayerBackToGiver () {
		
		Quest quest = CurrentQuests.Find (x => x.targetCoords == NavigationManager.CurrentCoords);

		if ( quest == null ) {
			Debug.LogError ("il est sensé avoir une quete là non ?");
			return;
		}

		quest.targetCoords = quest.originCoords;

		quest.ShowOnMap ();

		StoryReader.Instance.NextCell();
		StoryReader.Instance.UpdateStory ();
	}
	public Coords GetClosestIslandCoords () {

		int radius = 1;

		while ( radius < MapGenerator.Instance.MapScale ) {

			for (int x = -radius; x < radius; x++) {
				for (int y = -radius; y < radius; y++) {

					if (x == 0 && y == 0)
						continue;

					Coords coords = new Coords (NavigationManager.CurrentCoords.x + x, NavigationManager.CurrentCoords.y + y);

					if (coords > MapGenerator.Instance.MapScale || coords <= 0) {
						continue;
					}

					Chunk chunk = MapGenerator.Instance.GetChunk (coords);

					if (chunk.IslandData != null) {
						return coords;
					}


				}
			}

			++radius;
		}

		Debug.Log ("could not find closest island");

		return new Coords (-1,-1);

	}
	#endregion

	public List<Quest> CurrentQuests {
		get {
			return currentQuests;
		}

		set {
			currentQuests = value;
		}
	}

	public List<Quest> FinishedQuests {
		get {
			return finishedQuests;
		}

		set {
			finishedQuests = value;
		}
	}

	public Quest Coords_CheckForTargetQuest {
		get {
			return CurrentQuests.Find ( x=> x.targetCoords == NavigationManager.CurrentCoords);
		}
	}

	Quest Coords_CheckForStartQuest {
		get {
			return CurrentQuests.Find ( x=> x.originCoords == NavigationManager.CurrentCoords);
		}
	}

	Quest Coords_CheckForFinishedQuest {
		get {
			return finishedQuests.Find ( x => x.originCoords == NavigationManager.CurrentCoords );
		}
	}
}
