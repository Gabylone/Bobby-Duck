
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
	private Text giverText;

	[SerializeField]
	private Text experienceText;

	[SerializeField]
	private Text levelText;

	[SerializeField]
	private GameObject achievedFeedback;

	public void Select () {

		Quest quest = QuestManager.Instance.currentQuests [id];
		quest.ShowOnMap ();

		Tween.Bounce ( transform );

	}

	public void SetQuest ( int id ) {

		this.id = id;

		Quest quest = QuestManager.Instance.currentQuests [id];

		nameText.text = quest.Story.name;

//		giverText.text = quest.giver.Name;

//		goldText.text = quest.goldValue.ToString ();

//		experienceText.text = quest.experience.ToString ();

		if (quest.accomplished) {
			
			levelText.gameObject.SetActive (false);
			achievedFeedback.SetActive (true);
		} else {
			levelText.text = quest.level.ToString ();
			levelText.gameObject.SetActive (true);
			achievedFeedback.SetActive (false);
		}

	}

	public void GiveUpQuest() {

		MessageDisplay.onValidate += HandleOnValidate;

		MessageDisplay.Instance.Show ("Abandonner quête ?");
	}

	void HandleOnValidate ()
	{
		QuestManager.Instance.GiveUpQuest (QuestManager.Instance.currentQuests [id]);
	}
}
