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
}
