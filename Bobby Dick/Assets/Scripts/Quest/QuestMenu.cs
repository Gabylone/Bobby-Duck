using System.Collections;
using System.Collections.Generic;
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
//	[SerializeField]
//	private GameObject closeButton;

	void Awake () {
		Instance = this;
	}

	void Update () {

	}

	public void Init () {
		InitButtons ();
	}

	public void Open () {
		openButton.SetActive (false);
		menuGroup.SetActive (true);
	}

	public void Close () {
		openButton.SetActive (true);
		menuGroup.SetActive (false);
	}

	public void InitButtons () {

		for (int buttonIndex = 0; buttonIndex < QuestManager.Instance.CurrentQuests.Count; buttonIndex++) {

			if ( buttonIndex >= buttons.Count ) {
				GameObject newButton = Instantiate (buttonPrefab, anchor) as GameObject;
				buttons.Add(newButton.GetComponent<Button> ());

				buttons [buttonIndex].GetComponent<RectTransform> ().localPosition = Vector2.up * -(buttonDecal * buttonIndex);
			}

			if (buttonIndex < QuestManager.Instance.CurrentQuests.Count) {

				buttons [buttonIndex].gameObject.SetActive (true);

				buttons [buttonIndex].GetComponentInChildren<Text> ().text = QuestManager.Instance.CurrentQuests[buttonIndex].Story.name;
				buttons [buttonIndex].GetComponentInChildren<QuestButton> ().id = buttonIndex;

			} else {

				buttons [buttonIndex].gameObject.SetActive (false);


			}

		}

		float scale = (-anchor.localPosition.y) + (buttonPrefab.GetComponent<RectTransform>().rect.height*QuestManager.Instance.CurrentQuests.Count);

		contentTransform.sizeDelta = new Vector2 (contentTransform.sizeDelta.x , scale);

	}

	public void Select ( int i ) {

		Quest quest = QuestManager.Instance.CurrentQuests [i];
		quest.ShowOnMap ();

	}

}
