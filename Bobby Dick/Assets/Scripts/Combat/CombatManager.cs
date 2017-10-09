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

	// stsatesyes ?
	public enum States {
		None,

		CombatStart,
		PlayerMemberChoice,
		EnemyMemberChoice,
		StartTurn,
		PlayerAction,
		EnemyAction,

		Attacking,
		Fleeing,
		Guarding,
		Charming,
		Eating,
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
	[SerializeField] private GameObject playerFighters_Parent;
	[SerializeField] private GameObject enemyFighters_Parent;
	private Fighter[] initPlayerFighters;
	private Fighter[] initEnemyFighters;

	List<Fighter> currPlayerFighters = new List<Fighter>();
	List<Fighter> currEnemyFighters = new List<Fighter>();
//
	public Fighter currentFighter {
		get {

			if (memberIndex >= fighters.Count) {
				Debug.Log ("ATTANTEN : out of range / member index : " + memberIndex + " fighters : " + fighters.Count);
				return fighters [0];
			}
			return fighters [memberIndex];
		}
	}

	public CrewMember currentMember { get { return fighters [memberIndex].CrewMember; } }

	int crewValue = 0;


	/// <summary>
	/// action
	/// </summary>
	[SerializeField]
	private GameObject actionFeedback;
	private ActionType actionType;
	[SerializeField]
	private CategoryContent categoryFightContent;

	// EVENTS
	public delegate void FightStarting ();
	public FightStarting fightStarting;

	public delegate void FightEnding ();
	public FightEnding fightEnding;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {

		enemyFighters_Parent.SetActive (true);
		playerFighters_Parent.SetActive (true);
		initEnemyFighters = enemyFighters_Parent.GetComponentsInChildren<Fighter> (true);
		initPlayerFighters = playerFighters_Parent.GetComponentsInChildren<Fighter> (true);

		StoryFunctions.Instance.getFunction += HandleGetFunction;

		LootUI.useInventory += HandleUseInventory;
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.LaunchCombat:
			StartFight ();
			break;

		}
	}

	// Update is called once per frame
	void Update () {
		if ( updateState != null ) {
			updateState ();
			timeInState += Time.deltaTime;
		}


	}
	#region Combat Start
	private void CombatStart_Start () {

		foreach (CrewMember member in Crews.enemyCrew.CrewMembers)
			member.Icon.overable = true;

		SortFighters ();
		ShowFighters ();

		fightStarting ();

		CrewMember.selectedMember = currentMember;

		ChangeState (States.StartTurn);
	}

	void SortFighters ()
	{
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
	}

	private void CombatStart_Update () {}
	private void CombatStart_Exit () {}
	#endregion

	#region StartTurn
	private void StartTurn_Start () {

		if ( currentFighter.TurnsToSkip > 0 ) {

			currentFighter.TurnsToSkip--;

			if (currentFighter.TurnsToSkip == 0) {
				currentFighter.CombatFeedback.Display ("DEBOUT !");
				//
			} else {
				currentFighter.CombatFeedback.Display ("Encore\n" + currentFighter.TurnsToSkip + " Tours");
			}

			return;
		}

		currentFighter.ChangeState (Fighter.states.none);

		currentFighter.ChooseButton.SetActive (true);

		CardManager.Instance.ShowFightingCard (currentFighter.CrewMember);


	}
	private void StartTurn_Update () {

		if (timeInState > 0f) {

			if (currentFighter.TurnsToSkip > 0) {
				NextTurn ();
				return;
			}

			States state = currentMember.side == Crews.Side.Player ? States.PlayerAction : States.EnemyAction;
//			States state = currentMember.Side == Crews.Side.Player ? States.PlayerMemberChoice : States.EnemyMemberChoice;
			ChangeState (state);
		}

	}
	private void StartTurn_Exit () {}
	public void NextTurn () {

		CheckMembers ();

		if (currEnemyFighters.Count == 0) {
			return;
		}
		if (currPlayerFighters.Count == 0) {
			return;
		}

		foreach (Fighter fighter in fighters) {
			fighter.ChooseButton.SetActive (false);
		}

		++MemberIndex;
		ChangeState (States.StartTurn);
	}

	void CheckMembers ()
	{
		if ( currPlayerFighters.Count == 0 ) {

			if (Crews.getCrew (Crews.Side.Player).CrewMembers.Count == 0) {
				StopFight ();
				GameManager.Instance.GameOver (1);
			} else {
				Escape ();
			}
			return;
		}

		if ( currEnemyFighters.Count == 0 ) {
			if (Crews.getCrew (Crews.Side.Enemy).CrewMembers.Count == 0) {
				WinFight ();
			} else {
				Narrator.Instance.ShowNarratorTimed ("L'équipe adverse s'enfuit");
				Escape ();
			}
			return;
		}
	}
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

		switch (actionType) {
		case ActionType.Attacking:
			ChangeState (States.PlayerMemberChoice);
			break;
		case ActionType.Fleeing:
			StartAction ();
			break;
		case ActionType.Guarding:
			StartAction ();
			break;
		case ActionType.Charming:
			ChangeState (States.PlayerMemberChoice);
			break;
		case ActionType.Eating:
			StartAction ();
			break;
		default:
			throw new System.ArgumentOutOfRangeException ();
		}


	}
	#endregion

	#region Enemy Action
	private void EnemyAction_Start () {
		
		actionType = Enemy_GetAction ();

		switch (actionType) {
		case ActionType.Attacking:
			ChangeState (States.EnemyMemberChoice);
			break;
		case ActionType.Fleeing:
			StartAction ();
			break;
		case ActionType.Guarding:
			StartAction ();
			break;
		case ActionType.Charming:
			ChangeState (States.EnemyMemberChoice);
			break;
		case ActionType.Eating:
			break;
		default:
			throw new System.ArgumentOutOfRangeException ();
		}
	}
	private void EnemyAction_Update () {}
	private void EnemyAction_Exit () {}

	private ActionType Enemy_GetAction ()
	{
		ActionType tmpType = ActionType.Attacking;

//		float maxChanceOfCharisma = 0.45f;
//		float currentChanceOfCharisma = (currentMember.GetStat(Stat.Charisma) * maxChanceOfCharisma) / currentMember.maxStat;
//		if ( Random.value < currentChanceOfCharisma )
//			tmpType = ActionType.Charming;

		if ( currentMember.Health <= (float)currentMember.MemberID.maxHealth / 4f ) {

			float random = Random.value;

			if ( random <= 0.4f ) {

				tmpType = ActionType.Guarding;

				if ( random <= 0.2f ) {
					tmpType = ActionType.Fleeing;
				}
			}
		}

		return tmpType;
	}

	#endregion

	#region Player Member Choice 
	private void PlayerMemberChoice_Start () {
		ChoosingEnemyTarget (true);
	}
	private void PlayerMemberChoice_Update () {}
	private void PlayerMemberChoice_Exit () {}

	public void ChoseTargetMember (Fighter fighter) {

		ChoosingEnemyTarget (false);

		currentFighter.TargetFighter = fighter;
		currentFighter.TargetFighter.TargetFighter = currentFighter;

		CardManager.Instance.ShowFightingCard (fighter.CrewMember);

		StartAction ();
	}
	public void ChoosingEnemyTarget ( bool b ) {
		foreach ( Fighter fighter in fighters ) {
			if (fighter.CrewMember.side == Crews.Side.Enemy) {
				
				fighter.ChooseButton.SetActive (b);
				fighter.CrewMember.Icon.overable = b;
				fighter.ChooseButton.GetComponent<Button> ().interactable = b;
			}
		}
	}
	#endregion

	#region Enemy Member Choice 
	private void EnemyMemberChoice_Start () {
		
		int randomIndex = Random.Range (0, currPlayerFighters.Count);
		currentFighter.TargetFighter = currPlayerFighters [randomIndex];
		currentFighter.TargetFighter.TargetFighter = currentFighter;

		CardManager.Instance.ShowFightingCard (currPlayerFighters [randomIndex].CrewMember);
		currPlayerFighters [randomIndex].ChooseButton.SetActive (true);

		StartAction ();
	}
	private void EnemyMemberChoice_Update () {}
	private void EnemyMemberChoice_Exit () {}
	#endregion

	#region Throw Dice
	private void StartAction () {
		switch (actionType) {
		case ActionType.Attacking:
			ChangeState (States.Attacking);
			break;
		case ActionType.Fleeing:
			ChangeState (States.Fleeing);
			break;
		case ActionType.Guarding:
			ChangeState (States.Guarding);
			break;
		case ActionType.Charming:
			ChangeState (States.Charming);
			break;
		case ActionType.Eating:
			ChangeState (States.Eating);
			break;
		default:
			throw new System.ArgumentOutOfRangeException ();
		}
	}
	#endregion

	#region attacking
	private void Attacking_Start () {
		currentFighter.ChangeState (Fighter.states.moveToTarget);
	}
	private void Attacking_Update () {

		if (currentFighter.CurrentState != Fighter.states.hit) {
			
			if (currentFighter.ReachedTarget && currentFighter.TargetFighter.BackToInitPos) {
				Attack ();
				timeInState = 0f;
			}

			if ( timeInState > 5 ) {
				print ("C'est un peu long non ?");
				print ("le fighter a atteind sa cible ? " + currentFighter.ReachedTarget);
				print ("la cible du fighter est a sa position initiale ? " + currentFighter.TargetFighter.BackToInitPos);

				Attack ();
				timeInState = 0f;
			}

		} else {

			if (timeInState > currentFighter.Hit_TimeToEnableCollider) {
				currentFighter.TargetFighter.GetHit (currentFighter);
				NextTurn ();
			}

		}
	}
	private void Attacking_Exit () {
		//
	}
	private void Attack () {

		int diceValue = DiceManager.Instance.QuickThrow (currentFighter.CrewMember.GetStat(Stat.Strenght));
		float maxAttack = currentMember.Attack * 1.5f;
		int currentAttack = Mathf.RoundToInt ((maxAttack * diceValue) / 6f);

		if (diceValue == 1) {
			currentFighter.CombatFeedback.Display ("Crit\nFail",Color.blue);
			currentAttack = Mathf.RoundToInt (currentMember.Attack * 0.25f);
		} else if (diceValue == 6) {
			currentFighter.CombatFeedback.Display ("Crit\nSucc",Color.magenta);
			currentAttack = Mathf.RoundToInt (currentMember.Attack * 1.75f);
		}

		currentMember.currentAttack = currentAttack;
		currentFighter.ChangeState (Fighter.states.hit);
	}
	#endregion

	#region fleeing
	private void Fleeing_Start () {
		DiceManager.Instance.ThrowDice (DiceTypes.DEX, currentMember.GetStat(Stat.Dexterity));
	}
	private void Fleeing_Update () {
		if (DiceManager.Instance.Throwing == false) {
			Flee ();
			NextTurn ();
		}
	}
	private void Fleeing_Exit () {
		//
	}
	private void Flee () {

		if ( DiceManager.Instance.HighestResult >= 5 ) {

			currentFighter.Hide ();
			currentFighter.CombatFeedback.Display("Sucess!");

			DeleteFighter (currentFighter);

		} else if ( DiceManager.Instance.HighestResult == 1 ) {
			
			currentFighter.CombatFeedback.Display("Crit\nFail!");
			currentFighter.TurnsToSkip += 1;
		} else {
			currentFighter.CombatFeedback.Display("Fail!");
		}

	}
	#endregion

	#region guarding
	private void Guarding_Start () {
		Guard ();
		NextTurn ();
	}
	private void Guarding_Update () {
		//
	}
	private void Guarding_Exit () {
		//
	}
	private void Guard ()
	{
		currentFighter.ChangeState (Fighter.states.guard);
	}
	#endregion

	#region charming
	private void Charming_Start () {
		currentFighter.Speak ("T'as de beaux yeux");
		DiceManager.Instance.ThrowDice (DiceTypes.CHA, currentMember.GetStat(Stat.Charisma));
	}
	private void Charming_Update () {
		if (DiceManager.Instance.Throwing == false) {
			Charm ();
			NextTurn ();
		}
	}
	private void Charming_Exit () {
		//
	}
	private void Charm () {
		
		if (DiceManager.Instance.HighestResult >= 5) {
			
			currentFighter.TargetFighter.Speak ("Je peux pas me battre contre lui !");
			DeleteFighter (currentFighter.TargetFighter);

		} else if ( DiceManager.Instance.HighestResult == 1 ) {

			currentFighter.TargetFighter.Speak ("N'essaye pas de nous embobiner !");
//			currentFighter.CombatFeedback.Display ("Crit. Fail.");
			foreach ( Fighter fighter in currEnemyFighters ) {
				fighter.TurnsToSkip = 0;
			}

		} else {

			currentFighter.TargetFighter.Speak ("Qu'est-ce que t'as dis ?");
//			currentFighter.CombatFeedback.Display ("Raté");

		}
	}
	#endregion

	#region eating
	private void Eating_Start () {
		eatingMenuButton.SetActive (true);
		CrewMember.setSelectedMember(currentFighter.CrewMember);
		LootUI.Instance.Show (CategoryContentType.Combat,Crews.Side.Player);
	}
	private void Eating_Update () {
	}
	private void Eating_Exit () {
		LootUI.Instance.Hide ();
		eatingMenuButton.SetActive (false);
	}
	void HandleUseInventory (InventoryActionType actionType)
	{
		if (fighting) {
			
			if (actionType == InventoryActionType.Eat) {
				NextTurn ();
			}
		}
		
	}
	public GameObject eatingMenuButton;
	public void CloseEatingMenu () {
		ChangeState (States.PlayerAction);
	}
	#endregion

	#region fight end
	void StopFight ()
	{
		fightEnding ();
		Crews.enemyCrew.UpdateCrew (Crews.PlacingType.Hidden);
		ChangeState (States.None);

		CardManager.Instance.HideFightingCards ();
		HideFighters (Crews.Side.Player);
		HideFighters (Crews.Side.Enemy);

	}
	public void WinFight ( float delay ) {
		Invoke("WinFight",delay);
	}
	public void WinFight () {

		StopFight ();

		int po = crewValue * Random.Range (10, 20);

		string phrase = "Il avait " + po + " pièces d'or";
		DialogueManager.Instance.SetDialogue (phrase, CombatManager.Instance.currPlayerFighters[0].dialogueAnchor);

		GoldManager.Instance.GoldAmount += po;

		Invoke ("ShowLoot", 1);
	}

	void ShowLoot () {
		OtherInventory.Instance.StartLooting ();
	}

	public void Escape () {

		StopFight ();

		EndFight ();

		StoryLauncher.Instance.EndStory ();

		SoundManager.Instance.PlaySound (escapeSound);

	}
	public void EndFight () {

		fighting = false;

		CardManager.Instance.HideFightingCards ();

		Crews.playerCrew.UpdateCrew (Crews.PlacingType.Map);
		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);

	}
	#endregion

	#region fighters
	private void ShowFighters () {

		foreach ( Crews.Side side in Crews.Instance.Sides ) {
			
			Crews.getCrew(side).UpdateCrew ( Crews.PlacingType.Hidden );

			Fighter[] fighters = side == Crews.Side.Player ? initPlayerFighters : initEnemyFighters;

			for (int fighterIndex = 0; fighterIndex < Crews.getCrew(side).CrewMembers.Count; fighterIndex++) {
				
				fighters [fighterIndex].Init (Crews.getCrew(side).CrewMembers[fighterIndex],fighterIndex);
			}
		}

		foreach (CrewMember mem in Crews.enemyCrew.CrewMembers)
			crewValue += mem.Level;

		CardManager.Instance.ShowFightingCard (currPlayerFighters [0].CrewMember);
		CardManager.Instance.ShowFightingCard (currEnemyFighters [0].CrewMember);
	}
	private void HideFighters (Crews.Side side) {

		Fighter[] fighters = side == Crews.Side.Player ? initPlayerFighters : initEnemyFighters;

		foreach (Fighter f in fighters)
			f.Hide ();
	}
	public void DeleteFighter (Fighter fighter) {
		fighters.Remove (fighter);
		if ( fighter.CrewMember.side == Crews.Side.Player)
			currPlayerFighters.Remove (fighter);
		else
			currEnemyFighters.Remove (fighter);
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
		case States.EnemyAction:
			updateState = EnemyAction_Update;
			EnemyAction_Start ();
			break;
		case States.PlayerMemberChoice:
			updateState = PlayerMemberChoice_Update;
			PlayerMemberChoice_Start ();
			break;
		case States.EnemyMemberChoice:
			updateState = EnemyMemberChoice_Update;
			EnemyMemberChoice_Start();
			break;


			// action
		case States.Attacking:
			updateState = Attacking_Update;
			Attacking_Start ();
			break;
		case States.Fleeing:
			updateState = Fleeing_Update;
			Fleeing_Start ();
			break;
		case States.Guarding:
			updateState = Guarding_Update;
			Guarding_Start ();
			break;
		case States.Charming:
			updateState = Charming_Update;
			Charming_Start ();
			break;
		case States.Eating:
			updateState = Eating_Update;
			Eating_Start ();
			break;
			//

		case States.None:
			updateState = null;
			break;
		}
	}
	private void ExitState () {
		switch (previousState) {
		case States.CombatStart:
			CombatStart_Exit ();
			break;
		case States.StartTurn:
			StartTurn_Exit ();
			break;
		case States.PlayerAction:
			PlayerAction_Exit();
			break;
		case States.EnemyAction:
			EnemyAction_Exit();
			break;
		case States.PlayerMemberChoice:
			PlayerMemberChoice_Exit();
			break;
		case States.EnemyMemberChoice:
			EnemyMemberChoice_Exit();
			break;

			// actions
		case States.Attacking:
			Attacking_Exit ();
			break;
		case States.Fleeing:
			Fleeing_Exit ();
			break;
		case States.Guarding:
			Guarding_Exit ();
			break;
		case States.Charming:
			Charming_Exit ();
			break;
		case States.Eating:
			Eating_Exit ();
			break;
			//

		case States.None:
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

	public void StartFight () {

		Crews.enemyCrew.managedCrew.hostile = true;

		fighting = true;

		ChangeState (States.CombatStart);

	}



	public bool Fighting {
		get {
			return fighting;
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
