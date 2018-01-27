using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestFeedback : MonoBehaviour {

	[SerializeField]
	private GameObject group;

	[SerializeField]
	private Text uiText;

	public float duration = 2f;

	// Use this for initialization
	void Start () {
		
		QuestManager.Instance.newQuestEvent += HandleNewQuestEvent;

		NameGeneration.onDiscoverFormula += HandleOnDiscoverFormula;
	}

	void HandleOnDiscoverFormula (Formula Formula)
	{
		Display ("Nouvel Indice !");
	}

	void HandleNewQuestEvent ()
	{
		if (QuestManager.Instance.CurrentQuests.Count == QuestManager.Instance.maxQuestAmount) {
			Display ("Nombre maximum de quête atteint");
		} else {
			Display ("Nouvelle Quête");
		}
	}

	void Display ( string str ) {
		
		Show ();

		Tween.Bounce (transform);

		uiText.text = str;

		Invoke ("Hide",duration);

	}

	void Show () {
		group.SetActive (true);
	}

	void Hide () {
		group.SetActive (false);
	}
}
