using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class DisplayHunger_Icon : DisplayHunger {

	private MemberIcon linkedIcon;

	public int hungerToAppear = 50;

	public GameObject heartGroup;
	public Image heartImage;

	public float hungerToShowLife = 25f;

	public override void Start ()
	{
		base.Start ();

		linkedIcon = GetComponentInParent<MemberIcon> ();

		InitEvents ();

		HideHunger ();
		HideHeart ();
	}

	#region hear
	void ShowHeart () {
		HideHunger ();
		heartGroup.SetActive (true);
		Tween.Bounce (heartGroup.transform);
		UpdateHeartImage ();
	}
	void HideHeart() {
		heartGroup.SetActive (false);
		//
	}
	void UpdateHeartImage () {
		float l = (float)linkedIcon.member.Health / (float)linkedIcon.member.MemberID.maxHealth;
		HOTween.Kill (heartImage.rectTransform);
		HOTween.To ( heartImage , 0.5f , "fillAmount" , l );
	}
	#endregion

	void HandleEndStoryEvent ()
	{
		UpdateHungerIcon (linkedIcon.member);
	}

	void HandlePlayStoryEvent ()
	{
		HideHeart ();
		HideHunger ();
	}

	void HandleCloseInventory ()
	{
		if (StoryLauncher.Instance.PlayingStory == false) {
			HandleChunkEvent ();
		}
	}

	void HandleOpenInventory (CrewMember member)
	{
		HideHeart ();
		HideHunger ();
	}

	void HandleChunkEvent ()
	{
		UpdateHungerIcon (linkedIcon.member);
	}

	public override void UpdateHungerIcon (CrewMember member)
	{
		float fillAmount = 1f - ((float)member.CurrentHunger / (float)Crews.maxHunger);

		if (fillAmount * 100 < hungerToShowLife) {
			ShowHeart ();
		} else if (fillAmount * 100 < hungerToAppear) {
			base.UpdateHungerIcon (member);
		} else {
			HideHunger ();
		}

	}

	void InitEvents ()
	{
		NavigationManager.Instance.EnterNewChunk 	+= HandleChunkEvent;
		CrewInventory.Instance.openInventory 		+= HandleOpenInventory;
		StoryLauncher.Instance.onPlayStory 		+= HandlePlayStoryEvent;
		StoryLauncher.Instance.onEndStory 		+= HandleEndStoryEvent;;
		CrewInventory.Instance.closeInventory 		+= HandleCloseInventory;
	}

	void OnDestroy()
	{
		NavigationManager.Instance.EnterNewChunk 	-= HandleChunkEvent;
		CrewInventory.Instance.openInventory 		-= HandleOpenInventory;
		StoryLauncher.Instance.onPlayStory 		-= HandlePlayStoryEvent;
		StoryLauncher.Instance.onEndStory 		-= HandleEndStoryEvent;;
		CrewInventory.Instance.closeInventory 		-= HandleCloseInventory;
	}
}
