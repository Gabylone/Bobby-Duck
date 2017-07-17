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

	private Coords GetClosestIslandCoords () {

		int radius = 1;

		while ( radius < MapGenerator.Instance.MapScale ) {
			
			for (int x = -radius; x < radius; x++) {
				for (int y = -radius; y < radius; y++) {

					if (x == 0 && y == 0)
						continue;

					int posX = Boats.Instance.PlayerBoatInfo.PosX + x;
					int posY = Boats.Instance.PlayerBoatInfo.PosY + y;

					if (posX < 0 || posX >= MapGenerator.Instance.MapScale)
						continue;

					if (posY < 0 || posY >= MapGenerator.Instance.MapScale)
						continue;
					
					Chunk chunk = MapGenerator.Instance.Chunks [posX, posY];

					if (chunk.IslandData != null) {
						return new Coords (posX,posY);
					}

				}
			}

			++radius;
		}

		Debug.Log ("could not find closest island");

		return new Coords (-1,-1);

	}

	public void NewQuest () {

		Quest quest = finishedQuests.Find ( x => x.originCoords == NavigationManager.CurrentCoords );

		if ( quest != null ) {
			// get fallback node
			string phrase = "Merci beaucoup de m'avoir aidé !";
			DialogueManager.Instance.SetDialogueTimed (phrase, Crews.enemyCrew.captain);
			Invoke ("GoToFallbackNode", 1f);
			return;
		}

		quest = currentQuests.Find ( x => x.originCoords == NavigationManager.CurrentCoords );

		// check if player already has quest
		if (quest != null) {
			print ("la suite de la");
			ReturnToGiver (quest);
		} else {
			print ("aucune quete comme ça, créer une nouvelle");
			CreateNewQuest ();
		}

	}

	private void CreateNewQuest () {
		
		Quest newQuest = new Quest ();

		// get island position
		Coords c = GetClosestIslandCoords ();
		Coords boatCoords = NavigationManager.CurrentCoords;
		int distToQuest = (int)Vector2.Distance ( new Vector2(c.x,c.y) , new Vector2 (boatCoords.x , boatCoords.y) );

		// create quest
		newQuest.originCoords = NavigationManager.CurrentCoords;
		newQuest.targetCoords = c;
		newQuest.questID = StoryLoader.Instance.getStoryIndexFromPercentage (StoryType.Quest);
		currentQuests.Add (newQuest);

		// set quest value
		//		print ("lvl : "  );
		print ("distance to quest : " + distToQuest);
		newQuest.goldValue = distToQuest;

		// show on map
		MapGenerator.Instance.Chunks [c.x, c.y].State = ChunkState.DiscoveredIsland;
		newQuest.ShowOnMap ();


		// update diary
		QuestMenu.Instance.InitButtons();

		// get target node
		Node targetNode = StoryReader.Instance.GetNodeFromText(newQuest.Story, "debut");

		// get fallback node
		string s = StoryFunctions.Instance.CellParams;

		s = s.Remove (0, 1);
		s = s.Remove (s.Length - 1);
		string nodeText = s;

		Node fallbackNode = StoryReader.Instance.GetNodeFromText ( nodeText );

		StoryReader.Instance.SetNewStory (newQuest.Story, StoryType.Quest, targetNode, fallbackNode);
	}

	public void SetCoordsToGiver () {
		Quest quest = CurrentQuests.Find (x => x.targetCoords == NavigationManager.CurrentCoords);

		if ( quest == null ) {
			Debug.LogError ("il est sensé avoir une quete là non ?");
			return;
		}

		quest.questState = Quest.QuestState.Returning;
		quest.targetCoords = quest.originCoords;

		quest.ShowOnMap ();

		StoryReader.Instance.NextCell();
		StoryReader.Instance.UpdateStory ();
	}

	void ReturnToGiver ( Quest quest )
	{
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

		print ("no quests where found par là");

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

		GoldManager.Instance.GoldAmount += quest.goldValue;

		Karma.Instance.CurrentKarma += 2;

		finishedQuests.Add (quest);
		currentQuests.Remove (quest);

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

	public List<Quest> CurrentQuests {
		get {
			return currentQuests;
		}
	}

	public List<Quest> FinishedQuests {
		get {
			return finishedQuests;
		}
	}
}
