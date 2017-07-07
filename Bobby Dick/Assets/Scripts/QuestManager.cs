using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour {

	public static QuestManager Instance;

	public List<Quest> currentQuests = new List<Quest>();

	void Awake () {
		Instance = this;
	}

	public void NewQuest () {

		Quest newQuest = new Quest ();

		currentQuests.Add (newQuest);
	}

	public void CheckQuest () {
		foreach ( Quest quest in currentQuests ) {
			
			if ( quest.targetX == Boats.Instance.PlayerBoatInfo.PosX
				|| quest.targetY == Boats.Instance.PlayerBoatInfo.PosY ) {



			}

		}
	}

}

public class Quest {

	public int questID = 0;

	public int targetX = 0;
	public int targetY = 0;

	public Quest () {

	}

	//
}
