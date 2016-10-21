﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour {

	public static CombatManager Instance;

	Crews.Side attackingCrew;
	Crews.Side defendingCrew;
	Crews.Side targetCrew;

	// states
	public enum States {
		None,
		CombatStart,
		CrewPlacement,
		MemberChoice,
		MemberLerp,
		StartTurn,
		DiceThrow,
		Results,
		MemberReturn,
	}
	private States previousState = States.None;
	private States currentState = States.None;

	bool firstTurn = false;

	Quaternion lerpRot = Quaternion.identity;

	private CrewMember[] members = new CrewMember[2];

	DiceTypes currentDiceType;

	public delegate void UpdateState();
	UpdateState updateState;

	[Header("States Duration")]
	[SerializeField]
	private float crewPlacement_Duration = 1f;

	[SerializeField]
	private float memberPlacement_Duration = 1f;

	[SerializeField]
	private float resultsDuration = 1f;

	private float timeInState = 0f;

	bool choosingMember = false;

	[SerializeField]
	private GameObject chooseMemberFeedback;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if ( updateState != null ) {
			updateState ();
			timeInState += Time.deltaTime;
		}

		if (Input.GetKeyDown (KeyCode.M))
			WinFight ();
	}

	public void StartCombat () {
		ChangeState (States.CombatStart);
	}

	#region CombatStart
	private void CombatStart_Start () {

			// init
		firstTurn = true;

		SetTargetCrew(Crews.Side.Player);

			// create enemy crew
		if ( Crews.enemyCrew.CrewMembers.Count == 0 )
			Crews.enemyCrew.CreateRandomCrew ();

	}
	private void CombatStart_Update () {
		if ( timeInState > 0 ) {
			ChangeState (States.CrewPlacement);
		}
	}
	private void CombatStart_Exit () {
		//
	}
	#endregion

	#region CrewPlacement
	private void CrewPlacement_Start () {
		
		Crews.getCrew(targetCrew).UpdateCrew ( Crews.PlacingType.Combat );

	}
	private void CrewPlacement_Update () {
		
		if ( timeInState > Crews.getCrew(targetCrew).PlacingDuration ) {

			if (targetCrew == Crews.Side.Player) {
				
				// place enemy crew
				SetTargetCrew(Crews.Side.Enemy);;
				ChangeState (States.CrewPlacement);
				
			} else {

				// enemy chooses crew member
				ChangeState (States.MemberChoice);

			}
		}
	}
	private void CrewPlacement_Exit () {

	}
	#endregion

	#region MemberChoice
	private void MemberChoice_Start () {
		if (targetCrew == Crews.Side.Enemy) {
			MemberChoice_Enemy ();
		} else {
			MemberChoice_Player ();
		}
	}
	private void MemberChoice_Enemy () {
		
		int newIndex = Random.Range (0, Crews.enemyCrew.CrewMembers.Count);

		if (firstTurn == false) {
			if (Crews.enemyCrew.CrewMembers.Count > 1) {

				// not the same member as previously
				while (newIndex == getMember (Crews.Side.Enemy).GetIndex) {
					newIndex = Random.Range (0, Crews.enemyCrew.CrewMembers.Count);
				}
			}
		}

		// set random member
		setMember (Crews.Side.Enemy, Crews.enemyCrew.CrewMembers [newIndex]); 

		if (firstTurn) {
				// player chooses member
			SetTargetCrew (Crews.Side.Player);
			ChangeState (States.MemberChoice);
		} else {
				// lerp enemy player
			ChangeState (States.MemberLerp);
		}
	}
	private void MemberChoice_Player () {
		
		ChoosingMember = true;

	}
	private void MemberChoice_Update () {

	}
	private void MemberChoice_Exit () {

	}
	public void SetPlayerMember ( CrewMember member ) {

		setMember (Crews.Side.Player, member);

		ChoosingMember = false;

		ChangeState (States.MemberLerp);

	}
	public void Escape () {
		ChoosingMember = false;
		ExitFight ();
	}
	#endregion

	#region MemberLerp
	private void MemberLerp_Start () {
		
		Vector3 targetPos = Crews.getCrew (targetCrew).CrewAnchors [(int)Crews.PlacingType.SoloCombat].position;

		getMember (targetCrew).Icon.MoveToPoint ( Crews.PlacingType.SoloCombat , memberPlacement_Duration );
		
		CardManager.Instance.ShowFightingCard (targetCrew);
		
		DialogueManager.Instance.SetDialogue ("A l'abordage !", getMember (targetCrew).IconObj.transform);
		
	}
	private void MemberLerp_Update () {
		
		if ( timeInState > memberPlacement_Duration ) {

			getMember(targetCrew).Icon.ShowBody ();
			getMember(targetCrew).Icon.Overable = false;

			if ( firstTurn ) {
				if ( targetCrew == Crews.Side.Player ) {
					SetTargetCrew (Crews.Side.Enemy);
					ChangeState (States.MemberLerp);
				} else {
					SetTargetCrew(Crews.Side.Player);
					ChangeState (States.StartTurn);
				}
			} else {
				ChangeState (States.StartTurn);
			}

		}
	}
	private void MemberLerp_Exit () {

	}
	#endregion


	#region StartTurn
	private void StartTurn_Start () {

		if (firstTurn) {
			AttackingCrew = getMember (Crews.Side.Player).SpeedDice >= getMember (Crews.Side.Enemy).SpeedDice ? Crews.Side.Player : Crews.Side.Enemy;
			firstTurn = false;
		} else {
			AttackingCrew = AttackingCrew == Crews.Side.Player ? Crews.Side.Enemy : Crews.Side.Player;
		}

		if ( AttackingCrew == Crews.Side.Player )
		{
			DiceManager.Instance.ShowFeedbackDice ();
		}
		else
		{
			if (getMember(Crews.Side.Enemy).Health == 1 && Crews.enemyCrew.CrewMembers.Count > 1 ) {
				currentDiceType = DiceTypes.Speed;
			} else {
				currentDiceType = DiceTypes.Attack;
			}

			ChangeState (States.DiceThrow);
		}
	}
	private void StartTurn_Update () {
		
	}
	private void StartTurn_Exit () {
		//
	}
	public void ChooseDie (int i) {
		DiceManager.Instance.HideFeedbackDice ();

		currentDiceType = (DiceTypes)i;

		ChangeState (States.DiceThrow);
	}
	#endregion

	#region DiceThrow
	private void DiceThrow_Start () {

		DiceManager.Instance.ThrowDirection = AttackingCrew == Crews.Side.Player ? 1 : -1;

		DiceManager.Instance.ThrowDice (currentDiceType,getMember(AttackingCrew).getDiceValues[(int)currentDiceType+1]);


		// animation
		getMember(AttackingCrew).Icon.Animator.SetTrigger ("Attack");
	}
	private void DiceThrow_Update () {
		
		if ( !DiceManager.Instance.Throwing )
			ChangeState (States.Results);
	}
	private void DiceThrow_Exit () {
		
	}
	#endregion

	#region Results
	private void Results_Start () {

			// success //

//		Debug.Log ("throw result : " + DiceManager.Instance.CurrentThrow.Result (getMember(DefendingCrew).AttackDice));

		switch ( currentDiceType ) {
		case DiceTypes.Attack :

			switch ( DiceManager.Instance.CurrentThrow.Result (getMember(DefendingCrew).ConstitutionDice) ) {

			case Throw.Results.CritFailure:

				DialogueManager.Instance.SetDialogue ("Merde !", getMember(AttackingCrew).IconObj.transform);
				getMember (AttackingCrew).Info.DisplayInfo ("Fail","",Color.grey);
				//
				break;

			case Throw.Results.Failure :
				DialogueManager.Instance.SetDialogue ("Raté !", getMember(AttackingCrew).IconObj.transform);
				getMember (AttackingCrew).Info.DisplayInfo ("Fail","",Color.grey);
				//
				break;

			case Throw.Results.Success:
				DialogueManager.Instance.SetDialogue ("Aïe !", getMember(DefendingCrew).IconObj.transform);
				getMember(DefendingCrew).GetHit (getMember (attackingCrew).AttackDice);
//				getMember (AttackingCrew).Info.DisplayInfo ("SUCESS","!",Color.magenta);

				if (getMember(DefendingCrew).Health == 0) {
					SetTargetCrew (DefendingCrew);
					ChangeState (States.MemberReturn);
				}
				break;

			case Throw.Results.CritSuccess:
				DialogueManager.Instance.SetDialogue ("Aie PUTAIN !", getMember (DefendingCrew).IconObj.transform);
				int criticalDamage = Mathf.CeilToInt (getMember (attackingCrew).AttackDice * 1.5f);
				getMember (DefendingCrew).GetHit (getMember (attackingCrew).AttackDice + criticalDamage);
				getMember (AttackingCrew).Info.DisplayInfo ("CRITICAL","!",Color.magenta);

				if (getMember(DefendingCrew).Health == 0) {
					SetTargetCrew (DefendingCrew);
					ChangeState (States.MemberReturn);
				}
				break;
			}

			break;
		case DiceTypes.Speed :

			switch ( DiceManager.Instance.CurrentThrow.Result (getMember(DefendingCrew).SpeedDice) ) {
				
			case Throw.Results.CritFailure:
				
				DialogueManager.Instance.SetDialogue ("Merde !", getMember(AttackingCrew).IconObj.transform);
				//
				break;
				
			case Throw.Results.Failure :
				DialogueManager.Instance.SetDialogue ("Raté !", getMember(AttackingCrew).IconObj.transform);
				//
				break;
				
			case Throw.Results.Success:
				DialogueManager.Instance.SetDialogue ("A la prochaine !", getMember(AttackingCrew).IconObj.transform);
				SetTargetCrew (AttackingCrew);
				ChangeState(States.MemberReturn);
				//
				break;
				
			case Throw.Results.CritSuccess:
				DialogueManager.Instance.SetDialogue ("Tchao !", getMember (AttackingCrew).IconObj.transform);
				SetTargetCrew (AttackingCrew);
				ChangeState(States.MemberReturn);
				//
				break;
			}

			break;
		}
	}
	private void Results_Update () {
		if ( timeInState > resultsDuration ) {
			ChangeState (States.StartTurn);
		}
	}
	private void Results_Exit () {

	}
	#endregion

	#region MemberReturn
	private void MemberReturn_Start () {
		getMember (targetCrew).Icon.HideBody ();
		getMember (targetCrew).Icon.MoveToPoint (Crews.PlacingType.Combat, memberPlacement_Duration);

		CardManager.Instance.HideFightingCard (targetCrew);
	}
	private void MemberReturn_Update () {

		if (timeInState >= memberPlacement_Duration) {

			if (getMember (targetCrew).Health == 0) {
				getMember (targetCrew).Kill ();

				if (Crews.getCrew (targetCrew).CrewMembers.Count == 0) {
					WinFight ();
					return;
				}

			}

			ChangeState (States.MemberChoice);
		}

	}
	private void MemberReturn_Exit () {
		
	}
	#endregion

	#region win fight
	private void ExitFight () {
		CardManager.Instance.HideFightingCard (Crews.Side.Enemy);
		CardManager.Instance.HideFightingCard (Crews.Side.Player);
		IslandManager.Instance.Leave ();
		Crews.enemyCrew.Hide ();
		updateState = null;
	}
	private void WinFight () {
		OtherLoot.Instance.StartLooting ();
		ExitFight ();
	}
	#endregion

	#region StateMachine
	public void ChangeState ( States newState ) {
		previousState = currentState;
		currentState = newState;

		ExitState ();
		EnterState ();

		timeInState = 0f;

	}
	private void EnterState () {
		switch (currentState) {
		case States.CombatStart:
			updateState = CombatStart_Update;
			CombatStart_Start ();
			break;
		case States.CrewPlacement:
			updateState = CrewPlacement_Update;
			CrewPlacement_Start ();
			break;
		case States.MemberChoice:
			updateState = MemberChoice_Update;
			MemberChoice_Start ();
			break;
		case States.MemberLerp:
			updateState = MemberLerp_Update;
			MemberLerp_Start ();
			break;
		case States.StartTurn:
			updateState = StartTurn_Update;
			StartTurn_Start ();
			break;
		case States.DiceThrow:
			updateState = DiceThrow_Update;
			DiceThrow_Start ();
			break;
		case States.Results:
			updateState = Results_Update;
			Results_Start ();
			break;
		case States.MemberReturn :
			updateState = MemberReturn_Update;
			MemberReturn_Start ();
			break;
		}
	}
	private void ExitState () {
		switch (previousState) {
		case States.CombatStart:
			CombatStart_Exit ();
			break;
		case States.CrewPlacement:
			CrewPlacement_Exit();
			break;
		case States.MemberChoice:
			MemberChoice_Exit ();
			break;
		case States.MemberLerp:
			MemberLerp_Exit ();
			break;
		case States.StartTurn:
			StartTurn_Exit ();
			break;
		case States.DiceThrow:
			DiceThrow_Exit ();
			break;
		case States.Results:
			Results_Exit ();
			break;
		case States.MemberReturn :
			MemberReturn_Exit ();
			break;
		}
	}
	#endregion

	#region properties
	public States CurrentState {
		get {
			return currentState;
		}
		set {
			currentState = value;
		}
	}
	public CrewMember[] Members {
		get {
			return members;
		}
	}

	public CrewMember getMember (Crews.Side side) {
		return members[(int)side];
	}

	public void setMember ( Crews.Side crew, CrewMember member ) {
		members [(int)crew] = member;
	}

	public void SetTargetCrew ( Crews.Side side ) {
		targetCrew = side;
	}



	public Crews.Side TargetCrew {
		get {
			return targetCrew;
		}
	}

	public Crews.Side AttackingCrew {
		get {
			return attackingCrew;
		}
		set {
			attackingCrew = value;
			defendingCrew = value == Crews.Side.Enemy ? Crews.Side.Player : Crews.Side.Enemy;
		}
	}

	public Crews.Side DefendingCrew {
		get {
			return defendingCrew;
		}
		set {
			defendingCrew = value;

			attackingCrew = value == Crews.Side.Enemy ? Crews.Side.Player : Crews.Side.Enemy;
		}

	}

	public bool ChoosingMember {
		get {
			return choosingMember;
		}
		set {
			choosingMember = value;
			foreach ( CrewMember m in Crews.playerCrew.CrewMembers ) {
				m.Icon.ChoosingMember = value;
				m.Icon.Overable = value;
			}
			chooseMemberFeedback.SetActive (value);
		}
	}
	#endregion
}
