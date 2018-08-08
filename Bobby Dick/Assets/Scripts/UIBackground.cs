using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;
using System;

public class UIBackground : MonoBehaviour {

	RectTransform rectTransform;

	public float initXPos = 0f;
	public float skillX = 0f;
	public float hiddenX = 0f;

	public float duration = 0.3f;

	public GameObject uiGroup;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform> ();

        CombatManager.Instance.onChangeState += HandleOnChangeState;

        CombatManager.Instance.onFightStart += MoveBackGround;
		CombatManager.Instance.onFightEnd += HandleFightEnding;

		initXPos = rectTransform.rect.position.x;
	}

    private void HandleOnChangeState(CombatManager.States currState, CombatManager.States prevState)
    {
        if (!CombatManager.Instance.fighting)
        {
            ShowBackGround();
            return;
        }
        switch (currState)
        {
            case CombatManager.States.CombatStart:
            case CombatManager.States.PlayerMemberChoice:
            case CombatManager.States.EnemyMemberChoice:
            case CombatManager.States.StartTurn:
            case CombatManager.States.EnemyActionChoice:
            case CombatManager.States.PlayerAction:
            case CombatManager.States.EnemyAction:
                HideBackground();
                break;


            case CombatManager.States.PlayerActionChoice:
                MoveBackGround();
                break;

            default:
                break;
        }
    }

    void HandleFightEnding ()
	{
		Invoke("HandleFightEndingDelay" , duration);
        ShowBackGround();
    }

    void HandleFightEndingDelay () {

        Crews.playerCrew.UpdateCrew (Crews.PlacingType.Map);
	}

	void ShowBackGround ()
	{
		HOTween.To ( rectTransform  , duration , "anchoredPosition" , new Vector2 ( initXPos , 0f ) );

		uiGroup.SetActive (true);
	}

	void MoveBackGround ()
	{
		HOTween.To ( rectTransform  , duration , "anchoredPosition" , new Vector2 ( skillX , 0f ) );

		uiGroup.SetActive (false);
	}

	void HideBackground ()
	{
		HOTween.To ( rectTransform  , duration , "anchoredPosition" , new Vector2 ( hiddenX , 0f ) );

		uiGroup.SetActive (false);

	}
}
