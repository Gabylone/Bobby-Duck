using System;
using System.Collections;
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

	public delegate void OnInput ( Verb verb , Item item );
	public static OnInput onInput;

	public void OnEndEdit () {


		string str = inputField.text;

		if (str.Length == 0)
			return;

        List<string> inputParts = str.Split(new char[2] { ' ', '\'' }).ToList<string>();

        Verb verb = Verb.Find(inputParts[0]);

        if ( verb == null)
        {
            
        }
        else
        {
            inputParts.RemoveAt(0);
        }

        Item item = null;

        if ( inputParts.Count > 0)
        {
            /// ITEM ///
            string[] wordGroups = SplitInWordGroups(inputParts.ToArray());
            item = Item.GetInWords(wordGroups);

            if (item == null)
            {
                
            }
        }
        else
        {
            
        }

        if (onInput != null)
			onInput (verb, item);

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
