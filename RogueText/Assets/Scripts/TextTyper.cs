using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTyper : MonoBehaviour {

	public GameObject group;
	public Text uiText;

	public float typeRate = 0.01f;

    public bool debug = false;

	public string textToType = "";
	int characterIndex = 0;
	public int letterRate = 3;

    string typingText = "";

    private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";

	public bool finished = false;

    bool colorTyping = false;
    int colorcharacterIndex = -1;

	public void Clear () {
        
        if ( debug)
        {
            Debug.Log("clearing feedback");
        }

		textToType = "";
		uiText.text = "";
	}

	public virtual void Start () {

		uiText = group.GetComponentInChildren<Text> ();

        Clear();
         
	}

    public IEnumerator TypeCoroutine () {

        if ( debug)
        {
            Debug.Log("TEXT TO TYPE START : " + textToType);
        }

        while ( true ) {

            for (int i = 0; i < letterRate; i++)
            {
                TypeCharacter();

                if (characterIndex >= textToType.Length)
                {
                    if (debug)
                    {
                        Debug.Log("breaking in for : character index egal or sup to texttotype.L");
                    }
                    break;
                }
            }

            uiText.text = typingText + "_";

            Sound.Instance.PlayRandomTypeSound();

            yield return new WaitForSeconds(typeRate);

            if (characterIndex >= textToType.Length)
            {
                if (debug)
                {
                    Debug.Log("TEXT TO TYPE END : " + textToType);
                    Debug.Log("TYPING TEXT : " + typingText);
                    Debug.Log("breaking at end : character index egal or sup to texttotype.L");
                }
                uiText.text = typingText;
                break;
            }
        }

		Sound.Instance.PlayRandomComputerSound ();

		finished = true;

	}

    public void Hide()
    {
        group.SetActive(false);
        finished = true;
    }

    public void UpdateAndDisplay()
    {
        UpdateCurrentTileDescription();
        Display();
    }

    void TypeCharacter()
    {
        char currentChar = textToType[characterIndex];

        if (currentChar == '<')
        {
            char colorCharacter = textToType[characterIndex + 1];

            colorcharacterIndex = TextManager.Instance.colorCharacters.FindIndex(x => x == colorCharacter);

            colorTyping = true;

            textToType = textToType.Remove( characterIndex,2 );
            return;
        }

        if (currentChar == '>')
        {
            colorTyping = false;

            textToType = textToType.Remove(characterIndex, 1);
            return;
        }

        if (colorTyping)
        {
            string part = "" + textToType[characterIndex];

            string htmlColor;

            if ( colorcharacterIndex >= TextManager.Instance.colors.Length)
            {
                htmlColor = ColorUtility.ToHtmlStringRGB(Color.green);
            }
            else
            {
                htmlColor = ColorUtility.ToHtmlStringRGB(TextManager.Instance.colors[colorcharacterIndex]);
            }

            typingText += "<color=#" +
                htmlColor +
                ">" + part + "</color>";
        }
        else
        {
            string part = "" + textToType[characterIndex];


            typingText += part;
        }

        NextCharacter();

    }

    void NextCharacter()
    {
        ++characterIndex;
    }

    public string WithCaps (string str)
    {
        return str [0].ToString ().ToUpper () + str.Remove (0,1).ToLower();
    }

    public void Display()
    {
        Display(textToType);
    }

    public virtual void Display ( string str ) {

        StopCoroutine(TypeCoroutine());
        uiText.text = "";
        textToType = str;
		UpdateText ();
	}

    public virtual void UpdateCurrentTileDescription()
    {

    }

	public void UpdateText () {

		uiText.text = "";

		if (textToType.Length < 1) {
            Hide();
			return;
		}

		group.SetActive (true);

		finished = false;
        characterIndex = 0;
        typingText = "";

        StartCoroutine (TypeCoroutine ());

	}
	public void QuickUpdateText () {

		uiText.text = textToType;
		textToType = "";
	}
}
