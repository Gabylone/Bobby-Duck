﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerIcon : MonoBehaviour {

	private MemberIcon linkedIcon;

	public int hungerToAppear = 50;

	public GameObject group;

	public Image fullImage;

	public Image heartImage;

	public GameObject heartGroup;

	void Start () {

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

		CrewInventory.Instance.openInventory += HandleOpenInventory;
		StoryLauncher.Instance.playStoryEvent += Hide;

		CrewInventory.Instance.closeInventory += HandleCloseInventory;;

		linkedIcon = GetComponentInParent<MemberIcon> ();

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
		if ( linkedIcon.member.CurrentHunger> hungerToAppear ) {
			UpdateIcon ();
			Show ();
		}
	}

	void UpdateIcon () {

		float fillAmount = 1f - ((float)linkedIcon.member.CurrentHunger / (float)linkedIcon.member.maxHunger);

		fullImage.fillAmount = fillAmount;

		heartImage.fillAmount = (float)linkedIcon.member.Health / (float)linkedIcon.member.MemberID.maxHealth;

		if (fillAmount < 0.4f) {
			Show ();
		} else {
			Hide ();
		}

		if (fillAmount < 0.3f) {
			Tween.Bounce (group.transform, 0.2f, 1.2f);
		}

		if ( fillAmount <= 0.05f ) {
			heartGroup.SetActive (true);
			//
		} else {
			heartGroup.SetActive (false);
		}
	}

	void OnDestroy () {

		NavigationManager.Instance.EnterNewChunk -= HandleChunkEvent;

		CrewInventory.Instance.openInventory -= HandleOpenInventory;
		CrewInventory.Instance.closeInventory -= HandleChunkEvent;
		StoryLauncher.Instance.playStoryEvent -= Hide;
	}

	void Show () {
		group.SetActive (true);
	}

	void Hide () {
		group.SetActive (false);
	}
}
