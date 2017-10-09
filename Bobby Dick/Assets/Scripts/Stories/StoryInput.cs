using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryInput : MonoBehaviour {

	bool waitForInput = false;

	// Use this for initialization
	void Start () {
		StoryFunctions.Instance.getFunction += HandleGetFunction;

		CrewInventory.Instance.openInventory += HandleOpenInventory;
		CrewInventory.Instance.closeInventory += HandleCloseInventory;;
	}

	void HandleCloseInventory ()
	{
		Invoke ("Unlock_Delay", 0.01f);
	}
	void Unlock_Delay () {
		locked = false;
	}

	void HandleOpenInventory (CrewMember member)
	{
		locked = true;
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.Narrator:
		case FunctionType.OtherSpeak:
		case FunctionType.PlayerSpeak:
		case FunctionType.GiveTip:
		case FunctionType.AddToInventory:
		case FunctionType.RemoveFromInventory:
		case FunctionType.ShowQuestOnMap:
			Invoke ("WaitForSomeReason",0.01f);
			break;
		}
	}
	void WaitForSomeReason () {
		WaitForInput ();
	}

	// Update is called once per frame
	void Update () {
		if ( waitForInput ) {
			if (InputManager.Instance.OnInputDown ()) {
				PressInput ();
			}
		}
	}

	public bool locked = false;

	public delegate void OnPressInput ();
	public static OnPressInput onPressInput;

	void WaitForInput () {
		waitForInput = true;
	}

	void PressInput () {

		if (locked) {
			return;
		}


		if (onPressInput != null) {
			onPressInput ();
		}

		waitForInput = false;
	}

}
