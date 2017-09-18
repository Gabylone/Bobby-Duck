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

	// feedback
	public GameObject feedbackObject;
	float feedbackDuration = 1f;

	void Awake () {
		Instance = this;
	}

	void Start () {
		QuestManager.Instance.newQuestEvent += HandleNewQuestEvent;
		QuestManager.onGiveUpQuest += HandleOnFinishQuest;
		QuestManager.onFinishQuest += HandleOnFinishQuest;
	}

	void HandleOnFinishQuest (Quest quest)
	{
		InitButtons ();
	}

	#region feedback
	void HandleNewQuestEvent ()
	{
		feedbackObject.SetActive (true);

		Tween.Bounce (feedbackObject.transform);

		Invoke ("HideFeedback" , feedbackDuration );

		InitButtons ();
	}

	void HideFeedback () {
		feedbackObject.SetActive (false);
	}
	#endregion

	public void Init () {
		InitButtons ();
	}

	public void Open () {
		
		openButton.SetActive (false);
		menuGroup.SetActive (true);

		PlayerLoot.Instance.CloseLoot ();

		Tween.ClearFade (menuGroup.transform);
		Tween.Bounce ( menuGroup.transform , 0.2f , 1.05f);
	}

	public void Close () {
		openButton.SetActive (true);

		Tween.Scale (menuGroup.transform , 0.2f , 0.8f);
		Tween.Fade (menuGroup.transform , 0.2f );

		Invoke ("HideMenu" , 0.2f);
	}

	void HideMenu() {
		menuGroup.SetActive (false);
	}

	void InitButtons () {

		for (int buttonIndex = 0; buttonIndex < QuestManager.Instance.CurrentQuests.Count; buttonIndex++) {

			if ( buttonIndex >= buttons.Count ) {
				GameObject newButton = Instantiate (buttonPrefab, anchor) as GameObject;
				buttons.Add(newButton.GetComponent<Button> ());

				buttons [buttonIndex].GetComponent<RectTransform> ().localPosition = Vector2.up * -(buttonDecal * buttonIndex);
			}

			if (buttonIndex < QuestManager.Instance.CurrentQuests.Count) {

				buttons [buttonIndex].gameObject.SetActive (true);
				buttons [buttonIndex].GetComponent<QuestButton> ().SetQuest (buttonIndex);



			} else {

				buttons [buttonIndex].gameObject.SetActive (false);


			}

		}

		float scale = (-anchor.localPosition.y) + (buttonPrefab.GetComponent<RectTransform>().rect.height*QuestManager.Instance.CurrentQuests.Count);

		contentTransform.sizeDelta = new Vector2 (contentTransform.sizeDelta.x , scale);

	}

}
