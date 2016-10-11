using UnityEngine;
using System.Collections;

public class IslandManager : MonoBehaviour {

	public static IslandManager Instance;

	void Awake() {
		Instance = this;
	}

	public void Enter (){ 
		Transitions.Instance.ActionTransition.Switch();

		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion, Transitions.Instance.ActionTransition.Duration);

		Invoke ("StartStory" , Transitions.Instance.ActionTransition.Duration);
	}

	private void StartStory () {
		Crews.playerCrew.captain.Icon.ShowBody ();
		StoryReader.Instance.PickRandomStory ();
	}

	public void Leave () {
		//
	}

	void Update () {
		
	}
}
