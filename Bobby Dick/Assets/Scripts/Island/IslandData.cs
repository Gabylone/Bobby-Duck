using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class IslandData {

	public Vector2 positionOnScreen;

	public StoryManager storyManager;

	public IslandData ()
	{

	}

	public IslandData (StoryType storyType)
	{
		storyManager = new StoryManager ();

		storyManager.InitHandler (storyType);

		positionOnScreen = Island.Instance.GetRandomPosition ();

	}

	public int SpriteID {
		get {
			return storyManager.storyHandlers [0].Story.param;
		}
	}
}
