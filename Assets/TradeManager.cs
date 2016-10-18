using UnityEngine;
using System.Collections;

public class TradeManager : MonoBehaviour {

	[SerializeField]
	private LootUI enemyLootUI;

	public void StartTrade () {

		LootManager.enemyLoot.Randomize ();

	}
}
