using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StoryHandler {

	public bool gaveClue = false;

	public List<Story> stories = new List<Story> ();
	public List<Loot> loots = new List<Loot> ();
	public List<Crew> crews = new List<Crew>();

	public StoryHandler () {
		//
	}

	public StoryHandler (int x , int y) {
		Story = StoryLoader.Instance.RandomStory(x,y);
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
				Debug.Log ("mais y'en a pas");
				return null;
			}

			return stories[StoryReader.Instance.CurrentStoryLayer];
		}
		set {
			if (stories.Count > 0) {
				
//				Debug.LogError ("Là t'essaye d'ajouter l'histoire " + value.name + " mais y'en a déjà une");

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
