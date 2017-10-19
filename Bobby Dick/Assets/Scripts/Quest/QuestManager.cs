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
			HandleCompletedQuest (quest);
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
	void HandleCompletedQuest (Quest quest )
	{
		string phrase = "Merci beaucoup de m'avoir aidé !";
		DialogueManager.Instance.SetDialogueTimed (phrase, Crews.enemyCrew.captain);

		StoryReader.Instance.GoToNode (quest.newQuest_FallbackNode);
	}
	#endregion

	void ContinueQuest () {

		Quest quest = CurrentQuests.Find (x => x.targetCoords == Boats.PlayerBoatInfo.coords);


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
		Quest quest = Quest.currentQuest;
		
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
		Quest.currentQuest.ShowOnMap ();
	}

	public void SendPlayerBackToGiver () {
		
		Quest quest = CurrentQuests.Find (x => x.targetCoords == Boats.PlayerBoatInfo.coords);

		if ( quest == null ) {
			Debug.LogError ("il est sensé avoir une quete là non ?");
			return;
		}

		quest.targetCoords = quest.originCoords;

		quest.ShowOnMap ();

		StoryReader.Instance.NextCell();
		StoryReader.Instance.UpdateStory ();
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
			return CurrentQuests.Find ( x=> x.targetCoords == Boats.PlayerBoatInfo.coords);
		}
	}

	Quest Coords_CheckForStartQuest {
		get {

			foreach (Quest quest in CurrentQuests) {
				print (quest.Story.name);
				print (quest.layer);
			}

			int storyLayer = StoryReader.Instance.currentStoryLayer;

			if (StoryReader.Instance.CurrentStoryHandler.storyType == StoryType.Quest ) {
				storyLayer = StoryReader.Instance.previousStoryLayer;
			}

//			return currentQuests.Find ( x=> x.originCoords == Boats.PlayerBoatInfo.coords);
			return currentQuests.Find ( x => 
				x.originCoords == Boats.PlayerBoatInfo.coords && 
				storyLayer == x.layer &&
				x.row == StoryReader.Instance.Col &&
				x.col == StoryReader.Instance.Row
			);
		}
	}

	Quest Coords_CheckForFinishedQuest {
		get {

			int storyLayer = StoryReader.Instance.currentStoryLayer;

			if (StoryReader.Instance.CurrentStoryHandler.storyType == StoryType.Quest ) {
				storyLayer = StoryReader.Instance.previousStoryLayer;
			}

			return finishedQuests.Find ( x => 
				x.originCoords == Boats.PlayerBoatInfo.coords && 
				storyLayer == x.layer &&
				x.row == StoryReader.Instance.Col &&
				x.col == StoryReader.Instance.Row
			);
//			return finishedQuests.Find ( x => x.originCoords == Boats.PlayerBoatInfo.coords );
		}
	}
}
