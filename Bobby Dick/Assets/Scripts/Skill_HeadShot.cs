using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_HeadShot : Skill {

	bool onDelay = false;

	public int healthToAttack = 30;

	

	public override void InvokeSkill ()
	{
		if (onDelay) {
			print ("lui donne de l'energie");
			fighter.crewMember.energy += energyCost;
		}
		base.InvokeSkill ();
	}

	public override void ApplyEffect ()
	{

		base.ApplyEffect ();

		if (onDelay) {

			onDelay = false;
			hasTarget = false;
			playAnim = false;

			fighter.TargetFighter.GetHit (fighter, fighter.crewMember.Attack , 3f);

			EndSkill ();
			//

		} else {

			fighter.AddStatus (Fighter.Status.PreparingAttack);

			fighter.onSkillDelay += HandleOnSkillDelay;

			EndSkill ();

		}

	}

	Fighter delayFighter;
	void HandleOnSkillDelay (Fighter _delayFighter)
	{
		Invoke ("TriggerDelay",0.1f);
		this.delayFighter = _delayFighter;
		delayFighter.combatFeedback.Display (base.name);
	}

	void TriggerDelay () {
		onDelay = true;
		hasTarget = true;
		playAnim = true;

		Trigger (delayFighter);
	}

	public override bool MeetsRestrictions (CrewMember member)
	{
		if (member.GetEquipment (CrewMember.EquipmentPart.Weapon) == null)
			return false;

		if (member.GetEquipment(CrewMember.EquipmentPart.Weapon).spriteID == 1)  {
//			print ("il a une épée alors qu'il devrait avoir un pistolet");
		}

		return base.MeetsRestrictions (member) && member.GetEquipment(CrewMember.EquipmentPart.Weapon).spriteID == 0;
	}

	public override bool MeetsConditions (CrewMember member)
	{
		bool hasTarget = false;

		foreach (var item in CombatManager.Instance.getCurrentFighters(Crews.otherSide(member.side)) ) {
			if (item.crewMember.Health < healthToAttack) {
				hasTarget = true;
				preferedTarget = item;
			}
		}

		return hasTarget && base.MeetsConditions (member);
	}
}
