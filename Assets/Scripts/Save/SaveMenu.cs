using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveMenu : MonoBehaviour {

	[Header("Buttons")]
	[SerializeField] private Button[] saveButtons;

	[SerializeField]
	private GameObject saveGroup;
	[SerializeField]
	private GameObject saveFeedback;

		// saving
	private bool saving = false;
	public bool Saving {
		get {
			return saving;
		}
		set {
			saving = value;
		}
	}

	bool confirmed = false;

	bool opened = false;

	public bool Opened {
		get {
			return opened;
		}
		set {
			opened = value;

			saveGroup.SetActive (value);

			confirmed = false;

			if ( value == true )
				UpdateButtons ();
		}
	}

	void Start()
	{
		Opened = false;

		UpdateButtons ();
	}

	#region save action

	public void SaveSocket (int index) {

		if (saving) {

			if (confirmed) {
				
				Save (index);

			} else {

				if ( SaveTool.Instance.FileExists (index) ) {

					saveFeedback.SetActive (true);
					saveFeedback.GetComponentInChildren<Text> ().text = "Ecraser ?";
					confirmed = true;

				} else {

					Save (index);

				}
			}

		} else {

			SaveManager.Instance.LoadGame (index);

		}

	}
	private void Save (int index) {
		SaveTool.Instance.Save (index);
//		saving = false;
		confirmed = false;
		saveFeedback.SetActive (false);

		UpdateButtons ();

	}
	#endregion



	#region buttons
	public void UpdateButtons ()
	{
		

		int index = 0;

		foreach (Button button in saveButtons) {

			if (SaveTool.Instance.FileExists (index+1)) {

				GameData gameLoad = SaveTool.Instance.Load (index+1);

				saveButtons [index].GetComponentInChildren<Text> ().text = gameLoad.guyName;
				saveButtons [index].GetComponentInChildren<Text> ().color = Color.black;
				saveButtons[index].image.color = Color.white;
				saveButtons[index].interactable = true;

			} else {

				saveButtons [index].GetComponentInChildren<Text> ().text = saving ? "?" : "aucun";
				saveButtons [index].GetComponentInChildren<Text> ().color = Color.white;
				saveButtons[index].image.color = Color.black;
				saveButtons[index].interactable = saving;



			}

			++index;
		}
	}
	#endregion
}
