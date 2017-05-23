using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class IslandData {

	public string name = "";

	public int worldPositionX = 0;
	public int worldPositionY = 0;

	private StoryHandler storyHandler;

	private Vector2 appearRange = new Vector2 ( 241f , 125f );
	private Vector2 positionOnScreen;

	public IslandData ()
	{

	}

	public IslandData (int x , int y)
	{
		worldPositionX = x;
		worldPositionY = y;

		storyHandler = new StoryHandler (x,y);
		name = storyHandler.Story.name;

//		Debug.Log ("story name : " + storyHandler.Story.name);

		float islandPosX = Random.Range (-appearRange.x , appearRange.x);
		float islandPosY = Random.Range (-appearRange.y , appearRange.y);

		positionOnScreen = new Vector2 (islandPosX, islandPosY);

	}

	public Vector2 PositionOnScreen {
		get {
			return positionOnScreen;
		}
	}

	public StoryHandler StoryHandler {
		get {
			return storyHandler;
		}
	}
}
