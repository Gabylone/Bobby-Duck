using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerIcon : MonoBehaviour {

	private MemberIcon linkedIcon;

	public int hungerToAppear = 50;

	public GameObject hungerGroup;

	public Image fullImage;

	public Image heartImage;

	public GameObject heartGroup;

	void Start () {

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

		CrewInventory.Instance.openInventory += HandleOpenInventory;

		StoryLauncher.Instance.playStoryEvent += HandlePlayStoryEvent;
		StoryLauncher.Instance.endStoryEvent += HandleEndStoryEvent;;

		CrewInventory.Instance.closeInventory += HandleCloseInventory;;

		linkedIcon = GetComponentInParent<MemberIcon> ();

		heartGroup.SetActive (false);
		hungerGroup.SetActive (false);
	}

	void HandleEndStoryEvent ()
	{
		UpdateIcon ();
	}

	void HandlePlayStoryEvent ()
	{
		heartGroup.SetActive (false);
		hungerGroup.SetActive (false);
	}

	void HandleCloseInventory ()
	{
		if (StoryLauncher.Instance.PlayingStory == false) {
			HandleChunkEvent ();
		}
	}

	void HandleOpenInventory (CrewMember member)
	{
		hungerGroup.SetActive (false);
		heartGroup.SetActive (false);
	}

	void HandleChunkEvent ()
	{
		UpdateIcon ();
	}

	void UpdateIcon () {

		float fillAmount = 1f - ((float)linkedIcon.member.CurrentHunger / (float)linkedIcon.member.maxHunger);

		if (fillAmount < 0.45f) {
			
			if ( fillAmount <= 0.2f ) {
				heartImage.fillAmount = (float)linkedIcon.member.Health / (float)linkedIcon.member.MemberID.maxHealth;
				heartGroup.SetActive (true);
				hungerGroup.SetActive (false);

			} else {
				fullImage.fillAmount = fillAmount;
				heartGroup.SetActive (false);
				hungerGroup.SetActive (true);
			}

			Tween.Bounce (transform, 0.2f, 1.2f);

		} else {
			hungerGroup.SetActive (false);
		}

	}

	void OnDestroy () {

		NavigationManager.Instance.EnterNewChunk -= HandleChunkEvent;

		CrewInventory.Instance.openInventory -= HandleOpenInventory;
		CrewInventory.Instance.closeInventory -= HandleChunkEvent;
		StoryLauncher.Instance.playStoryEvent -= HandlePlayStoryEvent;
	}
}
