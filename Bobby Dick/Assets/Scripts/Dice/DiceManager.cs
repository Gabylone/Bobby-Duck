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

	[Header("Dice")]
	//[SerializeField]
	public float settlingDuration = 0.5f;

	[SerializeField]
	private Color[] diceColors;

	[SerializeField]
	private float throwDuration;
	private int throwDirection = 1;

	[SerializeField]
	private Dice[] dices;

	private Throw currentThrow;

	private int highestResult = 0;

	private bool throwing = false;
	float timer = 0f;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
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

	#region init
	private void InitDice () {
		foreach ( Dice die in dices) {
			die.Init ();
		}
	}
	private void ResetDice () {
		foreach ( Dice die in dices) {
			die.Reset ();
		}
	}
	#endregion

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
			dices[i].Throw ();
		}
	}
	private void Throwing_Update () {
		if ( timeInState > throwDuration) {
			ChangeState (states.showingHighest);
		}
	}
	private void Throwing_Exit () {
		

	}
	#endregion

	#region showing highest
	private void ShowingHighest_Start () {
		
		throwing = false;

		int highestThrowIndex = 0;

		int a = 0;

		for (int diceIndex = 0; diceIndex < CurrentThrow.diceAmount; diceIndex++) {
			if (dices[diceIndex].result > highestResult) {
				highestThrowIndex = a;
			}

			++a;
		}

		highestResult = dices [highestThrowIndex].result;

		dices[highestThrowIndex].Settle ();
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

	#region paint dice
	public Color DiceColors (DiceTypes type) {
		return diceColors [(int)type];
	}

	private void PaintDice ( DiceTypes type ) {
		foreach ( Dice dice in dices ) {
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

	public bool Throwing {
		get {
			return throwing;
		}
		set {
			throwing = value;
		}
	}
	public int HighestResult {
		get {
			return highestResult;
		}

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

}



public class Throw {

	public int diceAmount = 0;

	public DiceTypes diceType;

	public int highestResult = 0;

	public Throw ( int _amount , DiceTypes type ) {
		diceAmount = _amount;
		diceType = type;
	}

}
