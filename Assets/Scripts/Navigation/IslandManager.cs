using UnityEngine;
using System.Collections;

public class IslandManager : MonoBehaviour {

	public static IslandManager Instance;

	bool onIsland = false;

	void Awake() {
		Instance = this;
	}

	public void Enter (){ 
		Transitions.Instance.ActionTransition.Switch();

		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion, Transitions.Instance.ActionTransition.Duration);

		Invoke ("StartStory" , Transitions.Instance.ActionTransition.Duration);
	}

	private void StartStory () {

		onIsland = true;

		Crews.playerCrew.captain.Icon.ShowBody ();
		StoryReader.Instance.PickRandomStory ();

	}

	public void Leave () {

		onIsland = false;

		Transitions.Instance.ActionTransition.Switch();

		Crews.playerCrew.UpdateCrew (Crews.PlacingType.Map);

		Crews.enemyCrew.Hide ();
	}

	void Update () {
		
	}

	public bool OnIsland {
		get {
			return onIsland;
		}
	}
}
