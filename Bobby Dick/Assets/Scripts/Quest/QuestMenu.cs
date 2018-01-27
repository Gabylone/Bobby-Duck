using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using UnityEngine;
using UnityEngine.UI;

public class QuestMenu : MonoBehaviour {

	public static QuestMenu Instance;

	[SerializeField]
	private GameObject buttonPrefab;

	private List<Button> buttons = new List<Button>();

	[SerializeField]
	private float buttonDecal = 0f;

	[SerializeField]
	private Transform anchor;

	[SerializeField]
	private RectTransform contentTransform;

	[SerializeField]
	private GameObject menuGroup;
	[SerializeField]
	private GameObject openButton;

	public Text displayQuestText;

	[SerializeField]
	DisplayFormulas displayFormulas;
	void Awake () {
		Instance = this;
	}

	void Start () {
		
		QuestManager.onGiveUpQuest += HandleOnFinishQuest;
		CrewInventory.Instance.closeInventory += HandleCloseInventory;

		Close ();
	}

	public void Init () {
		InitButtons ();
	}

	void HandleCloseInventory ()
	{
		Close ();
	}

	void HandleOnFinishQuest (Quest quest)
	{
		UpdateButtons ();
	}

	public void Open () {
		
		openButton.SetActive (false);
		menuGroup.SetActive (true);

		CrewInventory.Instance.HideMenuButtons ();

		displayFormulas.ShowFormulas ();

		DisplayQuestAmount ();

		UpdateButtons ();

//		Tween.ClearFade (menuGroup.transform);
		Tween.Bounce ( menuGroup.transform , 0.2f , 1.05f);
	}

	void DisplayQuestAmount () {

		if (QuestManager.Instance.CurrentQuests.Count == 0) {
			displayQuestText.text = "aucune quêtes";
		} else {
			displayQuestText.text = QuestManager.Instance.CurrentQuests.Count + " quêtes en cours";
		}

	}

	public void Close () {
		
		openButton.SetActive (true);

		CrewInventory.Instance.ShowMenuButtons ();

		float dur = 0.1f;

		Tween.Scale (menuGroup.transform , dur , 0.95f);
//		Tween.Fade (menuGroup.transform , 0.2f );

		Invoke ("HideMenu" ,dur);
	}

	void HideMenu() {
		menuGroup.SetActive (false);
	}

	void InitButtons () {

		/// CREATE BUTTONS
		for (int buttonIndex = 0; buttonIndex < QuestManager.Instance.maxQuestAmount; buttonIndex++) {

			GameObject newButton = Instantiate (buttonPrefab, anchor) as GameObject;
			buttons.Add(newButton.GetComponent<Button> ());

		}

	}

	void UpdateButtons () {

		/// UPDATE BUTTON TO QUESTS
		for (int questIndex = 0; questIndex < buttons.Count; questIndex++) {

			if (questIndex < QuestManager.Instance.CurrentQuests.Count) {

				buttons [questIndex].gameObject.SetActive (true);
				buttons [questIndex].GetComponent<QuestButton> ().SetQuest (questIndex);

			} else {

				buttons [questIndex].gameObject.SetActive (false);


			}
		}
	}

//	IEnumerator UpdateButtonsCoroutine () {
//
//	}

}
