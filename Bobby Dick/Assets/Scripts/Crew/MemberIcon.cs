﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class MemberIcon : MonoBehaviour {

	public int index = 0;

		// components
	[Header ("Components")]
	public GameObject group;
	public GameObject faceGroup;
	public GameObject bodyGroup;

	public Animator animator;

    RectTransform rectTransform;

	public bool overable = true;

	public float moveDuration = 1f;

	public float bodyScale = 1f;

	public float initScale;

	public Crews.PlacingType currentPlacingType = Crews.PlacingType.None;
	public Crews.PlacingType previousPlacingType  = Crews.PlacingType.None;

    public Vector2 decal = Vector2.zero;

	public Transform dialogueAnchor;

	public CrewMember member;

    public DisplayHunger_Icon hungerIcon;

    IconVisual iconVisual;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

    }

    void Start () {
        LootUI.useInventory += HandleUseInventory;
    }

    void OnDestroy()
    {
        LootUI.useInventory -= HandleUseInventory;
    }

    void HandleUseInventory(InventoryActionType actionType)
    {
        switch (actionType)
        {
            case InventoryActionType.Equip:
            case InventoryActionType.PurchaseAndEquip:
            case InventoryActionType.Unequip:

                if (LootUI.Instance.SelectedItem.category == ItemCategory.Weapon)
                {
                    iconVisual.UpdateWeaponSprite(CrewMember.GetSelectedMember.MemberID);
                }

                break;
            default:
                break;
        }


    }

    public void SetMember (CrewMember member) {

		this.member = member;

		animator = GetComponentInChildren<Animator> ();
	
		HideBody ();

		InitVisual (member.MemberID);

	}

    #region overing
    public void OnPointerDown()
    {
        if (member.side == Crews.Side.Enemy)
        {
            StoryInput.Instance.LockFromMember();
            GetComponentInChildren<StatGroup>().Display(member);
            return;
        }

        if (!InGameMenu.Instance.canOpen)
        {
            print("cannot open player loot");
            return;
        }

        LootUI.Instance.OpenMemberLoot(member);

        SkillMenu.Instance.Close();
    }
    #endregion

    #region movement
    public void MoveToPoint ( Crews.PlacingType targetPlacingType ) {

		previousPlacingType = currentPlacingType;
		currentPlacingType = targetPlacingType;

		Vector3 targetPos = Crews.getCrew(member.side).CrewAnchors [(int)targetPlacingType].position;

        int index = member.GetIndex;

        if (currentPlacingType == Crews.PlacingType.Map) {

            //PlayerIcons.Instance.GetImage(index).color = Color.white;

            if (index < 0) {
				Debug.LogError ("index : " + index + " mapanchors :" + Crews.getCrew (member.side).mapAnchors.Length);
				Debug.LogError ("membre à probleme  "+ member.MemberName);
				Debug.LogError ("current membre  "+ CrewMember.GetSelectedMember.MemberName);
			}

			targetPos = Crews.getCrew (member.side).mapAnchors [member.GetIndex].position;
		}
        else
        {
            //PlayerIcons.Instance.GetImage(index).color = Color.clear;
        }

        //Debug.Log ("Moving To : " + Crews.getCrew(member.side).CrewAnchors [(int)targetPlacingType].name);

        rectTransform.DOMove(targetPos, moveDuration);

		switch (currentPlacingType) {

            case Crews.PlacingType.Map:

                HideBody();

                break;

            case Crews.PlacingType.Hidden:

                HideBody();

                break;

            case Crews.PlacingType.None:

                HideBody();

                break;

            case Crews.PlacingType.MemberCreation:

                ShowBody();

                break;

            case Crews.PlacingType.Inventory:

                ShowBody();

                if ( member.side == Crews.Side.Player)
                {
                    hungerIcon.HideInfo();
                }

                break;

            case Crews.PlacingType.Discussion:

                ShowBody();

                if (member.side == Crews.Side.Player)
                {
                    hungerIcon.HideInfo();
                }

                break;

            default:

                break;

		}
	}
	#endregion

	#region body
	public void HideBody () {
		
		bodyGroup.SetActive (false);
		animator.SetBool ("enabled", false);

		Vector3 targetScale = Vector3.one * initScale;
		if (member.side == Crews.Side.Player)
			targetScale.x = -targetScale.x;

        group.transform.DOScale(targetScale, moveDuration);

	}
	public void ShowBody () {
		
		bodyGroup.SetActive (true);
		animator.SetBool ("enabled", true);

        Vector3 targetScale = Vector3.one * bodyScale;
        if (member.side == Crews.Side.Player)
            targetScale.x = -targetScale.x;

        group.transform.DOScale(targetScale,moveDuration);

    }

	public void InitVisual (Member memberID)
	{
        iconVisual = GetComponent<IconVisual>();
		iconVisual.InitVisual (memberID);
    }
	#endregion
}