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

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.NewQuest:
			HandleQuest ();
			break;
		case FunctionType.CheckQuest:
			CheckQuest ();
			break;
		case FunctionType.SendPlayerBackToGiver:
			SetCoordsToGiver ();
			break;
		case FunctionType.SetQuestOnMap:
			SetQuestOnMap ();
			StoryReader.Instance.WaitForInput ();
			break;
		case FunctionType.FinishQuest:
			FinishQuest ();
			break;
		}
	}

	public void HandleQuest () {

		Quest quest = finishedQuests.Find ( x => x.originCoords == NavigationManager.CurrentCoords );

		// check if quest is already finished
		if ( quest != null ) {
			string phrase = "Merci beaucoup de m'avoir aidé !";
			DialogueManager.Instance.SetDialogueTimed (phrase, Crews.enemyCrew.captain);
			Invoke ("GoToFallbackNode", 0.5f);
			return;
		}

		// check if player already has quest
		quest = CurrentQuests.Find ( x => x.originCoords == NavigationManager.CurrentCoords);
		if (quest != null) {
			ReturnToGiver ();
		} else {
			CreateNewQuest ();
		}

	}

	private void CreateNewQuest () {
		
		Quest newQuest = new Quest ();

		if (newQuestEvent != null)
			newQuestEvent ();

		// create quest
		newQuest.originCoords = NavigationManager.CurrentCoords;
		newQuest.targetCoords = NavigationManager.CurrentCoords;
		newQuest.questID = StoryLoader.Instance.getStoryIndexFromPercentage (StoryType.Quest);
		currentQuests.Add (newQuest);

		// set quest value
		newQuest.goldValue = Random.Range(10,50);

		// update diary
		QuestMenu.Instance.InitButtons();

		// get target node
		Node targetNode = StoryReader.Instance.GetNodeFromText(newQuest.Story, "debut");

		// get fallback node
		string s = StoryFunctions.Instance.CellParams;


		if (s.Contains("]["))  {

			int nodeIndex = s.IndexOf ("]");

			string nodeWhenCompleted = s.Remove (1,nodeIndex+2);
			newQuest.targetNodeWhenCompleted = StoryReader.Instance.GetNodeFromText(nodeWhenCompleted);

			string fallbackNodeText = s.Remove (nodeIndex);

			Debug.LogError ("fall back node A PRIORI YA UN PROBLEME ICI: " + fallbackNodeText);

			Node fallbackNode = StoryReader.Instance.GetNodeFromText ( fallbackNodeText );

			StoryReader.Instance.SetNewStory (newQuest.Story, StoryType.Quest, targetNode, fallbackNode);


		} else {

			string nodeText = s;

			nodeText = nodeText.Remove( 0,1 );
			nodeText = nodeText.Remove( nodeText.IndexOf("]") );

			Debug.LogError ("fall back node : " + nodeText);
			Node fallbackNode = StoryReader.Instance.GetNodeFromText ( nodeText );
			StoryReader.Instance.SetNewStory (newQuest.Story, StoryType.Quest, targetNode, fallbackNode);

		}

	}

	void ReturnToGiver ( )
	{
		Quest quest = CurrentQuest_Origin;

		Node targetNode = StoryReader.Instance.GetNodeFromText(quest.Story, "fin");

		// get fallback node
		string s = StoryFunctions.Instance.CellParams;

		s = s.Trim (new char [3] { '\r','\t','\n' } );
		s = s.Remove (0, 1);
		s = s.Remove (s.Length - 1);
		string nodeText = s;

		Node fallbackNode = StoryReader.Instance.GetNodeFromText ( nodeText );

		StoryReader.Instance.SetNewStory (quest.Story, StoryType.Quest, targetNode, fallbackNode);
	}

	public void CheckQuest () {

		Quest quest = CurrentQuests.Find (x => x.targetCoords == NavigationManager.CurrentCoords);
		if ( quest != null ) {
			ContinueQuest (quest);
			return;
		}
			
		StoryReader.Instance.NextCell();
		StoryReader.Instance.UpdateStory ();
	}

	void ContinueQuest (Quest quest)
	{
		// get target node
		Node targetNode = StoryReader.Instance.GetNodeFromText(quest.Story, "suite");

		// get fallback node
		string s = StoryFunctions.Instance.CellParams;

		s = s.Remove (0, 1);
		s = s.Remove (s.Length - 1);
		string nodeText = s;

		Node fallbackNode = StoryReader.Instance.GetNodeFromText ( nodeText );

		StoryReader.Instance.SetNewStory (quest.Story, StoryType.Quest, targetNode, fallbackNode);
	}

	public void FinishQuest ()
	{
		Quest quest = CurrentQuests.Find (x => x.targetCoords == NavigationManager.CurrentCoords);
		if ( quest == null )
			quest = CurrentQuests.Find (x => x.originCoords == NavigationManager.CurrentCoords);
		if (quest == null)
			print ("arret...");
		
		GoldManager.Instance.GoldAmount += quest.goldValue;

		Karma.Instance.CurrentKarma += 2;

		finishedQuests.Add (quest);
		currentQuests.Remove (quest);

		if ( quest.targetNodeWhenCompleted != null ) {
			StoryReader.Instance.CurrentStoryHandler.fallbackNode = quest.targetNodeWhenCompleted;
		}

		StoryReader.Instance.NextCell();
		StoryReader.Instance.UpdateStory ();
	}

	private void GoToFallbackNode () {
		string s = StoryFunctions.Instance.CellParams;

		s = s.Remove (0, 1);
		s = s.Remove (s.Length - 1);
		string nodeText = s;

		Node fallbackNode = StoryReader.Instance.GetNodeFromText (nodeText);
		StoryReader.Instance.GoToNode (fallbackNode);
	}

	#region map stuff
	public void SetQuestOnMap () {

		Quest quest = CurrentQuest_Target;

		Coords c = GetClosestIslandCoords ();
		Coords boatCoords = NavigationManager.CurrentCoords;
		int distToQuest = (int)Vector2.Distance ( new Vector2(c.x,c.y) , new Vector2 (boatCoords.x , boatCoords.y) );

		// show on map
		MapGenerator.Instance.GetChunk (c).State = ChunkState.DiscoveredIsland;
		quest.targetCoords = c;
		quest.ShowOnMap ();
		quest.goldValue += (10 * distToQuest);
	}

	public void SetCoordsToGiver () {
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
	private Coords GetClosestIslandCoords () {

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

	public Quest CurrentQuest_Target {
		get {
			return CurrentQuests.Find ( x=> x.targetCoords == NavigationManager.CurrentCoords);
		}
	}

	public Quest CurrentQuest_Origin {
		get {
			return CurrentQuests.Find ( x=> x.originCoords == NavigationManager.CurrentCoords);
		}
	}
}
