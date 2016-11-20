using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IslandManager : MonoBehaviour {

	public static IslandManager Instance;

	bool onIsland = false;

	[SerializeField] private AudioClip dockSound;

	void Awake() {
		Instance = this;
	}

	public void Enter (){
		
		Transitions.Instance.ActionTransition.Switch();

		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion, Transitions.Instance.ActionTransition.Duration);
			
		if ( StoryLoader.Instance.CurrentIslandStory == null ) {
			StoryLoader.Instance.CurrentIslandStory = StoryLoader.Instance.RandomStory;
		}

		StoryReader.Instance.SetStory (StoryLoader.Instance.CurrentIslandStory);

		onIsland = true;

		Invoke ("StartStory" , Transitions.Instance.ActionTransition.Duration);

		SoundManager.Instance.PlayAmbiance (dockSound);
	}

	private void StartStory () {
		StoryReader.Instance.UpdateStory ();
	}

	public void Leave () {

		onIsland = false;

		Transitions.Instance.ActionTransition.Switch();

		Crews.playerCrew.UpdateCrew (Crews.PlacingType.Map);

		Crews.enemyCrew.Hide ();

		NavigationManager.Instance.PlaySound ();
	}

	public bool OnIsland {
		get {
			return onIsland;
		}
	}


}

public class IslandData {

	private Story story;

	private List<Loot> loots = new List<Loot>();
	private List<Crew> crews = new List<Crew>();

	private bool gaveClue = false;
	private Vector2 position;

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
