using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class IslandData {

	public string name = "";

	public int worldPositionX = 0;
	public int worldPositionY = 0;

	[SerializeField]
	private List<Story> stories = new List<Story> ();

	private List<Loot> loots = new List<Loot>();
	private List<Crew> crews = new List<Crew>();

	private bool gaveClue = false;

	private Vector2 appearRange = new Vector2 ( 241f , 125f );
	private Vector2 positionOnScreen;


	public IslandData ()
	{

	}

	public IslandData (int x , int y)
	{
		worldPositionX = x;
		worldPositionY = y;

		Story = StoryLoader.Instance.RandomStory(x,y);

		name = Story.name;

		float islandPosX = Random.Range (-appearRange.x , appearRange.x);
		float islandPosY = Random.Range (-appearRange.y , appearRange.y);

		positionOnScreen = new Vector2 (islandPosX, islandPosY);
	}

	public Vector2 PositionOnScreen {
		get {
			return positionOnScreen;
		}
	}

	public List<Story> Stories {
		get {
			return stories;
		}
		set {
			stories = value;
		}
	}

	public Story Story {
		get {
			if (stories.Count == 0) {
				return null;
			}
			return stories[StoryReader.Instance.CurrentStoryLayer];
		}
		set {
			if (stories.Count > 0) {
				Debug.LogError ("Là t'essaye d'ajouter l'histoire " + value.name + " mais y'en a déjà une");
				stories [0] = value;
				return;
			}

//			print 
			stories.Add (value);
		}
	}

	public List<Loot> Loots {
		get {
			return loots;
		}
		set {
			loots = value;
		}
	}

	public List<Crew> Crews {
		get {
			return crews;
		}
		set {
			crews = value;
		}
	}
}
