using UnityEngine;
using System.Collections;

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
			
		// SET STORY
		if ( StoryLoader.Instance.CurrentIslandStory == null ) {
			StoryLoader.Instance.CurrentIslandStory = StoryReader.Instance.RandomStory;
		}

//		StoryReader.Instance.SetStory (StoryLoader.Instance.Stories[0]);
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

	public void TestStory ( int id ) {
		
	}
}
