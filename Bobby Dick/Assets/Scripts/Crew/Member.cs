﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Member {

	// name
	public string Name = "";


	public int health = 100;
	public int maxHealth = 100;

	public int currentHunger = 0;

	// lvl
	public int Lvl 		= 0;
	public int xp 		= 0; 
	public int statPoints = 0;


	public List<Skill> specialSkills = new List<Skill> ();
	public List<Skill> defaultSkills = new List<Skill> ();

	// stats
	public int[] stats = new int[4] {
		1,1,1,1
	};

	public int daysOnBoard = 0;

	public Item equipedWeapon;
	public Item equipedCloth;

	public Member () {

	}

	public Member (CrewParams crewParams) {

		// GENRE
		if (crewParams.overideGenre) {
			Male = crewParams.male;
		} else {
			Male = Random.value < 0.5f;
		}

		// NAME
		if (Male) {
			Name = CrewCreator.Instance.MaleNames[Random.Range (0, CrewCreator.Instance.MaleNames.Length)];
		} else {
			Name = CrewCreator.Instance.FemaleNames[Random.Range (0, CrewCreator.Instance.FemaleNames.Length)];
		}

		// JOB & SKILLS
		job = (Job)Random.Range(0,5);
		// special
		specialSkills = SkillManager.getJobSkills (job);
		// default
		defaultSkills.Add(SkillManager.getSkill(Skill.Type.Flee));
		defaultSkills.Add(SkillManager.getSkill(Skill.Type.CloseAttack));
		defaultSkills.Add(SkillManager.getSkill(Skill.Type.SkipTurn));

		// LEVEL
		if (crewParams.level > 0) {
			Lvl = crewParams.level;
		} else {

			Lvl = Random.Range (Crews.playerCrew.captain.Level - 3, Crews.playerCrew.captain.Level + 3);
			if ( StoryReader.Instance.CurrentStoryHandler.storyType == StoryType.Quest ) {
				Debug.Log ("l'histoire est une quete, donc la crew est du meme niveau que ma quete");
//				QuestManager.Instance.Coords_CheckForTargetQuest.goldValue += (Lvl * 10);
//				QuestManager.Instance.Coords_CheckForTargetQuest.experience += (Lvl * 10);

				Lvl = QuestManager.Instance.Coords_CheckForTargetQuest.level;
			}
		}

		Lvl = Mathf.Clamp ( Lvl , 1 , 10 );

		int statAmount = Lvl - 1;

		while ( statAmount > 0 )  {
			++stats [Random.Range (0, 4)];
			--statAmount;
		}

		// il a 35% de chance d'être noir
		bodyColorID 	= Random.value < 0.35f ? 0 : 1;

		hairColorID 	= Random.Range ( 0 , CrewCreator.Instance.HairColors.Length  );
		if (Male) {
			hairSpriteID = Random.value > 0.2f ? Random.Range (0, CrewCreator.Instance.HairSprites_Male.Length) : -1;
		} else {
			hairSpriteID = Random.Range (0, CrewCreator.Instance.HairSprites_Female.Length);
		}

		beardSpriteID 	= Male ? (Random.value > 0.2f ? Random.Range (0 , CrewCreator.Instance.BeardSprites.Length) : -1) : -1;
		eyeSpriteID 	= Random.Range (0 , CrewCreator.Instance.EyesSprites.Length);
		eyebrowsSpriteID= Random.Range (0 , CrewCreator.Instance.EyebrowsSprites.Length);
		noseSpriteID 	= Random.Range (0 , CrewCreator.Instance.NoseSprites.Length);
		mouthSpriteID 	= Random.Range (0 , CrewCreator.Instance.MouthSprites.Length);

		equipedWeapon = ItemLoader.Instance.GetRandomItemOfCertainLevel (ItemCategory.Weapon, Lvl);
		equipedCloth = ItemLoader.Instance.GetRandomItemOfCertainLevel (ItemCategory.Clothes, Lvl);

	}

	// icon index
	public bool Male = false;
	public Job job;
	public int bodyColorID = 0;
	public int hairSpriteID = 0;
	public int eyeSpriteID = 0;
	public int eyebrowsSpriteID = 0;
	public int hairColorID = 0;
	public int beardSpriteID = 0;
	public int noseSpriteID = 0;
	public int mouthSpriteID = 0;
}