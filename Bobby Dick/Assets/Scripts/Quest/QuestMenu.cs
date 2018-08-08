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

	public delegate void OnOpenQuestMenu ();
	public static OnOpenQuestMenu onOpenQuestMenu;

	[SerializeField]
	DisplayFormulas displayFormulas;

	bool opened = false;

	void Awake () {
		Instance = this;

        onOpenQuestMenu = null;
        
	}

	void Start () {
		
		QuestManager.onGiveUpQuest += HandleOnGiveUpQuest;
//		CrewInventory.Instance.closeInventory += HandleCloseInventory;

		RayBlocker.onTouchRayBlocker += HandleOnTouchRayBlocker;

		HideMenu ();
	}

	void HandleOnTouchRayBlocker ()
	{
		if ( opened )
			Close ();
	}

	public void Init () {
		InitButtons ();
	}

	void HandleOnGiveUpQuest (Quest quest)
	{
		UpdateButtons ();
	}

	public void Open () {


		menuGroup.SetActive (true);

		//displayFormulas.ShowFormulas ();

		DisplayQuestAmount ();

		CrewInventory.Instance.HideMenuButtons ();

		UpdateButtons ();

//		Tween.ClearFade (menuGroup.transform);
		//Tween.Bounce ( menuGroup.transform , 0.2f , 1.05f);

		if (onOpenQuestMenu != null)
			onOpenQuestMenu ();

		opened = true;

	}

	public void Close () {
		opened = false;

		Invoke ("CloseDelay",0.01f);

		HideMenu();
	}

	void CloseDelay () {
		CrewInventory.Instance.ShowMenuButtons ();
		//
	}

	void DisplayQuestAmount () {

		/*if (QuestManager.Instance.currentQuests.Count == 0) {
			displayQuestText.text = "aucune quêtes";
		} else {
			displayQuestText.text = QuestManager.Instance.currentQuests.Count + " quêtes en cours";
		}*/

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
		StartCoroutine (UpdateButtonsCoroutine ());
	}

	IEnumerator UpdateButtonsCoroutine () {

		foreach (var item in buttons) {
			item.gameObject.SetActive (false);
		}

		/// UPDATE BUTTON TO QUESTS
		for (int questIndex = 0; questIndex < buttons.Count; questIndex++) {

			if (questIndex < QuestManager.Instance.currentQuests.Count) {

				buttons [questIndex].gameObject.SetActive (true);
				buttons [questIndex].GetComponent<QuestButton> ().SetQuest (questIndex);

				Tween.Bounce (buttons[questIndex].transform);

				yield return new WaitForSeconds (Tween.defaultDuration/1.5f);

			}
		}
	}

}
