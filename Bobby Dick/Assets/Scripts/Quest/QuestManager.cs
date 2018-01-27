using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour {

	public static QuestManager Instance;

	private List<Quest> currentQuests = new List<Quest>();
	private List<Quest> finishedQuests = new List<Quest>();

	public delegate void NewQuestEvent ();
	public NewQuestEvent newQuestEvent;

	public int maxQuestAmount = 20;

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
		case FunctionType.AccomplishQuest:
			AccomplishQuest ();
			break;
		case FunctionType.IsQuestAccomplished:
			CheckIfQuestIsAccomplished ();
			break;
		case FunctionType.GiveUpQuest:
			GiveUpActiveQuest ();
			break;
		}
	}

	void CheckIfQuestIsAccomplished ()
	{
		if ( Quest.currentQuest == null ) {
			Debug.LogError ("QUEST IS NULL : CheckIfQuestIsAccomplished");
			return;
		}

		int decal = Quest.currentQuest.accomplished ? 0 : 1;

		StoryReader.Instance.NextCell ();

		StoryReader.Instance.SetDecal (decal);

		StoryReader.Instance.UpdateStory ();
	}

	void AccomplishQuest ()
	{
		if ( Quest.currentQuest == null ) {
			Debug.LogError ("QUEST IS NULL : accomplish quest");
			return;
		}

		Quest.currentQuest.accomplished = true;

		Quest.currentQuest.targetCoords = Quest.currentQuest.originCoords;

		Quest.currentQuest.ShowOnMap ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();

	}
	#endregion

	#region new quest
	void HandleNewQuest () {



		// CHECK FINISHED QUESTS
		Quest quest = Coords_CheckForFinishedQuest;

		if ( quest != null ) {
			HandleCompletedQuest (quest);
			return;
		}

		// CHECK CURRENT QUESTS
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

		if (currentQuests.Count == maxQuestAmount) {
			Invoke ("FallBackDelay",1f);
		} else {
			newQuest.Init ();
			currentQuests.Add (newQuest);
		}

		if (newQuestEvent != null)
			newQuestEvent ();
	}
	void FallBackDelay () {
		Quest newQuest = new Quest ();
		newQuest.GetNewQuestnode ();
		StoryReader.Instance.GoToNode (newQuest.newQuest_FallbackNode);
	}
	#endregion

	#region completed quest
	void HandleCompletedQuest (Quest quest )
	{
		string phrase = "Merci beaucoup de m'avoir aidé !";
		DialogueManager.Instance.SetDialogueTimed (phrase, Crews.enemyCrew.captain);

		Invoke ("HandleCompletedQuest_Delay", DialogueManager.Instance.DisplayTime);
	}
	void HandleCompletedQuest_Delay () {
		StoryReader.Instance.GoToNode (Coords_CheckForFinishedQuest.newQuest_FallbackNode);
	}
	#endregion

	void ContinueQuest () {

		Quest quest = CurrentQuests.Find (x => x.targetCoords == Boats.playerBoatInfo.coords);

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

		Karma.Instance.AddKarma ();

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

	void GiveUpActiveQuest ()
	{
		GiveUpQuest (Quest.currentQuest);

		StoryReader.Instance.NextCell();
		StoryReader.Instance.UpdateStory ();
	}

	#region map stuff
	public void ShowQuestOnMap () {
		Quest.currentQuest.ShowOnMap ();
	}

	public void SendPlayerBackToGiver () {
		Debug.LogError ("SEND PLAYER BACK TO GIVER DOESNT EXIST ANYMORE");
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
			return CurrentQuests.Find ( x=> x.targetCoords == Boats.playerBoatInfo.coords);
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
				x.originCoords == Boats.playerBoatInfo.coords && 
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
				x.originCoords == Boats.playerBoatInfo.coords && 
				storyLayer == x.layer &&
				x.row == StoryReader.Instance.Col &&
				x.col == StoryReader.Instance.Row
			);
		}
	}
}
