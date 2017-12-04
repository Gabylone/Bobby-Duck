using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayJob : MonoBehaviour {

	public Image jobImage;

	public Text jobText;

	public string[] jobNames = new string[5] {
		"Brute",
		"Medecin",
		"Cuistot",
		"Flibustier",
		"Escroc"
	};

	void Start () {
		
		CrewInventory.Instance.openInventory += HandleOpenInventory;

		UpdateUI ();
	}


	void HandleOpenInventory (CrewMember member)
	{
		UpdateUI ();
	}

	void UpdateUI ()
	{
		CrewMember member = CrewMember.selectedMember;

		if (SkillManager.jobSprites.Length <= (int)member.job)
			print ("skill l : " + SkillManager.jobSprites.Length + " / member job " + (int)member.job);

		jobImage.sprite = SkillManager.jobSprites [(int)member.job];
		jobText.text = jobNames [(int)member.job];

		Bounce ();
	}

	void Bounce ()
	{
		Tween.Bounce (jobImage.transform);
	}
}
