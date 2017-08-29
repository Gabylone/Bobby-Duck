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
	bool showFeedback = false;
	float feedbackDuration = 0.5f;

	void Awake () {
		Instance = this;
	}

	void Start () {
		QuestManager.Instance.newQuestEvent += HandleNewQuestEvent;
	}

	#region feedback
	void HandleNewQuestEvent ()
	{
		feedbackObject.SetActive (true);

		HOTween.To ( feedbackObject.transform , feedbackDuration , "localScale" , Vector3.one * 1.2f , false , EaseType.EaseOutBounce , 0f );
		HOTween.To ( feedbackObject.transform , feedbackDuration , "localScale" , Vector3.one , false , EaseType.EaseInBounce , feedbackDuration );

		Invoke ("HideFeedback" , feedbackDuration * 3);
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
