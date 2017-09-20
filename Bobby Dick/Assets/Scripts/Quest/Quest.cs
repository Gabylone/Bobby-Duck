using UnityEngine;

[System.Serializable]
public class Quest {

	public enum QuestState {
		Started,
		Returning,
		Finished
	}

	public int questID = 0;

	public int goldValue = 0;

	public int experience = 0;

	public int level = 0;

	public Coords originCoords;
	public Coords targetCoords;

	public Node nodeWhenCompleted;
	public Node newQuest_FallbackNode;
	public Node checkQuest_FallbackNode;

	public MemberID giver;

	public Quest () {
		//

	}
	public void Init ()
	{
		goldValue = Random.Range(10,50);

		originCoords = Boats.PlayerBoatInfo.CurrentCoords;

		SetRandomCoords ();

		questID = StoryLoader.Instance.getStoryIndexFromPercentage (IslandType.Quest);

		GetNewQuestnode ();

		Node targetNode = Story.GetNode ("debut");

		StoryReader.Instance.SetNewStory (Story, IslandType.Quest, targetNode, newQuest_FallbackNode);

	}

	public void ReturnToGiver() {

		StoryReader.Instance.SetNewStory (Story, IslandType.Quest, Story.GetNode("fin") , newQuest_FallbackNode);
		//
	}

	public void Continue ()
	{

		string nodeText = StoryFunctions.Instance.CellParams;

		nodeText = nodeText.Remove (0, 2);

		checkQuest_FallbackNode = StoryReader.Instance.GetNodeFromText ( nodeText );

		StoryReader.Instance.SetNewStory (Story, IslandType.Quest, Story.GetNode("suite"), checkQuest_FallbackNode);

	}

	#region map & coords
	public void ShowOnMap ()
	{
		DisplayMap.Instance.OpenMap ();
		DisplayMap.Instance.CenterOnCoords (targetCoords);
		DisplayMap.Instance.HighlightPixel (targetCoords);
	}

	public void SetRandomCoords () {

		targetCoords = QuestManager.Instance.GetClosestIslandCoords ();

		Coords boatCoords = Boats.PlayerBoatInfo.CurrentCoords;
		int distToQuest = (int)Vector2.Distance ( new Vector2(targetCoords.x,targetCoords.y) , new Vector2 (boatCoords.x , boatCoords.y) );

		// show on map
		Chunk.GetChunk (targetCoords).State = ChunkState.DiscoveredIsland;
//		ShowOnMap ();
		goldValue += (10 * distToQuest);
	}
	#endregion

	#region nodes
	public void GetNewQuestnode () {

		string s = StoryFunctions.Instance.CellParams;

		s = s.Remove (0,2);

		string[] parts = s.Split (',');

		newQuest_FallbackNode = StoryReader.Instance.GetNodeFromText ( parts[0] );

		if ( parts.Length > 1 ) {
			nodeWhenCompleted = StoryReader.Instance.GetNodeFromText (parts [1]);
		}	
	}
	#endregion


	public Story Story {
		get {
			return StoryLoader.Instance.Quests [questID];
		}
	}

	//
}