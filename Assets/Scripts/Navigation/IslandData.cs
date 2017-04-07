using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class IslandData {

	[SerializeField]
	private Story story;

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

	public Story Story {
		get {
			return story;
		}
		set {
			story = value;
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
