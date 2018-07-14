using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Narrator : MonoBehaviour {

	public static Narrator Instance;

	[Header("Narrator")]
	[SerializeField] private Text narratorText;
	[SerializeField] private GameObject narratorObj;

	void Awake () {
		Instance = this;
	}

	void Start () {
		StoryFunctions.Instance.getFunction+= HandleGetFunction;

		CrewInventory.Instance.onOpenInventory += HandleOpenInventory;
		CrewInventory.Instance.onCloseInventory += HandleCloseInventory;

		LootManager.onRemoveItemFromInventory += HandleOnRemoveItemFromInventory;

		StoryInput.onPressInput += HandleOnPressInput;

	}

	void HandleOnRemoveItemFromInventory (Item item)
	{
		ShowNarrator ("objet retiré : " + item.name);
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.Narrator:
			ShowNarrator (cellParameters.Remove (0, 2));
			break;
		}
	}

	void HandleOnPressInput ()
	{
		HideNarrator ();
	}

	bool previousActive = false;
	void HandleOpenInventory (CrewMember member)
	{
		if (narratorObj.activeSelf) {

			narratorObj.SetActive (false);

			previousActive = true;
		}
	}

	void HandleCloseInventory ()
	{
		if ( previousActive ) {

			narratorObj.SetActive (true);

			previousActive = false;	
		}
	}

	#region narrator
	public void ShowNarratorTimed (string text) {

		ShowNarrator (text);
		Invoke ("HideNarrator" , 2.5f );
	}
	public void ShowNarrator (string text) {

		Tween.Bounce (narratorObj.transform , 0.1f , 1.01f);

		narratorObj.SetActive (true);

		narratorText.text = NameGeneration.CheckForKeyWords (text);
	}
	public void HideNarrator () {
		narratorObj.SetActive (false);
	}
	#endregion
}
