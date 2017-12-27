using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {

	public Fighter fighter;
	public Fighter preferedTarget;

	public string name = "";
	public string description = "";
	public int energyCost = 5;
	public float animationDelay = 0.6f;

	public bool playAnim = true;
	public SkillManager.AnimationType animationType;

	public bool hasTarget = false;
	public bool goToTarget = true;
	public bool canTargetSelf = true;
	public TargetType targetType;
	public int priority = 0;

	public Job linkedJob;

	public enum TargetType {
		Other,
		Self
	}

	public Type type;

	public virtual void Start () {
		
		SkillButton.onTriggerSKill += HandleOnTriggerSkill;
		CombatManager.Instance.onEnemyTriggerSkill += HandleOnTriggerSkill;

	}

	/// <summary>
	/// TRIGGER
	/// </summary>
	void HandleOnTriggerSkill (Skill skill)
	{
		if (skill.type == this.type) {

			Fighter fighter = CombatManager.Instance.currentFighter;

			if ( fighter.crewMember.energy < energyCost ) {
				print ("doenst has enough energy");
				return;
			}

			Trigger (fighter);
		}

	}

	void UseEnergy () {
		fighter.crewMember.energy -= energyCost;
	}

	public virtual void Trigger (Fighter fighter) {

		this.fighter = fighter;

		if (hasTarget) {
			SetTarget ();
		} else {
			SkipTarget ();
		}

	}

	/// <summary>
	/// no target
	/// </summary>
	void SkipTarget (){

		if (CombatManager.Instance.currentFighter.crewMember.side == Crews.Side.Enemy) { 
			CombatManager.Instance.ChangeState (CombatManager.States.EnemyAction);
		} else {
			CombatManager.Instance.ChangeState (CombatManager.States.PlayerAction);
		}

		InvokeSkill ();
	}

	/// <summary>
	/// target
	/// </summary>
	void SetTarget ()
	{
		if (CombatManager.Instance.currentFighter.crewMember.side == Crews.Side.Enemy) { 

			CombatManager.Instance.GoToTargetSelection (Crews.Side.Enemy, this);

		} else {
			
			CombatManager.Instance.GoToTargetSelection (Crews.Side.Player, this);

		}

		CombatManager.Instance.onChangeState += HandleOnChangeState;
	}



	void HandleOnChangeState (CombatManager.States currState, CombatManager.States prevState)
	{
		CombatManager.Instance.onChangeState -= HandleOnChangeState;

		if ( currState == CombatManager.States.PlayerAction || currState == CombatManager.States.EnemyAction ) {

			if (goToTarget) {
				fighter.onReachTarget += HandleOnReachTarget;
				fighter.ChangeState (Fighter.states.moveToTarget);
			} else {
				InvokeSkill ();
			}

		}
	}

	void HandleOnReachTarget ()
	{
		fighter.onReachTarget -= HandleOnReachTarget;
		InvokeSkill ();
	}

	public virtual void InvokeSkill () {

		UseEnergy ();

		fighter.ChangeState (Fighter.states.triggerSkill);

		TriggerAnimation ();

		Invoke ("ApplyEffect", animationDelay);
	}

	public void TriggerAnimation () {
		if (!playAnim)
			return;

		fighter.Animator.SetInteger("hitType",(int)animationType);
		fighter.Animator.SetTrigger ( "hit" );
		//
	}

	/// <summary>
	/// Triggers the skill.
	/// </summary>
	public virtual void ApplyEffect ()
	{
		if (goToTarget) {

			fighter.TargetFighter.CheckContact (fighter);

			Invoke ("InvokeMoveBack",1f);
		}
	}

	void InvokeMoveBack()  {
		fighter.ChangeState (Fighter.states.moveBack);
		//
	}

	/// <summary>
	/// end skill
	/// </summary>
	public virtual void EndSkill () {

		if ( SkillManager.CanUseSkill (fighter.crewMember.energy) ) {

			if ( fighter.crewMember.side == Crews.Side.Player )
				CombatManager.Instance.ChangeState (CombatManager.States.PlayerActionChoice);
			else
				CombatManager.Instance.ChangeState (CombatManager.States.EnemyActionChoice);

		} else {

			CombatManager.Instance.NextTurn ();

		}
	}

	public virtual bool MeetsRestrictions ( CrewMember member ) {


		return true;
	}

	public virtual bool MeetsConditions (CrewMember member) {

		if ( canTargetSelf == false ) {
			return member.energy >= energyCost && Crews.enemyCrew.CrewMembers.Count > 1;
		}

		// assez d'énergie
		return member.energy >= energyCost && MeetsRestrictions(member);
	}

	// ENUM //
	public enum Type {

		// NORMAL
		CloseAttack,
		DistanceAttack,
		Flee,
		SkipTurn,

		// BRUTE
		Cosh,
		Wallop,
		Leap,
		Fury,

		// SURGEON
		BistouryBlow,
		Jag,
		RatPoison,
		RhumRound,

		// COOK
		Goad,
//		Parry,
		HelpMate,
		ToastUp,
		PledgeOfFeast,

		// FILIBUSTER
		HeadShot,
		GrapeShot,
		BearTrap,
		Dynamite,

		// PLAYER
		HeadsOrTails,
		Robbery,
		Cuss,
		DoubleTalk,

	}
}
