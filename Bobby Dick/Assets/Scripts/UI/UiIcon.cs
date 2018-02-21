using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiIcon : MonoBehaviour {

	[SerializeField]
	private GameObject uiGroup;

	// Use this for initialization
	public virtual void Start () {
		
		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

		CrewInventory.Instance.closeInventory += HandleCloseInventory;

		CrewInventory.Instance.openInventory += HandleOpenInventory;

		UpdateUI ();

		Hide ();
	}

	public void HandleUseInventory (InventoryActionType actionType)
	{
		UpdateUI ();
	}

	public virtual void HandleChunkEvent ()
	{
		Debug.Log("chunk ui icon");
		Show ();
		Invoke ("Hide", 1f);
	}

	public virtual void HandleOpenInventory (CrewMember member)
	{
		CancelInvoke ();
		Show ();
	}

	public virtual void HandleCloseInventory ()
	{
		Hide ();
	}

	public void Show ()
	{
		uiGroup.SetActive (true);
	}

	public void Hide ()
	{
		uiGroup.SetActive (false);
	}

	public virtual void UpdateUI ()
	{
		
	}
}
