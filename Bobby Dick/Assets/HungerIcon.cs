using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerIcon : MonoBehaviour {

	private CrewIcon linkedIcon;

	public int hungerToAppear = 50;

	public GameObject group;

	public Image fullImage;

	public Image heartImage;

	void Start () {

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

		PlayerLoot.Instance.openInventory += HandleOpenInventory;
		StoryLauncher.Instance.playStoryEvent += Hide;

		PlayerLoot.Instance.closeInventory += HandleCloseInventory;;

		linkedIcon = GetComponentInParent<CrewIcon> ();

		Hide ();
	}

	void HandleCloseInventory ()
	{
		if (StoryLauncher.Instance.PlayingStory == false) {
			HandleChunkEvent ();
		}
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

		if (fillAmount < 0.4f) {
			heartImage.gameObject.SetActive (true);
			heartImage.fillAmount = (float)linkedIcon.Member.Health / (float)linkedIcon.Member.MemberID.maxHealth;
		} else {
			heartImage.gameObject.SetActive (false);
		}

		if (fillAmount < 0.3f) {
			Tween.Bounce (group.transform, 0.2f, 1.2f);
		}
	}

	void OnDestroy () {

		NavigationManager.Instance.EnterNewChunk -= HandleChunkEvent;

		PlayerLoot.Instance.openInventory -= HandleOpenInventory;
		PlayerLoot.Instance.closeInventory -= HandleChunkEvent;
		StoryLauncher.Instance.playStoryEvent -= Hide;
	}

	void Show () {
		group.SetActive (true);
	}

	void Hide () {
		group.SetActive (false);
	}
}
