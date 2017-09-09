using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatButton : MonoBehaviour {

	Button button;
	Text text;

	[SerializeField]
	private Stat stat;

	// Use this for initialization
	void Start () {

		button = GetComponent<Button> ();
		text = GetComponentInChildren<Text> ();

		PlayerLoot.Instance.openInventory += HandleOnCardUpdate;

		StatButton.onClickStatButton += HandleOnClickStatButton;

		Disable ();

	}

	void HandleOnClickStatButton ()
	{
		Display (PlayerLoot.Instance.SelectedMember);
	}

	void Disable ()
	{
		button.interactable = false;
	}

	void Enable () {
		button.interactable = true;
	}

	void HandleOnCardUpdate (CrewMember member)
	{
		Display (member);
	}

	void Display (CrewMember member) {
		if (member.StatPoints > 0 && member.GetStat(stat) < 7) {
			Enable ();
		} else {
			Disable ();
		}

		text.text = member.GetStat(stat).ToString();
	}

	public delegate void OnClickStatButton();
	public static OnClickStatButton	onClickStatButton;

	public void OnClick () {
		
		Tween.Bounce (transform);

		PlayerLoot.Instance.SelectedMember.HandleOnLevelUpStat (stat);

		if (onClickStatButton!=null)
			onClickStatButton ();
	}
}
