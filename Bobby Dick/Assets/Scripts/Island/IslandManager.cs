using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IslandManager : MonoBehaviour {

	public static IslandManager Instance;

	private bool onIsland = false;

	[SerializeField]
	private UIButton mapButton;

	void Awake() {
		Instance = this;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			print ("quittage de force");
			Leave ();
		}
	}

	#region enter island
	public void Enter (){
		
		if (OnIsland)
			return;
		
		onIsland = true;

		DisableWorldInterface ();

		StartCoroutine (EnterCoroutine ());
	}

	void DisableWorldInterface ()
	{
		mapButton.Opened = false;
		mapButton.Locked = true;

		NavigationManager.Instance.NavigationTriggers.SetActive (false);
	}

	IEnumerator EnterCoroutine () {

		Transitions.Instance.ScreenTransition.Fade = true;

		// wait for transitions
		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);

		Transitions.Instance.ActionTransition.Fade = true;
		Transitions.Instance.ScreenTransition.Fade = false;

		// place captain
		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion, Transitions.Instance.ActionTransition.Duration);

		// wait for transitions
		yield return new WaitForSeconds (Transitions.Instance.ActionTransition.Duration);

			// set story
		StoryReader.Instance.Reset ();
		StoryReader.Instance.UpdateStory ();

			// lower volume
		SoundManager.Instance.AmbianceSource.volume = SoundManager.Instance.AmbianceSource.volume / 2;


		onIsland = true;

	}

	#endregion

	#region leave island
	public void Leave () {

		// une ile a UNE histoire. une histoire a DES histoire.
		if ( StoryReader.Instance.CurrentStoryLayer > 0 ) {
			StoryReader.Instance.FallBackToPreviousStory ();
			return;
		}
		
		Transitions.Instance.ActionTransition.Fade = false;

		NavigationManager.Instance.NavigationTriggers.SetActive (true);

		Crews.playerCrew.UpdateCrew (Crews.PlacingType.Map);

		Crews.enemyCrew.Hide ();

		MapData.Instance.currentChunk.state = State.VisitedIsland;

		mapButton.Locked = false;

		SoundManager.Instance.AmbianceSource.volume = SoundManager.Instance.AmbianceSource.volume * 2;
		WeatherManager.Instance.PlaySound ();

		onIsland = false;
	}
	#endregion

	#region propeties
	public bool OnIsland {
		get {
			return onIsland;
		}
	}
	#endregion
}