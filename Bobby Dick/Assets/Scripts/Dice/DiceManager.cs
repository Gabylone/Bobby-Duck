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
		showingHighest,
	}
	private states previousState;
	private states currentState;

	float timeInState = 0f;

	private delegate void UpdateState ();
	UpdateState updateState;

		// STATES

	[Header("Dice")]
	[SerializeField]
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
	public int QuickThrow (int diceAmount) {

		int result = 0;

		int[] quickDices = new int[diceAmount];
		for (int i = 0; i < diceAmount; i++) {
			quickDices [i] = Random.Range (1, 7);
		}

		for (int diceIndex = 0; diceIndex < diceAmount; diceIndex++) {
			if (quickDices[diceIndex] > result) {
				result = quickDices [diceIndex];
			}
		}

		return result;
	}

	public void ThrowDice (DiceTypes type, int diceAmount) {

		ResetDice ();

		Throwing = true;

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

//		if ( InputManager.Instance.OnInputDown() ) {
//			ChangeState (states.showingHighest);
//			return;
//		}

		if ( timeInState > throwDuration) {
			ChangeState (states.showingHighest);
		}
	}
	private void Throwing_Exit () {
		

	}
	#endregion

	#region showing highest
	private void ShowingHighest_Start () {
		
		Throwing = false;

		Dice highestDie = dices [0];

		highestResult = 0;

		int index = 0;

		for (int diceIndex = 0; diceIndex < CurrentThrow.diceAmount; diceIndex++) {
			if (dices[diceIndex].result > highestResult) {
				highestResult = dices [diceIndex].result;
				highestDie = dices [diceIndex];
				index = diceIndex;
			}
		}

		highestResult = highestDie.result;

		for (int diceIndex = 0; diceIndex < CurrentThrow.diceAmount; diceIndex++) {
			if (diceIndex != index)
				dices [diceIndex].SettleDown 
				();
		}


//		highestDie. ();

		//
	}
	private void ShowingHighest_Update () {
		if ( InputManager.Instance.OnInputDown() ) {
			ChangeState (states.none);
			return;
		}

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
