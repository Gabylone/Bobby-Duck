using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class IslandData {

	public bool gaveClue = false;

	public Vector2 positionOnScreen;
	[System.NonSerialized]
	private Vector2 appearRange = new Vector2 ( 241f , 125f );

	public StoryManager storyManager;

	public IslandData ()
	{

	}

	public IslandData (int x , int y)
	{
		storyManager = new StoryManager ();
		storyManager.InitHandler (x,y);

		float islandPosX = Random.Range (-appearRange.x , appearRange.x);
		float islandPosY = Random.Range (-appearRange.y , appearRange.y);

		positionOnScreen = new Vector2 (islandPosX, islandPosY);

	}

	public int SpriteID {
		get {
			return storyManager.storyHandlers [0].Story.spriteID;
		}
	}
}
