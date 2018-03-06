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

	void Start () {
		if ( StoryFunctions.Instance )
		StoryFunctions.Instance.getFunction += HandleGetFunction;

		if (CombatManager.Instance) {
			CombatManager.Instance.onFightStart += HandleFightStarting;
			CombatManager.Instance.onFightEnd += HandleFightEnding;
		}

	}

	void HandleFightEnding ()
	{
		// non parce que du coup quand ils s'enfuient ça reste noir
//		actionTransition.Fade = true;
	}

	void HandleFightStarting ()
	{
		actionTransition.Fade = false;
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.Fade:

			FadeScreen ();

			StoryReader.Instance.NextCell ();
			StoryReader.Instance.Wait (Transitions.Instance.ActionTransition.Duration);

			break;
		}
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
	}
	IEnumerator FadeCoroutine () {

//		bool fadePlayer = Crews.playerCrew.captain.Icon.CurrentPlacingType == Crews.PlacingType.Discussion;
//		bool fadeOther = Crews.enemyCrew.CrewMembers.Count > 0;
//		if (fadePlayer)
//			Crews.playerCrew.HideCrew ();
//		if ( fadeOther )
//			Crews.enemyCrew.HideCrew ();

		ScreenTransition.QuickFade = true;

		yield return new WaitForSeconds (ScreenTransition.Duration  );

		ScreenTransition.QuickFade = false;

		yield return new WaitForSeconds (ScreenTransition.Duration );

//		if (fadePlayer)
//			Crews.playerCrew.ShowCrew ();
//		if ( fadeOther )
//			Crews.enemyCrew.ShowCrew ();
	}
}
