using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Action {

	public static Action last;

	public enum Type {

		None,

		Move,
		MoveRel,
		Look,
		DisplayInventory,
        CloseInventory,
		Enter,
        GoOut,
		Eat,
		Drink,
		Sleep,
		Take,
        Throw,
		AddToInventory,
		RemoveFromInventory,
        AddToTile,
        RemoveFromTile,
		Require,
		Display,
        GiveClue,
        MoveAway,
        OpenContainer,
        CloseContainer,
        Equip,
        Unequip,
        DescribeExterior,
        DisplayTimeOfDay,
        ExitByWindow,
        DescribeItem,
    }

	public Type type;

	public List<string> contents = new List<string>();

	public List<int> ints = new List<int>();

	public Item item;

}

public class ActionManager : MonoBehaviour {


	public static ActionManager Instance;

	public delegate void OnAction ( Action action );
	public static OnAction onAction;

	bool breakActions = false;
	public void BreakAction () {
		breakActions = true;
	}

	void Awake () {
		Instance = this;
	}

	void Start () {
		DisplayInput.onInput += HandleOnInput;
	}

    void HandleOnInput (Verb verb, Item item)
	{
        if (item == null && verb == null)
        {
            DisplayFeedback.Instance.Display("Quoi ?");
            return;
        }

        if (verb == null)
        {
            DisplayFeedback.Instance.Display("Que faire avec " + item.word.GetDescription(Word.Def.Defined));
            return;
        }

        if (verb != null && item == null)
        {
            item = Item.FindByName("verbe seul");
            if (!verb.cellContents.ContainsKey(item.row))
            {
                DisplayFeedback.Instance.Display(verb.question + " voulez vous " + verb.names[0]);
                return;
            }
        }

        if (item == null)
        {
            DisplayFeedback.Instance.Display(verb.question + " voulez vous " + verb.names[0]);
            return;
        }

        if (!verb.cellContents.ContainsKey(item.row))
        {
            DisplayFeedback.Instance.Display("Vous ne pouvez pas " + verb.names[0] + " " + item.word.GetDescription(Word.Def.Defined));
            return;
        }

        string str = verb.cellContents [item.row];

		bool foundAction = false;

		foreach (var part in str.Split('/') ) {

            Action action = GetAction(part, item);

            if (action != null)
            {

                onAction(action);

                foundAction = true;

                if (breakActions)
                {
                    Debug.Log(" !!!!! ACTION SEARCH IS STOPPED DUE TO THING !!!! ");
                    breakActions = false;
                    break;
                }

            }
        }

		if (foundAction) {
			Sound.Instance.PlayCorrectBip ();
		}
	}

	public Action GetAction ( string str , Item item ) {

		Action.Type[] actionTypes = System.Enum.GetValues (typeof(Action.Type)) as Action.Type[];

		bool hasParameters = str.Contains ( "(" );
		string function_str = str;
		if ( hasParameters ) {
			function_str = str.Remove (str.IndexOf ('('));

		}

		Action.Type actionType = System.Array.Find ( actionTypes , x => function_str.ToLower() == x.ToString().ToLower() );

		if ( actionType == Action.Type.None ) {

			Debug.LogError ("Couldn't find action type : " + function_str);

			return null;
		}

		Action newAction = new Action ();
		newAction.type = actionType;
		newAction.item = item;

	

		if ( hasParameters ) {

			string parameters_str = str.Remove (0,actionType.ToString().Length);

			// remove parentheses
			parameters_str = parameters_str.Remove (0, 1);
			parameters_str = parameters_str.Remove (parameters_str.Length - 1);

			string[] args = parameters_str.Split (',');

			foreach (var arg in args) {
				
				int i = 0;
				if ( int.TryParse ( arg , out i ) ) {
					newAction.ints.Add (i);
				} else {
					newAction.contents.Add (arg);
				}

			}
		}

		Action.last = newAction;

		return newAction;

	}

}
