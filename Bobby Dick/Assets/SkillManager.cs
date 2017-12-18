using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {

	public static Skill[] skills;

	public static Sprite[] skillSprites;
	public static Sprite[] jobSprites;

	public static List<Skill> defaultSkills = new List<Skill> ();

	public static string[] jobNames = new string[5] {
		"Brute",
		"Medecin",
		"Cuistot",
		"Flibustier",
		"Escroc"
	};

	public TextAsset skillData;

	// Use this for initialization
	void Start () {
		
		skills = GetComponentsInChildren<Skill> ();

		string[] rows = skillData.text.Split ('\n');

		int skillIndex = 0;
		foreach (var item in skills) {

			string[] cells = rows [skillIndex+2].Split (';');

			item.type = (Skill.Type)skillIndex;

			if (cells.Length <= 1)
				print (item.type);
			
			item.name = cells [1];
			item.energyCost = int.Parse ( cells[3] );
			item.priority = int.Parse ( cells[4] );
			item.description = cells [5];

			++skillIndex;
		}

		skillSprites = Resources.LoadAll<Sprite> ("Graph/SkillsSprites");
		jobSprites = Resources.LoadAll<Sprite> ("Graph/JobSprites");

		defaultSkills.Add (SkillManager.getSkill (Skill.Type.Flee));
		defaultSkills.Add (SkillManager.getSkill (Skill.Type.CloseAttack));
		defaultSkills.Add (SkillManager.getSkill (Skill.Type.SkipTurn));
		

	}

	public static Skill getSkill ( Skill.Type type ) {

		Skill skill = System.Array.Find (skills, x => x.type == type);

		if (skill == null)
			print ("getting skill : " + type + " is null");

		return skill;
	}

	public static int getSkillIndex ( Skill skill ) {

		int skillIndex = System.Array.FindIndex (skills, x => x.type == skill.type);

		return skillIndex;
	}

	public static bool CanUseSkill ( int energy ) {

		foreach (var item in skills) {
			if (energy >= item.energyCost) {

				if (item.type == Skill.Type.SkipTurn)
					continue;
				
				return true;
			}
		}

		return false;

	}

	public static List<Skill> getJobSkills ( Job job ) {

		List<Skill> jobSkills = new List<Skill> ();

		foreach (var skill in skills) {

			if (skill.linkedJob == job) {
				
				jobSkills.Add (skill);

			}

		}

		if ( jobSkills.Count == 0 )
			print ("skills were not found .... !!!! for job : " + job.ToString());

		return jobSkills;

	}

	public static Skill RandomSkill ( CrewMember member ) {

		List<Skill> fittingSkills = new List<Skill> ();

		int priority = 0;

		List<Skill> memberSkills = defaultSkills;
		foreach (var item in member.specialSkills) {
			memberSkills.Add (item);
		}

		// dans tous les skills du membre
		foreach (var item in memberSkills) {


			if ( item.MeetsConditions(member) ) {

//				print (item.name + " rempli les conditions");

//				if (item.priority < priority) {
//					print (item.name + " a une priorité inférieure");
//				}
//
				if (item.priority == priority) {
//					print (item.name + " a une priorité supérieure");
					fittingSkills.Add (item);
				}

				if ( item.priority > priority ) {

//					print (item.name + " a une priorité supérieure");

					fittingSkills.Clear ();
					fittingSkills.Add (item);
					priority = item.priority;

				}
			}
		}

		Skill skill = fittingSkills[Random.Range(0,fittingSkills.Count)];

		return skill;
	}
}
