using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFeedback : TextTyper {

	public static DisplayFeedback Instance;

	void Awake () {
		Instance = this;
	}

	public override void Start ()
	{
		base.Start ();

		DisplayInput.onInput += HandleOnInput;
		Player.onPlayerMove += HandleOnPlayerMove;
	}

	void HandleOnPlayerMove (Coords prevCoords, Coords newCoords)
	{
		Clear ();
	}

	void HandleOnInput (Verb verb, Item item)
	{
		if ( item == null && verb == null ) {
			AddToText ("Quoi ?");
			UpdateText ();
			Sound.Instance.PlayWrongBip ();
			return;
		}

		if ( verb == null ) {
			AddToText ("Que faire avec " + item.word.GetDescription(Word.Def.Defined) );
			UpdateText ();
			Sound.Instance.PlayWrongBip ();
			return;
		}

		if ( item == null ) {
			AddToText ("Avec quoi voulez vous " + verb.names[0]);
			UpdateText ();
			Sound.Instance.PlayWrongBip ();
			return;
		}

	}

	public void Display ( string str ) {
		AddToText (str);
		UpdateText ();
	}
}
