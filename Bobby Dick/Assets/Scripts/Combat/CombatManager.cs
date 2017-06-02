using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour {

	public static CombatManager Instance;

	public enum ActionType {

		Attacking,
		Fleeing,
		Guarding,
		Charming,
		Eating,

	}

	// stsates
	public enum States {
		None,
		CombatStart,
		PlayerCrewPlacement,
		EnemyCrewPlacement,
		PlayerMemberChoice,
		EnemyMemberChoice,
		PlayerMemberLerp,
		EnemyMemberLerp,
		StartTurn,
		PlayerAction,
		EnemyAction,
		Results,
		MemberReturn,
	}
	private States previousState = States.None;
	private States currentState = States.None;
	public delegate void UpdateState();
	UpdateState updateState;
	private float timeInState = 0f;

	/// <summary>
	/// states
	/// </summary>
	private bool firstTurn = false;

	private bool fighting = false;
	private bool started = false;

	public bool fightWon = false;
	public bool fightLost = false;

	[Header("States Duration")]
	[SerializeField] private float resultsDuration = 1f;

	/// <summary>
	/// The choosing member.
	/// </summary>
	[SerializeField]
	private GameObject chooseMemberFeedback;

	[Header("Sounds")]
	[SerializeField] private AudioClip escapeSound;

	/// <summary>
	/// The fighters
	/// </summary>

	private int memberIndex = 0;
	List<Fighter> fighters = new List<Fighter> ();

	[Header("Fighter Objects")]
	[SerializeField] private Fighter[] initPlayerFighters;
	[SerializeField] private Fighter[] initEnemyFighters;

	List<Fighter> currPlayerFighters = new List<Fighter>();
	List<Fighter> currEnemyFighters = new List<Fighter>();
//
	public Fighter currentFighter { get { return fighters [memberIndex]; } }
	private Fighter targetFighter;

	public Fighter TargetFighter {
		get {
			return targetFighter;
		}
		set {
			targetFighter = value;

			currentFighter.Target = targetFighter.BodyTransform;
			targetFighter.Target = currentFighter.BodyTransform;
		}
	}

	public CrewMember currentMember { get { return fighters [memberIndex].CrewMember; } }


	/// <summary>
	/// action
	/// </summary>
	[SerializeField]
	private GameObject actionFeedback;
	private ActionType actionType;



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
	#region combat beginning
	private void CombatStart_Start () {
		
		fighters.Clear ();

		currPlayerFighters.Clear ();

		currEnemyFighters.Clear ();

		for (int fighterIndex = 0; fighterIndex < Crews.playerCrew.CrewMembers.Count; fighterIndex++) {
			fighters.Add (initPlayerFighters[fighterIndex]);
			currPlayerFighters.Add (initPlayerFighters [fighterIndex]);
		}

		for (int fighterIndex = 0; fighterIndex < Crews.enemyCrew.CrewMembers.Count; fighterIndex++) {
			fighters.Add (initEnemyFighters[fighterIndex]);
			currEnemyFighters.Add (initEnemyFighters [fighterIndex]);
		}

		SortFighters ();

		ShowFighters ();

		ChangeState (States.StartTurn);
	}

	void SortFighters ()
	{
		//

	}

	private void CombatStart_Update () {}
	private void CombatStart_Exit () {}
	#endregion

	#region StartTurn
	private void StartTurn_Start () {
		States state = currentMember.Side == Crews.Side.Player ? States.PlayerMemberChoice : States.EnemyMemberChoice;
		ChangeState (state);
	}
	private void StartTurn_Update () {}
	private void StartTurn_Exit () {}
	#endregion

	#region Player Member Choice 
	private void PlayerMemberChoice_Start () {
		ChoosingEnemyTarget (true);
	}
	private void PlayerMemberChoice_Update () {}
	private void PlayerMemberChoice_Exit () {}

	public void ChoseTargetMember (Fighter fighter) {
		ChoosingEnemyTarget (false);

		TargetFighter = fighter;

		ChangeState (States.PlayerAction);
	}
	public void ChoosingEnemyTarget ( bool b ) {
		foreach ( Fighter fighter in fighters ) {
			if (fighter.CrewMember.Side == Crews.Side.Enemy)
				fighter.ChooseButton.SetActive (b);
		}
	}
	#endregion

	#region Enemy Member Choice 
	private void EnemyMemberChoice_Start () {
		int randomIndex = Random.Range (0, currPlayerFighters.Count);
		TargetFighter = currPlayerFighters [randomIndex];

		ChangeState (States.EnemyAction);
	}
	private void EnemyMemberChoice_Update () {}
	private void EnemyMemberChoice_Exit () {}
	#endregion

	#region PlayerAction
	private void PlayerAction_Start () {
		actionFeedback.SetActive (true);
	}
	private void PlayerAction_Update () {}
	private void PlayerAction_Exit () {}

	public void ChooseDie (int i) {

		actionFeedback.SetActive (false);

		actionType = (ActionType)i;

		Action_ThrowDices ();

	}
	#endregion

	#region Enemy Action
	private void EnemyAction_Start () {
		actionType = Enemy_GetAction ();
		Action_ThrowDices ();
	}
	private void EnemyAction_Update () {}
	private void EnemyAction_Exit () {}

	ActionType Enemy_GetAction ()
	{
		ActionType tmpType = ActionType.Attacking;

		float maxChanceOfCharisma = 0.45f;
		float currentChanceOfCharisma = currentMember.Charisma * currentMember.maxStat;

		if ( Random.value < currentChanceOfCharisma ) {
			tmpType = ActionType.Charming;
		}

		if ( currentMember.Health <= (float)currentMember.MaxHealth / 4f ) {

			float random = Random.value;

			if ( random <= 0.6f ) {

				tmpType = ActionType.Guarding;

				if ( random <= 0.3f ) {
					tmpType = ActionType.Fleeing;
				}
			}
		}

		return tmpType;
	}

	private void Action_ThrowDices ()
	{
		ChangeState (States.Results);
	}
	#endregion

	#region Results
	private void Results_Start () {

		switch (actionType) {
		case ActionType.Attacking:
			DiceManager.Instance.ThrowDice (DiceTypes.STR, currentMember.Strenght);
			break;
		case ActionType.Fleeing:
			DiceManager.Instance.ThrowDice (DiceTypes.DEX, currentMember.Dexterity);
			break;
		case ActionType.Guarding:
			DiceManager.Instance.ThrowDice (DiceTypes.CON, currentMember.Constitution);
			break;
		case ActionType.Charming:
			DiceManager.Instance.ThrowDice (DiceTypes.CHA, currentMember.Charisma);
			break;
		case ActionType.Eating:
			break;
		default:
			throw new System.ArgumentOutOfRangeException ();
		}
	}

	bool animated = false;

	private void Results_Update () {

		if (DiceManager.Instance.Throwing == false) {

			if (!animated) {

				switch (actionType) {
				case ActionType.Attacking:

					float maxAttack = currentMember.Attack * 1.5f;
					int currentAttack = Mathf.RoundToInt ((maxAttack * DiceManager.Instance.HighestResult) / 6f);
					if (DiceManager.Instance.HighestResult == 1) {
						currentAttack = Mathf.RoundToInt (currentMember.Attack * 0.75f);
					} else if (DiceManager.Instance.HighestResult == 6) {
						currentAttack = Mathf.RoundToInt (currentMember.Attack * 1.75f);
					}

					currentMember.CurrentAttack = currentAttack;
					currentFighter.ChangeState (Fighter.states.hit);
					break;

				case ActionType.Fleeing:
					break;
				case ActionType.Guarding:
					break;
				case ActionType.Charming:
					break;
				case ActionType.Eating:
					break;
				default:
					throw new System.ArgumentOutOfRangeException ();
				}

				animated = true;

			}


			if (timeInState > resultsDuration) {
				
			}
		} else {
			timeInState = 0f;
		}
	}
	private void Results_Exit () {
		animated = false;
	}
	public void NextTurn () {
		++MemberIndex;
		ChangeState (States.StartTurn);
	}
	#endregion

	#region attack
	private void Attack () {

	}
	#endregion

	#region run away
	private void RunAway () {
	
	}
	#endregion

	#region fight end
	private void ExitFight () {


	}
	public void WinFight () {

		int po = Crews.enemyCrew.ManagedCrew.Value * (int)Random.Range ( 10 , 15 );
		string phrase = "Il avait " + po + " pièces d'or";
		DialogueManager.Instance.SetDialogue (phrase, Crews.playerCrew.captain);

		GoldManager.Instance.GoldAmount += po;

		LootManager.Instance.setLoot ( Crews.Side.Enemy, LootManager.Instance.GetIslandLoot(ItemLoader.allCategories));
		OtherLoot.Instance.StartLooting ();
		Fighting = false;
	}

	public void Escape () {

		DialogueManager.Instance.SetDialogue ("Espece de lache !", targetFighter.CrewMember );

		SoundManager.Instance.PlaySound (escapeSound);

		Fighting = false;

		StoryLauncher.Instance.PlayingStory = false;

	}
	#endregion

	#region fighters
	private void ShowFighters () {

		Crews.getCrew(Crews.Side.Player).UpdateCrew ( Crews.PlacingType.Hidden );
		Crews.getCrew(Crews.Side.Enemy).UpdateCrew ( Crews.PlacingType.Hidden );

		for (int fighterIndex = 0; fighterIndex < Crews.playerCrew.CrewMembers.Count; fighterIndex++) {
			initPlayerFighters [fighterIndex].gameObject.SetActive (true);
			initPlayerFighters [fighterIndex].GetComponentInChildren<Fight_LoadSprites> ().UpdateOrder (fighterIndex);
			initPlayerFighters [fighterIndex].GetComponentInChildren<Fight_LoadSprites> ().UpdateSprites (Crews.playerCrew.CrewMembers [fighterIndex].MemberID);
			initPlayerFighters [fighterIndex].CrewMember = Crews.playerCrew.CrewMembers [fighterIndex];
			initPlayerFighters [fighterIndex].ChangeState (Fighter.states.none);
		}

		for (int fighterIndex = 0; fighterIndex < Crews.enemyCrew.CrewMembers.Count; fighterIndex++) {
			initEnemyFighters [fighterIndex].gameObject.SetActive (true);
			initEnemyFighters [fighterIndex].GetComponentInChildren<Fight_LoadSprites> ().UpdateOrder (fighterIndex);
			initEnemyFighters [fighterIndex].GetComponentInChildren<Fight_LoadSprites> ().UpdateSprites (Crews.enemyCrew.CrewMembers [fighterIndex].MemberID);
			initEnemyFighters [fighterIndex].CrewMember = Crews.enemyCrew.CrewMembers [fighterIndex];
			initEnemyFighters [fighterIndex].ChangeState (Fighter.states.none);
		}
	}
	public void DeleteFighter (Fighter fighter) {
		fighters.Remove (fighter);
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
		case States.StartTurn:
			updateState = StartTurn_Update;
			StartTurn_Start ();
			break;
		case States.PlayerAction:
			updateState = PlayerAction_Update;
			PlayerAction_Start ();
			break;
		case States.PlayerMemberChoice:
			updateState = PlayerMemberChoice_Update;
			PlayerMemberChoice_Start ();
			break;
		case States.EnemyMemberChoice:
			updateState = EnemyMemberChoice_Update;
			EnemyMemberChoice_Start ();
			break;
		case States.Results:
			updateState = Results_Update;
			Results_Start ();
			break;
		}
	}
	private void ExitState () {
		switch (previousState) {
		case States.CombatStart:
			CombatStart_Exit ();
			break;
		case States.PlayerMemberChoice:
			PlayerMemberChoice_Exit ();
			break;
		case States.StartTurn:
			StartTurn_Exit ();
			break;
		case States.PlayerAction:
			PlayerAction_Exit ();
			break;
		case States.EnemyAction:
			EnemyAction_Exit ();
			break;
		case States.Results:
			Results_Exit ();
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

	public bool Fighting {
		get {
			return fighting;
		}
		set {
			fighting = value;

			PlayerLoot.Instance.InventoryButton.Locked = fighting;

			if (fighting) {
				
				ChangeState (States.CombatStart);

			} else {

				Crews.playerCrew.UpdateCrew (Crews.PlacingType.Map);

				Crews.enemyCrew.Hide ();

				updateState = null;
			}
		}
	}

	public int MemberIndex {
		get {
			return memberIndex;
		}
		set {

			if ( value >= fighters.Count )
				value = 0;

			if ( value < 0 )
				value = fighters.Count-1;

			memberIndex = value;

		}
	}
	#endregion

}
