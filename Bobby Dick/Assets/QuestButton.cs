
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestButton : MonoBehaviour {

	public int id = 0;

	public void Select () {
		QuestMenu.Instance.Select (id);
	}
}
