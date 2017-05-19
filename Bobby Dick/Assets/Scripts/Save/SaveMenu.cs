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

	int currentSocket = -1;

		// saving
	private bool saving = false;

	bool save_Confirmed = false;

	bool opened = false;

	void Start()
	{
		Opened = false;

		saveGroup.SetActive (false);
	}

	#region save action

	public void SaveSocket (int index) {

		if (saving) {

			if (save_Confirmed) {
				
				Save (index);

			} else {

				if ( SaveTool.Instance.FileExists (index) ) {

					saveFeedback.SetActive (true);
					saveFeedback.GetComponentInChildren<Text> ().text = "Ecraser ?";
					save_Confirmed = true;

				} else {

					Save (index);

				}
			}

		} else {

			SaveManager.Instance.LoadGame (index);

		}

	}
	private void Save (int index) {
		
		SaveManager.Instance.SaveGame (index);
		save_Confirmed = false;
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

				string captainName = gameLoad.playerCrew.MemberIDs[0].male ? CrewCreator.Instance.MaleNames [gameLoad.playerCrew.MemberIDs [0].nameID] : CrewCreator.Instance.FemaleNames [gameLoad.playerCrew.MemberIDs [0].nameID];
				saveButtons [index].GetComponentInChildren<Text> ().text = captainName;
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


	public bool Opened {
		get {
			return opened;
		}
		set {
			opened = value;

			saveGroup.SetActive (value);

			save_Confirmed = false;

			saveFeedback.SetActive (false);

			if ( value == true )
				UpdateButtons ();
		}
	}

	public bool Saving {
		get {
			return saving;
		}
		set {
			saving = value;
		}
	}
	#endregion

	public int CurrentSocket {
		get {
			return currentSocket;
		}
		set {
			currentSocket = value;
		}
	}
}
