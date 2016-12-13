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
	public void ShowFightingCard ( Crews.Side attackingCrew ) {
		combatCards [(int)attackingCrew].UpdateMember (CombatManager.Instance.Members[(int)attackingCrew]);
	}
	public void HideFightingCard ( Crews.Side attackingCrew ) {
		combatCards [(int)attackingCrew].HideCard ();
	}
	public void UpdateCards () {
		for (int i = 0; i < 2; ++i)
			combatCards [i].UpdateMember (CombatManager.Instance.Members[i]);

		for ( int i = 0; i < 2; i++ )
			combatCards[i].UpdateMember (CombatManager.Instance.Members[i]);
	}
	#endregion

	public Card OveringCard {
		get {
			return overingCard;
		}
	}

}
