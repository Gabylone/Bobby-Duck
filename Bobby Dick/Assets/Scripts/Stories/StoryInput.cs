using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryInput : MonoBehaviour {

	public static StoryInput Instance;

	bool waitForInput = false;

    void Awake()
    {
        Instance = this;
        onPressInput = null;

    }

	void Start () {

		CrewInventory.Instance.onOpenInventory += HandleOpenInventory;
		CrewInventory.Instance.onCloseInventory += HandleCloseInventory;;

		StoryFunctions.Instance.getFunction += HandleGetFunction;

		WorldTouch.onPointerDown += HandlePointerDownEvent;

	}

	void HandlePointerDownEvent ()
	{
		if ( waitForInput ) {

			PressInput ();
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
			WaitForInput ();
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		 
		if ( waitForInput ) {

			if (InputManager.Instance.OnInputDown ()) {
				PressInput ();
			}
		}
	}
//
	public bool locked = false;

	public delegate void OnPressInput ();
	public static OnPressInput onPressInput;

	public void WaitForInput () {
		Invoke ("WaitForInputDelay", 0.01f);

	}
	void WaitForInputDelay () {
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
