using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiIcon : MonoBehaviour {

	[SerializeField]
	private GameObject uiGroup;

	// Use this for initialization
	public virtual void Start () {
		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;;
		PlayerLoot.Instance.closeInventory += HandleCloseInventory;
		PlayerLoot.Instance.openInventory += HandleOpenInventory;
		PlayerLoot.Instance.LootUI.useInventory += HandleUseInventory;

		UpdateUI ();
	}

	public void HandleUseInventory (InventoryActionType actionType)
	{
		UpdateUI ();
	}

	public virtual void HandleChunkEvent ()
	{
		Show ();
		Invoke ("Close", 1f);
	}

	public virtual void HandleOpenInventory ()
	{
		CancelInvoke ();
		Show ();
	}

	public virtual void HandleCloseInventory ()
	{
		Close ();
	}

	public void Show ()
	{
		uiGroup.SetActive (true);
	}

	public void Close ()
	{
		uiGroup.SetActive (false);
	}

	public virtual void UpdateUI ()
	{
		
	}
}
