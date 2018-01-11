using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryInput : MonoBehaviour {

	public static StoryInput Instance;

	bool waitForInput = false;

	void Awake () {
		Instance = this;
	}

	void Start () {

		CrewInventory.Instance.openInventory += HandleOpenInventory;
		CrewInventory.Instance.closeInventory += HandleCloseInventory;;

		StoryFunctions.Instance.getFunction += HandleGetFunction;

		WorldTouch.pointerDownEvent += HandlePointerDownEvent;

	}

	void HandlePointerDownEvent ()
	{
		if ( waitForInput ) {

			PressInput ();
//			Invoke("PressInput",0.1f);
//
//			if (InputManager.Instance.OnInputDown ()) {
//				//				PressInput ();
//			}
		}
	}

	void HandleCloseInventory ()
	{
		Invoke ("Unlock", 0.2f);
	}

	void Unlock () {
		locked = false;
	}

	void HandleOpenInventory (CrewMember member)
	{
		locked = true;
	}

	public void LockFromMember () {
		locked = true;
		Invoke ("Unlock", 0.3f);
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
			Invoke ("WaitForInput", 0.1f);
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		 
		if ( waitForInput ) {

			if (InputManager.Instance.OnInputDown ()) {
//				PressInput ();
				Invoke("PressInput",0.1f);
			}
		}
	}
//
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
