using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour {

	public static CombatManager Instance;

	public bool fighting = false;

	// stsatesyes ?
	public enum States {
		
		None,

		CombatStart,
		PlayerMemberChoice,
		EnemyMemberChoice,
		StartTurn,
		PlayerActionChoice,
		EnemyActionChoice,

		PlayerAction,
		EnemyAction,

	}

	private States previousState = States.None;
	private States currentState = States.None;
	public delegate void UpdateState();
	private UpdateState updateState;
	private float timeInState = 0f;


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

	public List<Fighter> currPlayerFighters = new List<Fighter>();
	public List<Fighter> currEnemyFighters = new List<Fighter>();
	public List<Fighter> getCurrentFighters (Crews.Side side) {
		return side == Crews.Side.Player ? currPlayerFighters : currEnemyFighters;
	}
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

	public CrewMember currentMember { get { return fighters [memberIndex].crewMember; } }

	int crewValue = 0;

	/// <summary>
	/// action
	/// </summary>
	[SerializeField]
	private CategoryContent categoryFightContent;

	[Header("Sounds")]
	[SerializeField] private AudioClip escapeSound;

	// EVENTS
	public delegate void FightStarting ();
	public FightStarting onFightStart;

	public delegate void FightEnding ();
	public FightEnding onFightEnd;

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

		onFightStart ();

		CrewMember.SetSelectedMember (currentMember);

		ChangeState (States.StartTurn);
	}

	void SortFighters ()
	{
		memberIndex = 0;

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

		currentFighter.SetTurn ();

		States state = currentMember.side == Crews.Side.Player ? States.PlayerActionChoice : States.EnemyActionChoice;

		ChangeState (state);

	}
	private void StartTurn_Update () {

//		if (timeInState > 1f) {
//
//			States state = currentMember.side == Crews.Side.Player ? States.PlayerActionChoice : States.EnemyActionChoice;
//
//			ChangeState (state);
//
//		}

	}

	private void StartTurn_Exit () {}

	public void NextTurn () {

		NextMember ();

		StartNewTurn ();

	}

	void StartNewTurn () {

		if (currEnemyFighters.Count == 0) {
			print ("on continue pas parce qu'il y a plus de membres joueurs");
			return;
		}
		if (currPlayerFighters.Count == 0) {
			print ("on continue pas parce qu'il y a plus de membres adversaires");
			return;
		}

		Invoke ("StartNewTurnDelay" , 1f);
		//
	}

	void StartNewTurnDelay () {
		ChangeState (States.StartTurn);
		//
	}

	void CheckMembers ()
	{
		if ( currPlayerFighters.Count == 0 ) {

			print ("plus de combattants joueurs");

			if (Crews.getCrew (Crews.Side.Player).CrewMembers.Count == 0) {

				print ("plus de membres joueurs : donc mort");

			} else {

				print ("il reste des membres joueurs : donc fuite joueur");

				Escape ();

				Invoke ("ExitFight",1f);


			}
			return;
		}

		if ( currEnemyFighters.Count == 0 ) {

			print ("plus de combattants ennemis");

			if (Crews.getCrew (Crews.Side.Enemy).CrewMembers.Count == 0) {

				print ("plus de membres ennemis : donc victoire");

				ReceiveXp ();
				ReceiveGold ();

				Invoke ("ExitFight",3f);
				Invoke ("ShowLoot", 3f);

			} else {

				print ("il reste des membres ennemis : donc fuite ennemie");

				Narrator.Instance.ShowNarratorTimed ("L'équipe adverse s'enfuit");

				Invoke ("ExitFight",1f);
				Escape ();
//				Invoke ("Escape",1f);


			}

			return;
		}
	}
	void NoMorePlayersDelay() {
		
	
	}
	void NoMoreEnnemiesDelay() {
		

	}
	#endregion

	#region member choice
	public Skill currentSkill;
	public void GoToTargetSelection ( Crews.Side side , Skill skill ) {

		currentSkill = skill;

		if (side == Crews.Side.Player) {
			ChangeState (States.PlayerMemberChoice);
		} else {
			ChangeState (States.EnemyMemberChoice);
		}

	}
	public void ChoseTargetMember (Fighter fighter) {

		fighter.SetAsTarget ();

		ChangeState (States.PlayerAction);
	}

	public void DisablePickable () {
		foreach (Fighter fighter in getCurrentFighters (Crews.Side.Player)) {
			fighter.Pickable = false;
		}
		foreach (Fighter fighter in getCurrentFighters (Crews.Side.Enemy)) {
			fighter.Pickable = false;
		}
	}

	public void ChoosingTarget (Crews.Side side) {

		if (side == Crews.Side.Enemy) {
			
			bool provoking = false;

			foreach (Fighter fighter in getCurrentFighters (side)) {
				if ( fighter.HasStatus(Fighter.Status.Provoking) ) {
					provoking = true;
					break;
				}
			}

			if (provoking) {

				print ("y'a eu provocation apparemment, donc y'en a qui sont pas pickable ?");

				foreach (Fighter fighter in getCurrentFighters (side)) {
					if (fighter.HasStatus (Fighter.Status.Provoking)) {
						fighter.Pickable = true;
					}
				}

			} else {

				foreach (Fighter fighter in getCurrentFighters (side)) {
					fighter.Pickable = true;
				}

			}
		} else {

			foreach (Fighter fighter in getCurrentFighters (side)) {
				if (!currentSkill.canTargetSelf) {

					if ( fighter != currentFighter )
						fighter.Pickable = true;

				} else {
					fighter.Pickable = true;
				}
			}

		}
	}
	#endregion

	#region Player Action Choice
	private void PlayerActionChoice_Start () {



	}
	private void PlayerActionChoice_Update () {}
	private void PlayerActionChoice_Exit () {}
	#endregion

	#region Player Member Choice 
	public GameObject cancelPlayerMemberChoiceButton;
	private void PlayerMemberChoice_Start () {

		cancelPlayerMemberChoiceButton.SetActive (true);
		Tween.Bounce (cancelPlayerMemberChoiceButton.transform);

		if (currentSkill.targetType == Skill.TargetType.Self) {
			ChoosingTarget (Crews.Side.Player);
		} else {
			ChoosingTarget (Crews.Side.Enemy);
		}

	}
	private void PlayerMemberChoice_Update () {}
	private void PlayerMemberChoice_Exit () {
		DisablePickable ();
		cancelPlayerMemberChoiceButton.SetActive (false);

	}
	public void CancelPlayerMemberChoice () {
		ChangeState (States.PlayerActionChoice);
	}
	#endregion

	#region Player Action
	private void PlayerAction_Start () {

	}
	private void PlayerAction_Update () {}
	private void PlayerAction_Exit () {}
	#endregion

	#region Enemy Action Choice
	private void EnemyActionChoice_Start () {

		Skill skill = SkillManager.RandomSkill (currentMember);
		skill.Trigger (CombatManager.Instance.currentFighter);

	}
	private void EnemyActionChoice_Update () {}
	private void EnemyActionChoice_Exit () {}
	#endregion


	#region Enemy Member Choice 
	private void EnemyMemberChoice_Start () {

		if ( currentSkill.preferedTarget != null ) {
			currentSkill.preferedTarget.SetAsTarget ();
			currentSkill.preferedTarget = null;
			return;
		}

		if (currentSkill.targetType == Skill.TargetType.Self) {

			// attention au pledge of feast.
			List<Fighter> targetFighters = currEnemyFighters;

			if ( currentSkill.canTargetSelf == false )
				targetFighters.Remove (currentFighter);

			int randomIndex = Random.Range (0, targetFighters.Count);

			targetFighters [randomIndex].SetAsTarget ();

		} else {

			foreach (var item in currPlayerFighters) {
				if (item.HasStatus (Fighter.Status.Provoking)) {
					item.SetAsTarget ();
					return;
				}
			}

			Fighter weakestFighter = currPlayerFighters [0];
			foreach (var item in currPlayerFighters) {
				if ( item.crewMember.Health < weakestFighter.crewMember.Health ) {
					weakestFighter = item;
				}
			}

			weakestFighter.SetAsTarget ();
//			int randomIndex = Random.Range (0, currPlayerFighters.Count);
//
//			currPlayerFighters [randomIndex].SetAsTarget ();

		}

	}
	private void EnemyMemberChoice_Update () {

		if (timeInState >= 0.5f) {

			ChangeState (States.EnemyAction);

		}

	}
	private void EnemyMemberChoice_Exit () {}
	#endregion

	#region Enemy Action
	private void EnemyAction_Start () {

	}
	private void EnemyAction_Update () {}
	private void EnemyAction_Exit () {}
	#endregion

	#region loot & xp
	public void ReceiveGold () {

		int po = crewValue * Random.Range (10, 15);

		currPlayerFighters [0].combatFeedback.Display ("+ " + po + " or" ,Color.yellow);

		GoldManager.Instance.AddGold (po);

	}
	public void ReceiveXp() {

		foreach (var item in currPlayerFighters) {

			int xpPerMember = 40;

			item.combatFeedback.Display ("+ " + xpPerMember + " xp",Color.blue);

			item.crewMember.AddXP (xpPerMember);
		}

	}

	void ShowLoot () {
		OtherInventory.Instance.StartLooting ();
	}
	#endregion 

	#region fight end
	public void Escape () {

		StoryLauncher.Instance.EndStory ();

		SoundManager.Instance.PlaySound (escapeSound);

	}

	public void ExitFight () {

		ChangeState (States.None);

		HideFighters (Crews.Side.Player);
		HideFighters (Crews.Side.Enemy);

		onFightEnd ();

		fighting = false;


	}
	#endregion

	#region fighters
	private void ShowFighters () {

		foreach ( Crews.Side side in Crews.Instance.Sides ) {
			
			Crews.getCrew(side).UpdateCrew ( Crews.PlacingType.Hidden );

			Fighter[] fighters = side == Crews.Side.Player ? initPlayerFighters : initEnemyFighters;

			for (int fighterIndex = 0; fighterIndex < Crews.getCrew(side).CrewMembers.Count; fighterIndex++) {
				
				fighters [fighterIndex].Reset (Crews.getCrew(side).CrewMembers[fighterIndex],fighterIndex);
			}
		}

		foreach (CrewMember mem in Crews.enemyCrew.CrewMembers)
			crewValue += mem.Level;
	}
	private void HideFighters (Crews.Side side) {

		Fighter[] fighters = side == Crews.Side.Player ? initPlayerFighters : initEnemyFighters;

		foreach (Fighter f in fighters)
			f.Hide ();
	}
	public void DeleteFighter (Fighter fighter) {
		fighters.Remove (fighter);
		if ( fighter.crewMember.side == Crews.Side.Player)
			currPlayerFighters.Remove (fighter);
		else
			currEnemyFighters.Remove (fighter);

		CheckMembers ();
	}
	#endregion

	#region StateMachine
	public delegate void OnChangeState (States currState , States prevState);
	public OnChangeState onChangeState;
	public void ChangeState ( States newState ) {


		previousState = currentState;
		currentState = newState;

		if (onChangeState != null) {
			onChangeState (currentState, previousState);
		}

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
		case States.PlayerActionChoice:
			updateState = PlayerActionChoice_Update;
			PlayerActionChoice_Start ();
			break;

		case States.PlayerMemberChoice:
			updateState = PlayerMemberChoice_Update;
			PlayerMemberChoice_Start ();
			break;
		case States.PlayerAction:
			updateState = PlayerAction_Update;
			PlayerAction_Start ();
			break;
		case States.EnemyActionChoice:
			updateState = EnemyActionChoice_Update;
			EnemyActionChoice_Start ();
			break;
		case States.EnemyMemberChoice:
			updateState = EnemyMemberChoice_Update;
			EnemyMemberChoice_Start();
			break;
		case States.EnemyAction:
			updateState = EnemyAction_Update;
			EnemyAction_Start ();
			break;

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
		case States.PlayerActionChoice:
			PlayerAction_Exit();
			break;
		case States.PlayerMemberChoice:
			PlayerMemberChoice_Exit();
			break;
		case States.PlayerAction:
			PlayerAction_Exit();
			break;
		case States.EnemyActionChoice:
			EnemyAction_Exit();
			break;
		case States.EnemyMemberChoice:
			EnemyMemberChoice_Exit();
			break;
		case States.EnemyAction:
			EnemyAction_Exit();
			break;
			//

		case States.None:
			break;
		}
	}
	#endregion

	#region properties
	public void StartFight () {

		Crews.enemyCrew.managedCrew.hostile = true;

		fighting = true;

		ChangeState (States.CombatStart);

	}

	void NextMember () {
		
		++memberIndex;

		if ( memberIndex >= fighters.Count )
			memberIndex = 0;

		if ( memberIndex < 0 )
			memberIndex = fighters.Count-1;
	}
	#endregion


}
