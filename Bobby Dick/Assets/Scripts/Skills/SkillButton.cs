using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {

	public Image skillImage;

	public GameObject descriptionGroup;

	public Text uiText_SkillName;
	public Text uiText_Description;

	public Skill skill;

	public Button button;

	public float timeToShowDescription = 0.5f;

	public virtual void Start () {
		button = GetComponentInChildren<Button> ();
		HideDescription ();
	}

	#region description
	public void ShowDescription ()
	{
		descriptionGroup.SetActive (true);

 		uiText_SkillName.text = skill.name;
		uiText_Description.text = skill.description;

		Tween.Bounce ( descriptionGroup.transform );
	}

	public void HideDescription ()
	{
		descriptionGroup.SetActive (false);
	}
	#endregion

	public virtual void SetSkill (Skill _skill)
	{
		skill = _skill;

		skillImage.sprite = SkillManager.skillSprites [(int)skill.type];

		uiText_SkillName.text = _skill.type.ToString ();
	}

	public void Show () {
		gameObject.SetActive (true);
		Tween.Bounce (transform);
	}

	public void Hide () 
	{
		gameObject.SetActive (false);
	}

}
