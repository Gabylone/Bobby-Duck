using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {

	public static Skill[] skills;

	public static Sprite[] skillSprites;
	public static Sprite[] jobSprites;

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
			item.description = cells [4];

			++skillIndex;
		}

		skillSprites = Resources.LoadAll<Sprite> ("Graph/SkillsSprites");
		jobSprites = Resources.LoadAll<Sprite> ("Graph/JobSprites");

	}

	public static Skill getSkill ( Skill.Type type ) {

		Skill skill = System.Array.Find (skills, x => x.type == type);

		if (skill == null)
			print ("getting skill : " + type + " is null");

		return skill;
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
}
