using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryActionButton : MonoBehaviour {

	Button button;

	public InventoryActionType inventoryActionType;

	void Start () {
		button = GetComponent<Button> ();
	}

	public void TriggerAction () {
	
		button.interactable = false;
		Tween.Bounce (transform);

		Invoke ("TriggerActionDelay" , Tween.defaultDuration);
	}


	void TriggerActionDelay () {
		button.interactable = true;

		LootUI.Instance.InventoryAction (inventoryActionType);

		//
	}

}
