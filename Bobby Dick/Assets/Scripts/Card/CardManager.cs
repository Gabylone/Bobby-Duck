using UnityEngine;
using System.Collections;

public class CardManager : MonoBehaviour {

	public static CardManager Instance;

	[Header ("Overing")]
	[SerializeField]
	private Card[] overingCards;

	[Header("Combat")]
	[SerializeField] private Card[] combatCards;

	void Awake () {
		Instance = this;
	}

	void Start () {
		foreach (Card card in overingCards)
			card.Init ();

		foreach (Card combatCard in combatCards)
			combatCard.Init ();
	}

	#region overing cards
	public void ShowOvering ( CrewMember member ) {

		Vector3 pos = member.Icon.transform.position;
		ShowOvering (member, pos);
	}
	public void ShowOvering ( CrewMember member, Vector2 pos ) {
		overingCards[(int)member.Side].UpdateMember (member);
		overingCards[(int)member.Side].PlaceCard (pos);
	}

	public void HideOvering () {
		foreach (Card card in overingCards)
			card.HideCard ();
	}
	#endregion

	#region fighting cards
	public void ShowFightingCard ( CrewMember member ) {
		combatCards [(int)member.Side].UpdateMember (member);
	}
	public void HideFightingCards () {
		foreach (var combatCard in combatCards) {
			combatCard.HideCard ();
		}
	}
	#endregion

}
