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
		ThrowDice,
		LaunchAction,
	}
	private States previousState = States.None;
	private States currentState = States.None;
	public delegate void UpdateState();
	UpdateState updateState;
	private float timeInState = 0f;

	private float timeBetweenSteps = 0f;

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
	public Fighter currentFighter { get { return fighters [memberIndex]; } }

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
		initEnemyFighters = enemyFighters_Parent.GetComponentsInChildren<Fighter> (true);
		initPlayerFighters = playerFighters_Parent.GetComponentsInChildren<Fighter> (true);
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

		foreach (CrewMember member in Crews.enemyCrew.CrewMembers)
			member.Icon.Overable = true;

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

		if ( currentFighter.SkipNextTurn) {

			currentFighter.CombatFeedback.Display ("Skip\nTurn");
			return;
		}

		currentFighter.ChooseButton.SetActive (true);
		CardManager.Instance.ShowFightingCard (currentFighter.CrewMember);


	}
	private void StartTurn_Update () {

		if (timeInState > timeBetweenSteps) {

			if (currentFighter.SkipNextTurn) {
				currentFighter.SkipNextTurn = true;
				NextTurn ();
				return;
			}

			States state = currentMember.Side == Crews.Side.Player ? States.PlayerAction : States.EnemyAction;
//			States state = currentMember.Side == Crews.Side.Player ? States.PlayerMemberChoice : States.EnemyMemberChoice;
			ChangeState (state);
		}

	}
	private void StartTurn_Exit () {}
	public void NextTurn () {

		currentFighter.ChooseButton.SetActive (false);

		++MemberIndex;
		ChangeState (States.StartTurn);
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
			ChangeState (States.ThrowDice);
			break;
		case ActionType.Guarding:
			ChangeState (States.ThrowDice);
			break;
		case ActionType.Charming:
			ChangeState (States.PlayerMemberChoice);
			break;
		case ActionType.Eating:
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
			ChangeState (States.ThrowDice);
			break;
		case ActionType.Guarding:
			ChangeState (States.ThrowDice);
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
//		float currentChanceOfCharisma = currentMember.Charisma * currentMember.maxStat;
//
//		if ( Random.value < currentChanceOfCharisma ) {
//			tmpType = ActionType.Charming;
//		}
//
//		if ( currentMember.Health <= (float)currentMember.MaxHealth / 4f ) {
//
//			float random = Random.value;
//
//			if ( random <= 0.6f ) {
//
//				tmpType = ActionType.Guarding;
//
//				if ( random <= 0.3f ) {
//					tmpType = ActionType.Fleeing;
//				}
//			}
//		}

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

		ChangeState (States.ThrowDice);
	}
	public void ChoosingEnemyTarget ( bool b ) {
		foreach ( Fighter fighter in fighters ) {
			if (fighter.CrewMember.Side == Crews.Side.Enemy) {
				fighter.ChooseButton.SetActive (b);
				fighter.CrewMember.Icon.Overable = b;
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

		ChangeState (States.ThrowDice);
	}
	private void EnemyMemberChoice_Update () {}
	private void EnemyMemberChoice_Exit () {}
	#endregion

	#region Throw Dice
	private void ThrowDice_Start () {
		switch (actionType) {
		case ActionType.Attacking:
			currentFighter.ChangeState (Fighter.states.moveToTarget);
			break;
		case ActionType.Fleeing:
			DiceManager.Instance.ThrowDice (DiceTypes.DEX, currentMember.Dexterity);
			break;
		case ActionType.Guarding:
//			DiceManager.Instance.ThrowDice (DiceTypes.CON, currentMember.Constitution);
			break;
		case ActionType.Charming:
			currentFighter.Speak ("T'as de beaux yeux");
			DiceManager.Instance.ThrowDice (DiceTypes.CHA, currentMember.Charisma);
			break;
		case ActionType.Eating:
			break;
		default:
			throw new System.ArgumentOutOfRangeException ();
		}
	}

	private void ThrowDice_Update () {

		if ( DiceManager.Instance.Throwing == false) {
			
			switch (actionType) {
			case ActionType.Attacking:
				if (currentFighter.ReachedTarget)
					ChangeState (States.LaunchAction);
				break;
			case ActionType.Fleeing:
					ChangeState (States.LaunchAction);
				break;
			case ActionType.Guarding:
				ChangeState (States.LaunchAction);
				break;
			case ActionType.Charming:
				ChangeState (States.LaunchAction);
				break;
			case ActionType.Eating:
				break;
			default:
				throw new System.ArgumentOutOfRangeException ();
			}

		}




	}
	private void ThrowDice_Exit () {
	}
	#endregion

	#region Launch Action
	private void LaunchAction_Start () {
		switch (actionType) {
		case ActionType.Attacking:
			Attack ();
			break;
		case ActionType.Fleeing:
			Flee ();
			break;
		case ActionType.Guarding:
			Guard ();
			break;
		case ActionType.Charming:
			Charm ();
			break;
		case ActionType.Eating:
			break;
		default:
			throw new System.ArgumentOutOfRangeException ();
		}
	}

	private void LaunchAction_Update () {
		if ( timeInState > timeBetweenSteps) {
			NextTurn ();
		}
	}
	private void LaunchAction_Exit () {
		CardManager.Instance.HideFightingCards ();
	}
	#endregion

	#region actions
	private void Attack () {

		int diceValue = DiceManager.Instance.QuickThrow (currentFighter.CrewMember.Strenght);
		float maxAttack = currentMember.Attack * 1.5f;
		int currentAttack = Mathf.RoundToInt ((maxAttack * diceValue) / 6f);

		if (diceValue == 1) {
			currentFighter.CombatFeedback.Display ("Crit\nFail",Color.blue);
			currentAttack = Mathf.RoundToInt (currentMember.Attack * 0.25f);
		} else if (diceValue == 6) {
			currentFighter.CombatFeedback.Display ("Crit\nSucc",Color.magenta);
			currentAttack = Mathf.RoundToInt (currentMember.Attack * 1.75f);
		}

		currentMember.CurrentAttack = currentAttack;
		currentFighter.ChangeState (Fighter.states.hit);
	}
	private void Flee () {

		if ( DiceManager.Instance.HighestResult >= 5 ) {

			currentFighter.Hide ();
			DeleteFighter (currentFighter);

			currentFighter.CombatFeedback.Display("Sucess!");

		} if ( DiceManager.Instance.HighestResult == 1 ) {
			
			currentFighter.CombatFeedback.Display("Crit\nFail!");
			currentFighter.SkipNextTurn = true;
		} else {
			
			currentFighter.CombatFeedback.Display("Fail!");
		}

	}

	void Guard ()
	{
		currentFighter.ChangeState (Fighter.states.guard);
	}

	private void Charm () {
		if (DiceManager.Instance.HighestResult >= 5) {
			
			currentFighter.CombatFeedback.Display ("Succ");
			currentFighter.TargetFighter.Speak ("Ah, merci, c'est gentil");
			currentFighter.TargetFighter.SkipNextTurn = true;
		} else if ( DiceManager.Instance.HighestResult == 1 ) {
			currentFighter.TargetFighter.Speak ("Il est con ce mec...");
			currentFighter.CombatFeedback.Display ("Crit\nFail");
			foreach ( Fighter fighter in currEnemyFighters ) {
				fighter.SkipNextTurn = false;
			}
		} else {
			currentFighter.TargetFighter.Speak ("Quoi ?");
			currentFighter.CombatFeedback.Display ("Oups");

		}
	}
	#endregion

	#region fight end
	public void WinFight ( float delay ) {
		Invoke("WinFight",delay);
	}
	public void WinFight () {

		int po = Crews.enemyCrew.ManagedCrew.Value * (int)Random.Range ( 10 , 15 );
		string phrase = "Il avait " + po + " pièces d'or";
		DialogueManager.Instance.SetDialogue (phrase, CombatManager.Instance.currPlayerFighters[0].dialogueAnchor);

		GoldManager.Instance.GoldAmount += po;

		CardManager.Instance.HideFightingCards ();

		Invoke ("ShowLoot", 1);
	}

	void ShowLoot () {
		LootManager.Instance.setLoot ( Crews.Side.Enemy, LootManager.Instance.GetIslandLoot(ItemLoader.allCategories));
		OtherLoot.Instance.StartLooting ();

	}

	public void Escape () {

		DialogueManager.Instance.SetDialogue ("Espece de lache !", currentFighter.TargetFighter.CrewMember );

		SoundManager.Instance.PlaySound (escapeSound);

		Fighting = false;

		StoryLauncher.Instance.PlayingStory = false;

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
	}
	private void HideFighters () {
		foreach ( Crews.Side side in Crews.Instance.Sides ) {
			Crews.getCrew(side).UpdateCrew ( Crews.PlacingType.Map );

			Fighter[] fighters = side == Crews.Side.Player ? initPlayerFighters : initEnemyFighters;

			for (int fighterIndex = 0; fighterIndex < Crews.getCrew(side).CrewMembers.Count; fighterIndex++) {
				fighters [fighterIndex].Hide ();
			}
		}
	}
	public void DeleteFighter (Fighter fighter) {
		fighters.Remove (fighter);
		if ( fighter.CrewMember.Side == Crews.Side.Player)
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
		case States.ThrowDice:
			updateState = ThrowDice_Update;
			ThrowDice_Start ();
			break;
		case States.LaunchAction:
			updateState = LaunchAction_Update;
			LaunchAction_Start ();
			break;
		case States.None:
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
		case States.ThrowDice:
			ThrowDice_Exit();
			break;
		case States.LaunchAction:
			LaunchAction_Exit();
			break;
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

				HideFighters ();

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
