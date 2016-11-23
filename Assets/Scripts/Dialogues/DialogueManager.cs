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

	private bool DisplayingText = false;

	[Header("Parameters")]
	[SerializeField]
	private Vector3 speaker_Decal = Vector3.up * 2f;
	private Transform speaker_Transform;

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
	public void SetDialogue (string[] phrases, Transform t) {
		DialogueTexts = phrases;

		speaker_Transform = t;

		StartDialogue ();
	}
	public void SetDialogue (string phrase, Transform t) {

		DialogueTexts = new string[1]{ phrase };

		speaker_Transform = t;

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

		SoundManager.Instance.PlayRandomSound ( speakSounds );
	}

	private void UpdateDialogue () {
		
		if (CurrentTime > 0)
		{
			CurrentTime -= Time.deltaTime;

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
	private void EndDialogue ()
	{
		DisplayingText = false;

		bubble_Obj.SetActive (false);
	}
	#endregion

	#region narrator
	public void ShowNarrator (string text) {
		narratorObj.SetActive (true);

		narratorText.text = text;

		if ( !IslandManager.Instance.OnIsland ) {
			Invoke ("HideNarrator" , 2.5f );
		}
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
		if (speaker_Transform == null) {
			EndDialogue ();
			return;
		}

		// get viewport position of target object
		Vector3 pos = Camera.main.WorldToViewportPoint (speaker_Transform.position);

			// clamp bubble
		pos.x = Mathf.Clamp (pos.x, bubbleBounds.x, 1 - bubbleBounds.x);
		pos.y = Mathf.Clamp (pos.y, bubbleBounds.y, 1 - bubbleBounds.y);

		// scale
		Vector3 scale = Vector3.one;

		scale.x = pos.x < 0.5f ? 1 : -1;
		scale.y = pos.y < 0.5f ? 1 : -1;

		bubble_Image.localScale = scale;

		// bubble decal
		Vector3 decal = new Vector3 ( 
			pos.x > 0.5f ? -speaker_Decal.x : speaker_Decal.x,
			pos.y > 0.5f ? -speaker_Decal.y : speaker_Decal.y,
			0);

		pos += decal;

		// position
		bubble_Image.anchorMin = pos;
		bubble_Image.anchorMax = pos;


		// straighten text
		bubble_Text.transform.position = bubble_Image.transform.position;


	}
}
