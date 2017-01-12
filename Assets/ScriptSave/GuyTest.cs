using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuyTest : MonoBehaviour {

	public static GuyTest Instance;

	private string[] maleNames = new string[51] {
		"Jean","Eric", "Nathan", "Jacques", "Benoit", "Jeremy", "Jerome", "Bertrand", "Vladimir", "Dimitri", "Jean-Jacques", "Gérard", "Nestor", "Etienne", "Leon", "Henry", "David", "Esteban", "Louis", "Carles", "Victor", "Michel", "Gabriel", "Pierre", "André", "Fred", "Cassius", "César", "Paul", "Martin", "Claude", "Levis", "Alex", "Olivier", "Mustafa", "Nicolas", "Chris", "Oleg", "Emile", "Richard", "Romulus", "Rufus", "Stan", "Charles", "Quincy", "Antoine", "Virgile", "Boromir", "Archibald", "Eddy", "Kenneth"
	};

	private string[] femaleNames = new string[51] {
		"Jeanne","Erica", "Nathalie", "Jacquelines", "Barbara", "Ella", "Flo", "Laura", "Natasha", "Irene", "Yvonne", "Gérarde", "Nelly", "Elisa", "Adele", "Henriette", "Alice", "Esteban", "Louise", "Carla", "Victoria", "Michelle", "Gabrielle", "Sarah", "Andréa", "Marion", "Valentine", "Cléopatre", "Pauline", "Martine", "Claudette", "Nina", "Alexandra", "Clementine", "Julia", "Olivia", "Christine", "Rose", "Emilia", "Agathe", "Lily", "Claire", "Yasmine", "Charlotte", "Scarlett", "Marina", "Virginie", "Anaïs", "Tatiana", "Cécile", "Marianne"
	};

	public Image guy;
	public Text name;

	// Use this for initialization
	void Awake () {
		Instance = this;
	}

	void Start () {
		SaveManager.Instance.loadData += Load;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Randomize () {
		int r = Random.Range (0,51);

		name.text = Random.value < 0.5f ? femaleNames[r] : maleNames[r];
		guy.color = Random.ColorHSV ();

		SaveManager.Instance.CurrentData.guyColor = guy.color;
		SaveManager.Instance.CurrentData.guyName = name.text;
	}

	public void Load () {
		name.text = SaveManager.Instance.CurrentData.guyName;
		guy.color = SaveManager.Instance.CurrentData.guyColor;
	}
}
