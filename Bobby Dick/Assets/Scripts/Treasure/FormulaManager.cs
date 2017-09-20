using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FormulaManager : MonoBehaviour {

	public static FormulaManager Instance;

	[SerializeField]
	private int formulaAmount = 2;



	public Formula[] formulas;

	[SerializeField]
	private GameObject formulaGroup;

	[SerializeField]
	private InputField inputField;

	void Awake () {
		Instance = this;
	}

	void Start () {
		StoryFunctions.Instance.getFunction += HandleGetFunction;
	}

	void Update () {
		if ( Input.GetKeyDown(KeyCode.J) ) {
			print ( getDirectionToFormula() );
		}
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.CheckClues:
			StartFormulaCheck ();
			break;
		}
	}

	public void Init () {

	}

	public void CreateNewClues () {

		formulas = new Formula[formulaAmount];

		for (int i = 0; i < formulaAmount; ++i) {

			Formula newFormula = new Formula ();
			newFormula.name = NameGeneration.Instance.randomWord;
			newFormula.coords = MapGenerator.Instance.RandomCoords;


			Chunk.GetChunk (newFormula.coords).IslandData = new IslandData (IslandType.Clue);

			formulas [i] = newFormula;

		}
	}

	#region formula check
	void StartFormulaCheck () {
		formulaGroup.SetActive (true);
		StoryReader.Instance.NextCell ();
	}

	public void CheckFormula () {

		string stringToCheck = inputField.text.ToLower();
		inputField.text = "";
		formulaGroup.SetActive (false);

		Formula containedFormula = System.Array.Find (formulas, x => stringToCheck.Contains (x.name.ToLower ()));

		if ( containedFormula == null ) {
			print ("input field does not contain any formulas");
			StoryReader.Instance.SetDecal (0);
			StoryReader.Instance.UpdateStory ();
			return;
		}

		if ( containedFormula.verified ) {
			print ("formula is already verified... need another one");
			StoryReader.Instance.SetDecal (0);
			StoryReader.Instance.UpdateStory ();
			return;
		}

		containedFormula.verified = true;
		print ("la formule est bonne");

		bool allFormulasHaveBeenVerified = true;

		foreach (var formula in formulas) {
			if ( formula.verified== false) {
				allFormulasHaveBeenVerified = false;
			}
		}

		if ( allFormulasHaveBeenVerified ) {
			print ("toutes les formules sont vérifiées, il faut ouvrir la porte");
			StoryReader.Instance.SetDecal (2);
			StoryReader.Instance.UpdateStory ();
		} else {
			print ("il reste des formules à vérifier");
			StoryReader.Instance.SetDecal (1);
			StoryReader.Instance.UpdateStory ();
		}



	}
	#endregion

	public string getDirectionToFormula () {
		Directions dir = NavigationManager.Instance.getDirectionToPoint (FormulaManager.Instance.GetNextClueIslandPos);
		string directionPhrase = NavigationManager.Instance.getDirName (dir);

		return directionPhrase;
	}

	public string getFormula () {

		Formula formula = System.Array.Find(formulas,x=>x.coords == Boats.PlayerBoatInfo.CurrentCoords);

		formula.found = true;

		return "<b>" + formula.name.ToUpper() + "</b>";
	}

	public Vector2 GetNextClueIslandPos {
		get {
			int a = 0;
			foreach (var form in formulas) {
				if ( !form.found ) {
					print (a.ToString ());
					return (Vector2)form.coords;
				}
				++a;
			}

			return (Vector2)MapData.Instance.treasureIslandCoords;

		}
	}

	public void LoadFormulas ()
	{
		formulas = SaveManager.Instance.CurrentData.formulas;
	}

	public void SaveFormulas () {
		SaveManager.Instance.CurrentData.formulas = formulas;
	}
}

[System.Serializable]
public class Formula {
	public string 	name;
	public Coords 	coords;
	public bool 	verified	= false;
	public bool 	found		= false;

	public Formula () {
		//
	}
}