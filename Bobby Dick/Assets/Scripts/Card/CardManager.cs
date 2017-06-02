using UnityEngine;
using System.Collections;

public class CardManager : MonoBehaviour {

	public static CardManager Instance;

	[Header ("Overing")]
	[SerializeField]
	private Card overingCard;

	[Header("Combat")]
	[SerializeField] private Card[] combatCards;

	[SerializeField]
	private Vector3 decal = Vector3.zero;

	void Awake () {
		Instance = this;
	}

	#region overing cards
	public void ShowOvering ( CrewMember member ) {
		overingCard.UpdateMember (member);
		Vector3 targetPos = member.IconObj.transform.position;
		if ( member.IconObj.transform.position.x > 0 )
			targetPos += decal;

		overingCard.PlaceCard (targetPos);
	}
	public void HideOvering () {
		overingCard.HideCard ();
	}
	#endregion

	#region fighting cards
	public void ShowFightingCard ( CrewMember member ) {
		combatCards [(int)member.Side].UpdateMember (member);
	}
	public void HideFightingCard ( CrewMember member) {
		combatCards [(int)member.Side].HideCard ();
	}
	#endregion

	public Card OveringCard {
		get {
			return overingCard;
		}
	}

}
