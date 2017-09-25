using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class IslandData {

	public Vector2 positionOnScreen;
	[System.NonSerialized]
	private Vector2 appearRange = new Vector2 ( 241f , 125f );

	public StoryManager storyManager;

	public IslandData ()
	{

	}

	public IslandData (IslandType storyType)
	{
		storyManager = new StoryManager ();

		storyManager.InitHandler (storyType);

		positionOnScreen = Island.Instance.GetRandomPosition ();

	}

	public int SpriteID {
		get {
			return storyManager.storyHandlers [0].Story.spriteID;
		}
	}
}
