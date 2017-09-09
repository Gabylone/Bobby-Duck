using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerIcon : MonoBehaviour {

	private CrewIcon linkedIcon;

	public int hungerToAppear = 50;

	public GameObject group;

	public Image fullImage;

	void Start () {

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

		PlayerLoot.Instance.openInventory += HandleOpenInventory;
		StoryLauncher.Instance.playStoryEvent += Hide;

		PlayerLoot.Instance.closeInventory += HandleChunkEvent;

		linkedIcon = GetComponentInParent<CrewIcon> ();

		Hide ();
	}

	void HandleOpenInventory (CrewMember member)
	{
		Hide ();
	}

	void HandleChunkEvent ()
	{
		if ( linkedIcon.Member.CurrentHunger> hungerToAppear ) {
			UpdateIcon ();
			Show ();
		}
	}

	void UpdateIcon () {

		float fillAmount = 1f - ((float)linkedIcon.Member.CurrentHunger / (float)linkedIcon.Member.MaxState);

		fullImage.fillAmount = fillAmount;

		if (fillAmount < 0.1f) {
			Tween.Bounce (group.transform, 0.2f, 1.2f);
		}
	}

	void OnDestroy () {

		NavigationManager.Instance.EnterNewChunk -= HandleChunkEvent;

		PlayerLoot.Instance.openInventory -= HandleOpenInventory;
		PlayerLoot.Instance.closeInventory -= HandleChunkEvent;
		StoryLauncher.Instance.playStoryEvent -= Hide;
	}

	public void Show () {
		group.SetActive (true);
	}

	public void Hide () {
		group.SetActive (false);
	}
}
