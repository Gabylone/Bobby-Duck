using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {

	public static Action last;

	public enum Type {

		None,

		Move,
		MoveRel,
		LookAround,
		Enter,
		GoOut,
		Eat,
		Drink,
		Sleep,
		Describe
	}

	public Type type;

	public List<string> contents = new List<string>();

	public List<int> ints = new List<int>();

}

public class ActionManager : MonoBehaviour {

	public delegate void OnAction ( Action action );
	public static OnAction onAction;

	public static void CheckAction ( string str ) {

		Action.Type[] actionTypes = System.Enum.GetValues (typeof(Action.Type)) as Action.Type[];

		string functionOnly = str;
		if ( functionOnly.Contains ( "(" ) ) {
			functionOnly = functionOnly.Remove (str.IndexOf ('('));
			Debug.LogError ("cell coupée : " + functionOnly);
		}

		Action.Type actionType = System.Array.Find ( actionTypes , x => functionOnly.ToLower() == x.ToString().ToLower() );

		if (actionType != Action.Type.None) {

			Action newAction = new Action ();
			newAction.type = actionType;
			Action.last = newAction;

			Debug.Log ("action trouvée : " + actionType.ToString ());

			string parameters = str.Remove (0,actionType.ToString().Length);

//			string[] args = new string[0];

			if ( parameters.Length > 0 ) {

				// remove parentheses
				parameters = parameters.Remove (0, 1);
				parameters = parameters.Remove (parameters.Length - 1);

				string[] args = parameters.Split (',');

				foreach (var item in args) {
					int i = 0;
					if ( int.TryParse ( item , out i ) ) {
		
						newAction.ints.Add (i);
						Debug.LogError ("found int : " + i.ToString());

					} else {
						Debug.LogError (item + " is not parsable");
						newAction.contents.Add (item);
					}
				}
			}

			Sound.Instance.PlayCorrectBip ();

			if ( onAction != null ) {
				onAction (newAction);
			}

		} else {
			Debug.Log ("action inconnue");
		}
	}

}
