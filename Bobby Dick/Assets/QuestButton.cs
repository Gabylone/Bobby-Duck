
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour {

	public int id = 0;

	[SerializeField]
	private Text nameText;

	[SerializeField]
	private Text goldText;

	[SerializeField]
	private Text experienceText;

	public void Select () {

		Quest quest = QuestManager.Instance.CurrentQuests [id];
		quest.ShowOnMap ();

		Tween.Bounce ( transform );
	}

	public void SetQuest ( int id ) {

		this.id = id;

		Quest quest = QuestManager.Instance.CurrentQuests [id];

		nameText.text = quest.Story.name;

		goldText.text = quest.goldValue.ToString ();

		experienceText.text = quest.experience.ToString ();


	}

	public void GiveUpQuest() {

		MessageDisplay.onValidate += HandleOnValidate;

		MessageDisplay.Instance.Show ("Abandonner quête ?");
	}

	void HandleOnValidate ()
	{
		QuestManager.Instance.GiveUpQuest (QuestManager.Instance.CurrentQuests [id]);
	}
}
