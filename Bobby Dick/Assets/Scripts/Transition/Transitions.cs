using UnityEngine;
using System.Collections;

public class Transitions : MonoBehaviour {

	public static Transitions Instance;

	[SerializeField]
	private Transition screenTransition;

	[SerializeField]
	private Transition actionTransition;

	void Awake () {
		Instance = this;
	}

	public Transition ScreenTransition {
		get {
			return screenTransition;
		}
		set {
			screenTransition = value;
		}
	}

	public Transition ActionTransition {
		get {
			return actionTransition;
		}
		set {
			actionTransition = value;
		}
	}

	public void FadeScreen () {
		StartCoroutine (FadeCoroutine ());
		//
	}
	IEnumerator FadeCoroutine () {

		bool fadePlayer = Crews.playerCrew.captain.Icon.CurrentPlacingType == Crews.PlacingType.Discussion;
		bool fadeOther = Crews.enemyCrew.CrewMembers.Count > 0;
		if (fadePlayer)
			Crews.playerCrew.HideCrew ();
		if ( fadeOther )
			Crews.enemyCrew.HideCrew ();

		ScreenTransition.Fade = true;

		yield return new WaitForSeconds (ScreenTransition.Duration );

		ScreenTransition.Fade = false;

		yield return new WaitForSeconds (ScreenTransition.Duration );

		if (fadePlayer)
			Crews.playerCrew.ShowCrew ();
		if ( fadeOther )
			Crews.enemyCrew.ShowCrew ();
	}
}
