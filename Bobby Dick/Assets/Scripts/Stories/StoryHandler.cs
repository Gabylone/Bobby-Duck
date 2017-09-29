using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum StoryType {
	Normal,
	Treasure,
	Home,
	Clue,
	Boat,
	Quest
}

[System.Serializable]
public class StoryManager {

	public List<StoryHandler> storyHandlers = new List<StoryHandler>();

	public StoryManager () {
		
	}

	public void AddStoryHandler (StoryHandler handler ) {
		storyHandlers.Add (handler);
	}

	public void InitHandler ( StoryType storyType ) {
		
		int storyId = StoryLoader.Instance.getStoryIndexFromPercentage (storyType);

		StoryHandler handler = new StoryHandler (storyId,storyType);
		storyHandlers.Add (handler);
	}

	public StoryHandler CurrentStoryHandler {
		get {
			return storyHandlers [StoryReader.Instance.CurrentStoryLayer];
		}
	}
}

[System.Serializable]
public class StoryHandler {

	public int decal = 0;
	public int index = 0;

		// serialisation
	public int fallBackLayer = 0;

	public Node 				fallbackNode;

	public int 					storyID 			= 0;
	public StoryType 			storyType;
	public List<contentDecal> 	contentDecals 		= new List<contentDecal>();
	public List<Loot> 			loots 				= new List<Loot> ();
	public List<Crew> 			crews 				= new List<Crew>();
		//

	public StoryHandler () {
		//
	}

	public StoryHandler (int _storyID,StoryType _storyType) {
		storyID = _storyID;
		storyType = _storyType;
	}

	public Story Story {
		get {
			switch (storyType) {
			case StoryType.Normal:
				return StoryLoader.Instance.IslandStories[storyID];
				break;
			case StoryType.Treasure:
				return StoryLoader.Instance.TreasureStories[storyID];
				break;
			case StoryType.Home:
				return StoryLoader.Instance.HomeStories[storyID];
				break;
			case StoryType.Clue:
				return StoryLoader.Instance.ClueStories[storyID];
				break;
			case StoryType.Boat:
				return StoryLoader.Instance.BoatStories[storyID];
				break;
			case StoryType.Quest:
				return StoryLoader.Instance.Quests[storyID];
				break;
			default:
				return StoryLoader.Instance.IslandStories[storyID];
				break;
			}

		}
	}

	public Loot GetLoot ( int x , int y ) {
		return loots.Find (loot => (loot.row == x) && (loot.col== y) );
	}

	public void SetLoot (Loot targetLoot) {
		loots.Add (targetLoot);
	}

	public Crew GetCrew ( int x , int y ) {
		return crews.Find (crew => (crew.row == x) && (crew.col== y) );
	}

	public void SetCrew (Crew targetCrew) {
		crews.Add (targetCrew);
	}

	public List<Crew> Crews {
		get {
			return crews;
		}
		set {
			crews = value;
		}
	}

	public void SetDecal (int i) {
		SaveDecal = i;
	}
	public int GetDecal() {
		return SaveDecal;
	}

	private int SaveDecal {
		get {
			contentDecal cDecal = contentDecals.Find ((contentDecal obj) => (obj.x == StoryReader.Instance.Decal) && (obj.y == StoryReader.Instance.Index) );

			if (cDecal == default(contentDecal)) {
				return -1;
			}

			return cDecal.decal;
		}
		set {
			contentDecals.Add (new contentDecal(StoryReader.Instance.Decal , StoryReader.Instance.Index , value) );
		}
	}
}

public struct contentDecal {
	
	public int x;
	public int y;

	public int decal;
	public contentDecal (int x,int y, int decal) {
		this.x 		= x;
		this.y 		= y;
		this.decal 	= decal;
	}

	public static bool operator ==( contentDecal cd1, contentDecal cd2) 
	{
		return cd1.x == cd2.x && cd1.y == cd2.y && cd1.decal == cd2.decal;
	}
	public static bool operator != (contentDecal cd1 , contentDecal cd2) 
	{
		return !(cd1==cd2);
	}

}
