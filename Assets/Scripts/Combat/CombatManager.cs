using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour {

	public static CombatManager Instance;

	Crews.Side attackingCrew;
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

	[SerializeField]
	private float crewPlacement_Duration = 1f;

	[SerializeField]
	private float memberPlacement_Duration = 1f;

	private float timeInState = 0f;

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
	}

	public void StartCombat () {
		ChangeState (States.CombatStart);
	}

	#region CombatStart
	private void CombatStart_Start () {

			// init
		firstTurn = true;
		targetCrew = Crews.Side.Player;

			// create enemy crew
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
				
				targetCrew = Crews.Side.Enemy;
				
				ChangeState (States.CrewPlacement);
				
			} else {

					// player can choose his member
				foreach (CrewMember member in Crews.playerCrew.CrewMembers) {
					member.Icon.ChoosingMember = true;
				}

			}
		}
	}
	private void CrewPlacement_Exit () {

	}

	public void SetPlayerMember ( CrewMember member ) {

		targetCrew = Crews.Side.Player;

		setMember (Crews.Side.Player, member);
		
		foreach ( CrewMember m in Crews.playerCrew.CrewMembers ) {
			m.Icon.ChoosingMember = false;
		}
		
		if (firstTurn) {
			ChangeState (States.MemberChoice);
		} else {
			ChangeState (States.MemberLerp);
		}
		
	}
	#endregion

	#region MemberChoice
	private void MemberChoice_Start () {

		if (targetCrew == Crews.Side.Enemy) {

			int newIndex = Random.Range (0, Crews.enemyCrew.CrewMembers.Count);

			if ( Crews.enemyCrew.CrewMembers.Count > 1 ) {

				while (newIndex == getMember (Crews.Side.Enemy).GetIndex) {
					newIndex = Random.Range (0, Crews.enemyCrew.CrewMembers.Count);
				}
			}

			setMember (Crews.Side.Enemy, Crews.enemyCrew.CrewMembers [newIndex]); 


			if ( firstTurn )
				targetCrew = Crews.Side.Player;

			ChangeState (States.MemberLerp);
			

		} else {
			if ( firstTurn ) {
				targetCrew = Crews.Side.Enemy;
				ChangeState (States.MemberChoice);
			} else {

				foreach (CrewMember member in Crews.playerCrew.CrewMembers) {
					member.Icon.ChoosingMember = true;
				}


			}
		}
	}
	private void MemberChoice_Update () {

	}
	private void MemberChoice_Exit () {

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
					targetCrew = Crews.Side.Enemy;
					ChangeState (States.MemberLerp);
				} else {
					targetCrew = Crews.Side.Player;
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
			attackingCrew = getMember (Crews.Side.Player).SpeedDice >= getMember (Crews.Side.Enemy).SpeedDice ? Crews.Side.Player : Crews.Side.Enemy;
			firstTurn = false;
		} else {
			attackingCrew = attackingCrew == Crews.Side.Player ? Crews.Side.Enemy : Crews.Side.Player;
		}

		if ( attackingCrew == Crews.Side.Player )
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
		
		DiceManager.Instance.ThrowDirection = attackingCrew == Crews.Side.Player ? 1 : -1;

		DiceManager.Instance.ThrowDice (currentDiceType,getMember(attackingCrew).getDiceValues[(int)currentDiceType+1]);
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
		switch ( currentDiceType ) {
		case DiceTypes.Attack :

			switch ( DiceManager.Instance.CurrentThrow.Result (defendingMember.ConstitutionDice) ) {

			case Throw.Results.CritFailure:

				DialogueManager.Instance.SetDialogue ("Merde !", getMember(attackingCrew).IconObj.transform);

				break;

			case Throw.Results.Failure :
				DialogueManager.Instance.SetDialogue ("Raté !", getMember(attackingCrew).IconObj.transform);
				//
				break;

			case Throw.Results.Success:
				DialogueManager.Instance.SetDialogue ("Aïe !", defendingMember.IconObj.transform);
				defendingMember.GetHit (DiceManager.Instance.CurrentThrow.highestResult);

				break;

			case Throw.Results.CritSuccess :
				DialogueManager.Instance.SetDialogue ("Merde !", defendingMember.IconObj.transform);
				defendingMember.GetHit (DiceManager.Instance.CurrentThrow.highestResult + 1);

				break;
			}

			break;
		case DiceTypes.Speed :

			switch ( DiceManager.Instance.CurrentThrow.Result (defendingMember.SpeedDice) ) {
				
			case Throw.Results.CritFailure:
				
				DialogueManager.Instance.SetDialogue ("Merde !", getMember(attackingCrew).IconObj.transform);
				
				break;
				
			case Throw.Results.Failure :
				DialogueManager.Instance.SetDialogue ("Raté !", getMember(attackingCrew).IconObj.transform);
				//
				break;
				
			case Throw.Results.Success:
				DialogueManager.Instance.SetDialogue ("A la prochaine !", getMember(attackingCrew).IconObj.transform);
				ChangeState(States.MemberReturn);
				//
				break;
				
			case Throw.Results.CritSuccess :
				DialogueManager.Instance.SetDialogue ("Tchao !", getMember(attackingCrew).IconObj.transform);
				ChangeState(States.MemberReturn);
				//
				break;
			}

			break;
		}
	}
	private void Results_Update () {
		if ( timeInState > 1 ) {
			ChangeState (States.StartTurn);
		}
	}
	private void Results_Exit () {

	}
	#endregion

	#region MemberReturn
	private void MemberReturn_Start () {
		getMember (attackingCrew).Icon.HideBody ();
		getMember (attackingCrew).Icon.MoveToPoint (Crews.PlacingType.Combat, memberPlacement_Duration);

		CardManager.Instance.HideFightingCard (attackingCrew);
	}
	private void MemberReturn_Update () {

		if (timeInState >= memberPlacement_Duration) {

			if ( getMember(attackingCrew).Health == 0 ) {
				getMember(attackingCrew).Kill ();
			}

			targetCrew = attackingCrew;
			ChangeState (States.MemberChoice);
		}

	}
	private void MemberReturn_Exit () {

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

	public CrewMember getMember (Crews.Side attackingCrew) {
		return members[(int)attackingCrew];
	}

	public CrewMember defendingMember {
		get {
			return attackingCrew == Crews.Side.Player ? getMember (Crews.Side.Enemy) : getMember(Crews.Side.Player);
		}
	}

	public void setMember ( Crews.Side attackingCrew, CrewMember member ) {
		members [(int)attackingCrew] = member;
	}

	#endregion

}
