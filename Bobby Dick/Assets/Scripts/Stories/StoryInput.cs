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
		StoryFunctions.Instance.getFunction += HandleGetFunction;

		CrewInventory.Instance.openInventory += HandleOpenInventory;
		CrewInventory.Instance.closeInventory += HandleCloseInventory;;
	}

	void HandleCloseInventory ()
	{
		Invoke ("Unlock", 0.01f);
	}

	void Unlock () {
		locked = false;
	}

	void HandleOpenInventory (CrewMember member)
	{
		locked = true;
	}

	public void Lock () {
		locked = true;
		Invoke ("Unlock", 0.01f);
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
			Invoke ("Story_WaitForInput",0.01f);
			break;
		}
	}

	void Story_WaitForInput () {
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
