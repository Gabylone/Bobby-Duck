using UnityEngine;
using System.Collections;

public class CardManager : MonoBehaviour {

	public static CardManager Instance;


	[Header("Combat")]
	[SerializeField] private Card[] combatCards;

	void Awake () {
		Instance = this;
	}

	void Start () {
		foreach (Card combatCard in combatCards)
			combatCard.Init ();

		CombatManager.Instance.fightEnding += HideFightingCards;
	}

	#region fighting cards
	public void ShowFightingCard ( CrewMember member ) {
		combatCards [(int)member.side].ShowCard ();
		combatCards [(int)member.side].UpdateMember (member);
	}
	public void HideFightingCards () {
		foreach (var combatCard in combatCards) {
			combatCard.HideCard ();
		}
	}
	#endregion

}
