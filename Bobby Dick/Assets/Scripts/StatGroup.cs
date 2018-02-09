using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatGroup : MonoBehaviour {

	[SerializeField]
	private Text attackText;

	[SerializeField]
	private Text defenceText;

	[SerializeField]
	private GameObject group;

	public Image levelImage;
	public Text levelText;

	// Use this for initialization
	void Start () {
		Hide ();
	}

	void Show () {
		group.SetActive (true);
	}
	void Hide () 
	{
		group.SetActive (false);
	}

	public void Display (CrewMember member) {
		
		Show ();
		Tween.Bounce (transform);

		UpdateUI (member);

		CancelInvoke ();

		Invoke("Hide" , 1f);

	}
	
	// Update is called once per frame
	public void UpdateUI (CrewMember member) {
//		
//		attackText.text = "" + member.Attack;
//		defenceText.text = "" + member.Defense;

		levelImage.color = member.GetLevelColor ();

		levelText.text = "" + member.Level;


	}
}
