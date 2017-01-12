using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour {

	public static CombatManager Instance;

	Crews.Side attackingCrew;
	Crews.Side defendingCrew;
	Crews.Side targetCrew;

	public enum ActionType {

		Attacking,
		Fleeing,

	}

	// stsates
	public enum States {
		None,
		CombatStart,
		CrewPlacement,
		MemberChoice,
		MemberLerp,
		StartTurn,
		Action,
		Results,
		MemberReturn,
	}
	private States previousState = States.None;
	private States currentState = States.None;

	bool firstTurn = false;

	Quaternion lerpRot = Quaternion.identity;

	private CrewMember[] members = new CrewMember[2];

	ActionType actionType;

	public delegate void UpdateState();
	UpdateState updateState;

	[Header("States Duration")]
	[SerializeField] private float crewPlacement_Duration = 1f;
	[SerializeField] private float memberPlacement_Duration = 1f;
	[SerializeField] private float resultsDuration = 1f;
	[SerializeField] private float actionDuration = 0.5f;

	private float timeInState = 0f;

	bool choosingMember = false;

	int enemeyCount = 0;

	[Header("Chances")]
	[SerializeField] private float maxDodgeChance = 43f;
	[SerializeField] private float maxEscapechance = 86f;
	[SerializeField] private float maxCriticalChance = 35f;

	[Header("Sounds")]
	[SerializeField] private AudioClip escapeSound;

	[Header("Fighter Objects")]
	[SerializeField] private GameObject playerFighter;
	[SerializeField] private GameObject enemyFighter;

	bool fighting = false;

	[SerializeField]
	private GameObject chooseMemberFeedback;

	[SerializeField]
	private MemberFeedback playerFeedback;
	[SerializeField]
	private MemberFeedback enemyFeedback;

	public MemberFeedback PlayerFeedback {
		get {
			return playerFeedback;
		}
	}

	public MemberFeedback EnemyFeedback {
		get {
			return enemyFeedback;
		}
	}

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

		if (Input.GetKeyDown (KeyCode.K))
			WinFight ();
	}

	public void StartCombat () {

		PlayerLoot.Instance.InventoryButton.Locked = true;

		ChangeState (States.CombatStart);

		fighting = true;
	}

	#region CombatStart
	private void CombatStart_Start () {

			// init
		firstTurn = true;

		SetTargetCrew(Crews.Side.Player);

			// create enemy crew
		if ( Crews.enemyCrew.CrewMembers.Count == 0 ) {
			Debug.LogError ("pas d'équipage pour commencer combat");
			updateState = null;
		}

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

		DialogueManager.Instance.SetDialogue ("Espece de lache !", getMember (Crews.Side.Enemy));

		SoundManager.Instance.PlaySound (escapeSound);

		ChoosingMember = false;

//		ExitFight ();
//		StoryReader.Instance.SetDecal (3);
//		StoryReader.Instance.NextCell ();
//		StoryReader.Instance.Wait (0.5f);

		ExitFight ();
		IslandManager.Instance.Leave ();

	}
	#endregion

	#region MemberLerp
	private void MemberLerp_Start () {

		DialogueManager.Instance.SetDialogue ("A l'abordage !", getMember (targetCrew));

//		getMember(targetCrew).Icon.ShowBody ();
		getMember(targetCrew).Icon.Overable = false;
		getMember (targetCrew).Icon.HideFace ();

		if ( firstTurn ) {
			
			if ( targetCrew == Crews.Side.Player ) {
				
				SetTargetCrew (Crews.Side.Enemy);
				ShowEnemyFighter ();

				ChangeState (States.MemberLerp);

			} else {
				
				SetTargetCrew(Crews.Side.Player);
				ShowPlayerFighter ();

				ChangeState (States.StartTurn);
			}

		} else {
			
			ChangeState (States.StartTurn);

		}
		
	}

	private void ShowPlayerFighter () {
		
		getMember (targetCrew).Icon.HideFace ();
		CardManager.Instance.ShowFightingCard (targetCrew);

		playerFighter.SetActive (true);
		playerFighter.GetComponentInChildren<Fight_LoadSprites> ().UpdateSprites (getMember (targetCrew).MemberID);
		playerFighter.GetComponent<Humanoid> ().CrewMember = getMember (targetCrew);
	}

	private void ShowEnemyFighter () {

		getMember (targetCrew).Icon.HideFace ();
		CardManager.Instance.ShowFightingCard (targetCrew);

		enemyFighter.SetActive (true);
		enemyFighter.GetComponentInChildren<Fight_LoadSprites> ().UpdateSprites (getMember (targetCrew).MemberID);
		enemyFighter.GetComponent<Humanoid> ().CrewMember = getMember (targetCrew);
	}

	private void MemberLerp_Update () {
//		
		if ( timeInState > memberPlacement_Duration ) {


		}
	}
	private void MemberLerp_Exit () {

	}
	#endregion


	#region StartTurn
	private void StartTurn_Start () {


	}
	private void StartTurn_Update () {
		
	}
	private void StartTurn_Exit () {
		//
	}
	public void ChooseDie (int i) {
		HideFeedbackDice ();

		actionType = (ActionType)i;

		ChangeState (States.Action);
	}
	#endregion

	#region Action
	private void Action_Start () {
		
	}
	private void Action_Update () {
		
		if ( timeInState > actionDuration )
			ChangeState (States.Results);
	}
	private void Action_Exit () {
		
	}
	#endregion

	#region Results
	private void Results_Start () {

		switch (actionType) {
		case ActionType.Attacking:
			Attack ();
			break;
		case ActionType.Fleeing:
			RunAway ();
			break;
		}
	}

	private void RunAway () {
		float escapeChance = (maxEscapechance * getMember(attackingCrew).Dexterity) / 10;
		if (Random.value * 100 < escapeChance) {
			
			SoundManager.Instance.PlaySound (escapeSound);
			
			DialogueManager.Instance.SetDialogue ("A la prochaine !", getMember (AttackingCrew));

			SetTargetCrew (AttackingCrew);
			ChangeState (States.MemberReturn);

//			getMember (AttackingCrew).Info.DisplayInfo ("ESCAPE","!",Color.magenta);


		} else {
			
			DialogueManager.Instance.SetDialogue ("Raté !", getMember (AttackingCrew));

		}

	
	}

	private void Attack () {

		int attack = getMember (attackingCrew).Attack;

		// escape //
		float dodgeChance = (maxDodgeChance * getMember (defendingCrew).Dexterity) / 10f;
		if ( Random.value * 100 < dodgeChance ) {

			DialogueManager.Instance.SetDialogue ("Raté !", getMember(AttackingCrew));
//			getMember (AttackingCrew).Info.DisplayInfo ("Fail","",Color.grey);

			return;
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
		getMember (targetCrew).Icon.ShowFace ();

		CardManager.Instance.HideFightingCard (targetCrew);

		if (targetCrew == Crews.Side.Player)
			playerFighter.SetActive (false);
		else
			enemyFighter.SetActive (false);

		if (getMember (DefendingCrew).Health == 0) {
			getMember (DefendingCrew).Kill ();
			getMember (AttackingCrew).AddXP (getMember (targetCrew).Level * 25);

			if (Crews.getCrew (targetCrew).CrewMembers.Count == 0) {
				WinFight ();
				return;
			}

		}

		ChangeState (States.MemberChoice);
	}
	private void MemberReturn_Update () {
//
//		if (timeInState >= memberPlacement_Duration) {
//
//
//		}

	}
	private void MemberReturn_Exit () {
		
	}
	#endregion

	#region fight end
	private void ExitFight () {

		getMember (Crews.Side.Player).Icon.ShowFace ();
		playerFighter.SetActive (false);

		CardManager.Instance.HideFightingCard (Crews.Side.Enemy);
		CardManager.Instance.HideFightingCard (Crews.Side.Player);

		fighting = false;

		Crews.enemyCrew.Hide ();
		updateState = null;;
	}
	private void WinFight () {

		int po = Crews.enemyCrew.ManagedCrew.Value * (int)Random.Range ( 10 , 15 );
		string phrase = "Il avait " + po + " pièces d'or";

		GoldManager.Instance.AddGold (po);

		LootManager.Instance.setLoot ( Crews.Side.Enemy, LootManager.Instance.GetIslandLoot(ItemLoader.allCategories));
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
		case States.Action:
			updateState = Action_Update;
			Action_Start ();
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
		case States.Action:
			Action_Exit ();
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

	[SerializeField]
	private GameObject feedbackDice;

	private void ShowFeedbackDice () {
		feedbackDice.SetActive (true);
	}
	private void HideFeedbackDice () {
		feedbackDice.SetActive (false);
	}

	public bool Fighting {
		get {
			return fighting;
		}
	}
}
