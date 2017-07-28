using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour {

	public static QuestManager Instance;

	private List<Quest> currentQuests = new List<Quest>();
	private List<Quest> finishedQuests = new List<Quest>();

	void Awake () {
		Instance = this;
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

		s = s.Remove (0, 1);
		s = s.Remove (s.Length - 1);

		if (s.Contains("]["))  {

			int nodeIndex = s.IndexOf ("]");

			string compNode = s.Remove (0,nodeIndex+2);
			newQuest.targetNodeWhenCompleted = StoryReader.Instance.GetNodeFromText(compNode);

			string fallbackNodeText = s.Remove (nodeIndex);
			Node fallbackNode = StoryReader.Instance.GetNodeFromText ( fallbackNodeText );
			StoryReader.Instance.SetNewStory (newQuest.Story, StoryType.Quest, targetNode, fallbackNode);

		} else {

			string nodeText = s;

			Node fallbackNode = StoryReader.Instance.GetNodeFromText ( nodeText );
			StoryReader.Instance.SetNewStory (newQuest.Story, StoryType.Quest, targetNode, fallbackNode);

		}

	}

	void ReturnToGiver ( )
	{
		Quest quest = CurrentQuest;

		Node targetNode = StoryReader.Instance.GetNodeFromText(quest.Story, "fin");

		// get fallback node
		string s = StoryFunctions.Instance.CellParams;

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

		Quest quest = CurrentQuest;

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

	public Quest CurrentQuest {
		get {
			return CurrentQuests.Find ( x=> x.targetCoords == NavigationManager.CurrentCoords);
		}
	}
}
