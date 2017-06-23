using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour {

	public static DialogueManager Instance;

	void Awake () {
		Instance = this;
	}

	[Header("UI Element")]
	[SerializeField] private GameObject bubble_Obj;
	[SerializeField] private RectTransform bubble_Image;
	[SerializeField] private Text bubble_Text;

	private Transform target;

	private bool DisplayingText = false;

	[Header("Parameters")]
	[SerializeField]
	private Vector3 speaker_Decal = Vector3.up * 2f;
	private CrewMember talkingMember;

	[SerializeField] private Vector2 bubbleBounds = new Vector2();

	private int TextIndex = 0;
	private string[] TextsToDisplay = new string[2] {"Rentrer","Dialogues"};

	[Header("Narrator")]
	[SerializeField] private Text narratorText;
	[SerializeField] private GameObject narratorObj;

	[SerializeField]
	private float DisplayTime = 2.5f;
	private float CurrentTime = 0f;

	[Header("Sounds")]
	[SerializeField] private AudioClip[] speakSounds;

	void Update() 
	{
		if (DisplayingText == true) {
			UpdateDialogue ();
		}

	}
	#region set dialogue
	bool timed = false;
	public void SetDialogueTimed (string phrase, Transform _target) {
		timed = true;
		phrase = CheckForKeyWords (phrase);
		SetDialogue (phrase, _target);
	}
	public void SetDialogueTimed (string phrase, CrewMember crewMember) {
		timed = true;
		phrase = CheckForKeyWords (phrase);
		SetDialogue (phrase, crewMember.Icon.dialogueAnchor);
	}
	public void SetDialogue (string phrase, CrewMember crewMember) {
		phrase = CheckForKeyWords (phrase);
		SetDialogue (phrase, crewMember.Icon.dialogueAnchor);
	}
	public void SetDialogue (string phrase, Transform _target) {
		
		DialogueTexts = new string[1] {phrase};

		target = _target;

		StartDialogue ();
	}

	public string[] DialogueTexts {
		get { return TextsToDisplay; }
		set { TextsToDisplay = value; }
	}
	#endregion

	#region dialogue states
	private void StartDialogue () {

		bubble_Obj.SetActive (true);

		// reset text
		TextIndex = 0;
		bubble_Text.text = TextsToDisplay[TextIndex];
		CurrentTime = DisplayTime;

		DisplayingText = true;

		UpdateBubblePosition ();

		if ( talkingMember != null )
			SoundManager.Instance.PlaySound ( speakSounds[talkingMember.MemberID.VoiceID] );
	}

	private void UpdateDialogue () {
		
		if (CurrentTime > 0)
		{
			if (timed) {
				CurrentTime -= Time.deltaTime;
			}

			UpdateBubblePosition ();

		}
		else
		{
			if (TextIndex < TextsToDisplay.Length - 1)
				NextPhrase ();
			else
				EndDialogue ();

			CurrentTime = DisplayTime;
		}
	}
	public void EndDialogue ()
	{
		DisplayingText = false;

		bubble_Obj.SetActive (false);

		timed = false;
	}
	#endregion

	#region key words

	string lastItemName = "";

	public string LastItemName {
		get {
			return lastItemName;
		}
		set {
			lastItemName = value;
		}
	}

	public string CheckForKeyWords ( string text ) {

		if ( text.Contains ("CAPITAINE") ) {
			text = text.Replace ( "CAPITAINE" , Crews.playerCrew.captain.MemberName );
		}

		if ( text.Contains ("OTHERNAME") ) {
			text = text.Replace ( "OTHERNAME" , Crews.enemyCrew.captain.MemberName );
		}

		if ( text.Contains ("NOMBATEAU") ) {
			text = text.Replace ( "NOMBATEAU" , Boats.Instance.PlayerBoatInfo.Name);
		}

		if ( text.Contains ("LASTITEM") ) {

			if ( lastItemName.Length < 1 ) {
				text = text.Replace ( "LASTITEM" , "une babiole" );
			} else {
				text = text.Replace ( "LASTITEM" , lastItemName );
				lastItemName = "";
			}

		}

		if ( text.Contains ("DIRECTIONTOFORMULA") ) {
			text = text.Replace ( "DIRECTIONTOFORMULA" , ClueManager.Instance.getDirectionToFormula () );
		}

		if ( text.Contains ("BOUNTY") ) {
			text = text.Replace ( "BOUNTY" , Karma.Instance.Bounty.ToString () );
		}

		if ( text.Contains ("FORMULA") ) {
			text = text.Replace ( "FORMULA" , ClueManager.Instance.getFormula () );
		}

		if ( text.Contains ("RANDOMFEMALENAME") ) {
			text = text.Replace ( "RANDOMFEMALENAME" , CrewCreator.Instance.FemaleNames[Random.Range (0,CrewCreator.Instance.FemaleNames.Length)]);
		}

		if ( text.Contains ("RANDOMMALENAME") ) {
			text = text.Replace ( "RANDOMMALENAME" , CrewCreator.Instance.MaleNames[Random.Range (0,CrewCreator.Instance.MaleNames.Length)]);
		}

		return text;
	}
	#endregion

	#region narrator
	public void ShowNarratorTimed (string text) {

		narratorObj.SetActive (true);

		narratorText.text = CheckForKeyWords (text);

		Invoke ("HideNarrator" , 2.5f );
	}
	public void ShowNarrator (string text) {
		
		narratorObj.SetActive (true);

		narratorText.text = CheckForKeyWords (text);
	}
	public void HideNarrator () {
		narratorObj.SetActive (false);
	}
	#endregion

	private void NextPhrase ()
	{
		++TextIndex;
		UpdateText ();
	}

	private void UpdateText () {
		bubble_Text.text = TextsToDisplay[TextIndex];
	}

	private void UpdateBubblePosition ()
	{
		if (target == null) {
			EndDialogue ();
			return;
		}

		// scale
		Vector3 scale = Vector3.one;

		scale.x = target.position.x < 0.5f ? 1 : -1;
		bubble_Image.localScale = scale;

//		// bubble decal
//		Vector3 decal = new Vector3 ( 
//			pos.x > 0.5f ? -speaker_Decal.x : speaker_Decal.x,
//			pos.y > 0.5f ? -speaker_Decal.y : speaker_Decal.y,
//			0);
//
//		pos += decal;

		// position
		bubble_Image.transform.position = target.position;


		// straighten text
		bubble_Text.transform.position = bubble_Image.transform.position;

	}

	public AudioClip[] SpeakSounds {
		get {
			return speakSounds;
		}
	}
}
