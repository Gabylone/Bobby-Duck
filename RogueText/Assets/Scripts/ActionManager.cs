using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    // singleton
    public static ActionManager Instance;

    // action event
    public delegate void OnAction(Action action);
    public static OnAction onAction;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DisplayInput.onInput += DisplayDescriptionFeedback;
    }

    void DisplayDescriptionFeedback(Verb verb, Item primaryItem, Item secundaryItem)
    {
            // check if ANYTHING has been recognized
        if (primaryItem == null && verb == null)
        {
            DisplayFeedback.Instance.Display("Quoi ?");
            return;
        }

            // check if verb has been recognized
        if (verb == null)
        {
            DisplayFeedback.Instance.Display("Que faire avec " + primaryItem.word.GetDescription(Word.Def.Defined));
            return;
        }

            // check if item has been recognized
        if (verb != null && primaryItem == null)
        {
            primaryItem = Item.FindByName("verbe seul");

            if (!verb.cellContents.ContainsKey(primaryItem.row))
            {
                DisplayFeedback.Instance.Display(verb.question + " voulez vous " + verb.names[0]);
                return;
            }
        }
        
            // only verb
        if (primaryItem == null)
        {
            DisplayFeedback.Instance.Display(verb.question + " voulez vous " + verb.names[0]);
            return;
        }

            // verb / item combinaison doesn't exist
        if (!verb.cellContents.ContainsKey(primaryItem.row))
        {
            DisplayFeedback.Instance.Display("Vous ne pouvez pas " + verb.names[0] + " " + primaryItem.word.GetDescription(Word.Def.Defined));
            return;
        }

            // get cell content
        string str = verb.cellContents[primaryItem.row];

        bool foundAction = false;

        // separate all actions
        foreach (var part in str.Split('\n'))
        {
            // parse action
            Action action = GetAction(verb, part, primaryItem, secundaryItem);


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

        if (foundAction)
        {
            Sound.Instance.PlayCorrectBip();
        }
    }

    public Action GetAction(Verb verb, string str, Item primaryItem, Item secundaryItem)
    {
        Action.Type[] actionTypes = System.Enum.GetValues(typeof(Action.Type)) as Action.Type[];
        
        str = str.TrimEnd('"');
        str = str.TrimStart('"');

            // check parameters
        bool hasParameters = str.Contains("(");
        string function_str = str;
        if (hasParameters)
        {
            function_str = str.Remove(str.IndexOf('('));
        }

            // get action type
        Action.Type actionType = System.Array.Find(actionTypes, x => function_str.ToLower() == x.ToString().ToLower());

        if (actionType == Action.Type.None)
        {

            Debug.LogError("Couldn't find action type : " + function_str);

            return null;
        }

            // create action 
        Action newAction = new Action();
        newAction.type = actionType;
        newAction.primaryItem = primaryItem;
        newAction.secundaryItem = secundaryItem;
        newAction.verb = verb;

            // check parameters
        if (hasParameters)
        {
            string parameters_str = str.Remove(0, actionType.ToString().Length);

            // remove parentheses
            parameters_str = parameters_str.Remove(0, 1);
            parameters_str = parameters_str.Remove(parameters_str.Length - 1);

            string[] args = parameters_str.Split(',');

            foreach (var arg in args)
            {

                int i = 0;
                if (int.TryParse(arg, out i))
                {
                    newAction.ints.Add(i);
                }
                else
                {
                    newAction.contents.Add(arg);
                }

            }
        }

        Action.current = newAction;

        return newAction;
    }

    #region action breaking
    bool breakActions = false;

    public void BreakAction()
    {
        breakActions = true;
    }
    #endregion

}
