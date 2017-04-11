using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class IslandData {

	[SerializeField]
	private List<Story> stories = new List<Story> ();

	private List<Loot> loots = new List<Loot>();
	private List<Crew> crews = new List<Crew>();

	private bool gaveClue = false;
	private Vector2 position;

	public bool visited = false;
	public bool seen = false;

	public IslandData ()
	{

	}

	public IslandData ( Vector2 pos )
	{
		position = pos;
	}

	public Vector2 Position {
		get {
			return position;
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
			if (stories.Count == 0)
				return null;
			return stories[IslandManager.Instance.StoryLayer];
		}
		set {
			if (stories.Count > 0) {
				Debug.LogError ("Là t'essaye d'ajouter l'histoire " + value.name + " mais y'en a déjà une");
				stories [0] = value;
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
