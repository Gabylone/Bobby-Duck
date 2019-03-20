using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInput : MonoBehaviour {

	public static DisplayInput Instance;

	public InputField inputField;

	void Awake () {
		Instance = this;
	}

	void Start () {

		Hide ();
	}

	public void Show (){
		gameObject.SetActive (true);

    }
	public void Hide (){
		gameObject.SetActive (false);
    }

    void HandleOnStopTyping ()
	{
		Show ();
		Focus ();
	}

	void HandleOnStartTyping ()
	{
		Hide ();
	}

	public delegate void OnInput ( Verb verb , Item primaryItem , Item secundaryItem );
	public static OnInput onInput;

    public List<string> inputParts;

    public void OnEndEdit () {


		string str = inputField.text;

		if (str.Length == 0)
			return;

        inputParts = str.Split(new char[2] { ' ', '\'' }).ToList<string>();

        Verb verb = Verb.Find(inputParts[0]);

        if ( verb == null)
        {

        }
        else
        {
            inputParts.RemoveAt(0);
        }


        Item primaryItem = null;
        Item secundaryItem = null;

        if ( inputParts.Count > 0 )
        {
            /// ITEM ///
            //string[] wordGroups = SplitInWordGroups(inputParts.ToArray());

            foreach (var item_str in inputParts)
            {
                if ( verb != null && verb.availableForAllItems )
                {
                    primaryItem = Item.FindByName(item_str);
                }
                else
                {
                    primaryItem = Item.GetInWord(item_str);
                }

                if (primaryItem != null)
                    break;
            }
            
            if ( primaryItem != null)
            {
                foreach (var inputPart in inputParts)
                {
                    secundaryItem = Item.GetInWord(inputPart);
                    if (secundaryItem != null && secundaryItem.row != primaryItem.row)
                        break;
                }
            }

            ///
        }
        else
        {
            
        }

        if (onInput != null)
			onInput (verb, primaryItem, secundaryItem);

		Clear ();

    }

    public string[] SplitInWordGroups(string[] args)
    {
        string[] phraseParts = new string[args.Length];
        for (int a = 0; a < args.Length; a++)
        {
            string s = "";

            for (int i = a; i < args.Length; i++)
            {
                s += args[i];

                if (i < args.Length - 1)
                {
                    s += " ";
                }
            }

            phraseParts[a] = s;
        }

        return phraseParts;
    }

    public void EndInput()
    {
        inputField.interactable = false;
        inputField.enabled = false;
        inputField.text = "";
    }

    public void OnValueChanged () {
		Sound.Instance.PlayRandomTypeSound ();
	}

	void Clear ()
	{
		inputField.text = "";
		Focus ();
	}
	void Focus () {

		inputField.Select ();
        inputField.ActivateInputField();

	}
}
