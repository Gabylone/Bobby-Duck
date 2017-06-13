using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class IslandData {

	public string name = "";

	public int spriteID = 0;

	public Vector2 positionOnScreen;

	[SerializeField]
	private StoryHandler storyHandler;

	private Vector2 appearRange = new Vector2 ( 241f , 125f );

	public IslandData ()
	{

	}

	public IslandData (int x , int y)
	{

		storyHandler = new StoryHandler (x,y);
		name = storyHandler.Story.name;
		spriteID = storyHandler.Story.spriteID;

//		Debug.Log ("story name : " + storyHandler.Story.name);

		float islandPosX = Random.Range (-appearRange.x , appearRange.x);
		float islandPosY = Random.Range (-appearRange.y , appearRange.y);

		positionOnScreen = new Vector2 (islandPosX, islandPosY);


	}

	public StoryHandler StoryHandler {
		get {
			return storyHandler;
		}
	}
}
