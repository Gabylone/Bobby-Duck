using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DiceTypes {
	STR,
	DEX,
	CHA,
	CON,
}

public class DiceManager : MonoBehaviour {
		
	// singleton
	public static DiceManager Instance;

		// STATES
	public enum states {
		none,

		throwing,
		settling,
		removingCriticals,
		showingHighest,
	}
	private states previousState;
	private states currentState;

	float timeInState = 0f;

	private delegate void UpdateState ();
	UpdateState updateState;

		// STATES

	[Header ("Elements")]
	[SerializeField]
	private GameObject feedbackDice;

	[Header("Dice")]
	[SerializeField]
	private GameObject[] diceObjects;
	//[SerializeField]
	public float settlingDuration = 0.5f;

	[SerializeField]
	private Color[] diceColors;

	[SerializeField]
	private float throwDuration;
	int throwDirection = 1;

	private Dice[] diceClass;

	Throw currentThrow;

	private bool throwing = false;
	float timer = 0f;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {

		diceClass = new Dice[diceObjects.Length];

		int a = 0;
		foreach ( GameObject obj in diceObjects ) {
			diceClass [a] = obj.GetComponent<Dice> ();
			++a;
		}

		InitDice ();
		ResetDice ();
	}
	
	// Update is called once per frame
	void Update () {

		if ( updateState != null ) {
			updateState ();
			timeInState += Time.deltaTime;

		}
	}

	#region throwing
	public void ThrowDice (DiceTypes type, int diceAmount) {

		ResetDice ();

		throwing = true;

		currentThrow = new Throw (diceAmount, type);

		ChangeState (states.throwing);

	}
	
	private void Throwing_Start () {
		
		PaintDice (currentThrow.diceType);
		
		for (int i = 0; i < currentThrow.diceAmount ; ++i) {
			diceClass[i].Throw ();
		}
	}
	private void Throwing_Update () {
		if ( timeInState > throwDuration) {
			ChangeState (states.showingHighest);
		}
	}
	private void Throwing_Exit () {
		

	}

	int highestThrow = 0;

	public int getHighestThrow {
		get {
			return highestThrow;
		}

	}
	#endregion

	#region showing highest
	private void ShowingHighest_Start () {
		
		throwing = false;

		int highestResult = diceClass[0].result;
		highestThrow = 0;

		int a = 0;

		foreach ( Dice die in diceClass ) {

			if (die.result > highestResult) {
				highestThrow = a;
				highestResult = die.result;
			}

			++a;
		}

		diceClass[highestThrow].Settle ();

		//
	}
	private void ShowingHighest_Update () {
		if (timeInState > settlingDuration) {
			ChangeState (states.none);
		}
	}
	private void ShowingHighest_Exit () {

		ResetDice ();
	}
	#endregion

	#region states
	public void ChangeState ( states newState ) {
		previousState = currentState;
		currentState = newState;

		switch (previousState) {
		case states.throwing :
			Throwing_Exit ();
			break;
		case states.showingHighest :
			ShowingHighest_Exit ();
			break;
		case states.none :
			// nothing
			break;
		}

		switch (currentState) {
		case states.throwing :
			updateState = Throwing_Update;
			Throwing_Start ();
			break;
		case states.showingHighest :
			updateState = ShowingHighest_Update;
			ShowingHighest_Start ();
			break;

		case states.none :
			updateState = null;
			break;
		}

		timeInState = 0f;
	}
	#endregion

	#region throw
	private void ResetDice () {
		foreach ( Dice die in diceClass) {
			die.Reset ();
		}
	}
	private void InitDice () {
		foreach ( Dice die in diceClass) {
			die.Init ();
		}
	}
	public bool Throwing {
		get {
			return throwing;
		}
		set {
			throwing = value;
		}
	}
	#endregion

	#region paint dice
	public Color DiceColors (DiceTypes type) {
		return diceColors [(int)type];
	}

	private void PaintDice ( DiceTypes type ) {
		foreach ( Dice dice in diceClass ) {
			dice.Paint (type);
		}
	}
	#endregion

	#region properties
	public float ThrowDuration {
		get {
			return throwDuration;
		}
	}

	public int ThrowDirection {
		get {
			return throwDirection;
		}
		set {
			throwDirection = value;
		}
	}

	public Throw CurrentThrow {
		get {
			return currentThrow;
		}
		set {
			currentThrow = value;
		}
	}
	#endregion
}



public class Throw {

	public enum Results {
		CritFailure,
		Failure,
		Success,
		CritSuccess,
	}

	public Results s;

	public List<int> results = new List<int>();

	public int diceAmount = 0;

	public DiceTypes diceType;

	public int highestResult = 0;

	private Dice[] dice;

	public Throw ( int _amount , DiceTypes type ) {
		diceAmount = _amount;
		diceType = type;
	}

	public void Add ( Dice dice ) {
		
		if (dice.result > highestResult) {
			
			highestResult = dice.result;
		}



		results.Add ( dice.result );

	}

	public void Add ( int i ) {
		results.Add ( i );

		if (i > highestResult)
			highestResult = i;
	}

	public Results Result ( int valueToCompare ) {

		if ( highestResult == 6 )
			return Results.CritSuccess;
//
//		if (highestResult > valueToCompare)
//			return Results.Success;

		if (highestResult == 1)
			return Results.Failure;

		return Results.Success;
	}

}
