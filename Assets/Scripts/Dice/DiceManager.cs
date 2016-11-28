using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DiceTypes {
	Attack,
	Speed,
	Const,
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

		throwDirection = 1;

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

		currentThrow = new Throw (1, type);
//		currentThrow = new Throw (diceAmount, type);

		ChangeState (states.throwing);

	}
	
	private void Throwing_Start () {

		throwing = true;
		
		PaintDice (currentThrow.diceType);
		
		for (int i = 0; i < currentThrow.diceAmount ; ++i) {
			diceClass[i].Throw ();
		}
	}
	private void Throwing_Update () {
		if ( timeInState > throwDuration) {
			ChangeState (states.settling);
		}
	}
	private void Throwing_Exit () {
		

	}
	#endregion

	#region settling
	private void Settling_Start () {
		for (int i = 0; i < currentThrow.diceAmount ; ++i) {
			diceClass[i].Settle ();
		}
	}
	private void Settling_Update () {
		if (timeInState > settlingDuration) {
			CheckResults ();
		}
	}
	private void Settling_Exit () {


	}
	public void CheckResults () {

		for ( int i = 0; i < currentThrow.diceAmount; ++i) {
			currentThrow.Add (diceClass [i].result);
		}

		bool criticalFailure = false;
		bool criticalSuccess = false;
		
		foreach (int result in currentThrow.results) {
			
			if (result == 1)
				criticalFailure = true;
			
			if (result == 6)
				criticalSuccess = true;
			
		}

		ChangeState (states.showingHighest);

		/*if (criticalFailure || criticalSuccess) {
			ChangeState (states.removingCriticals);
		} else {
			ChangeState (states.showingHighest);
		}*/
	}
	#endregion

	#region removing criticals
	private void RemovingCriticals_Start () {
		//
	}
	private void RemovingCriticals_Update () {
		//
	}
	private void RemovingCriticals_Exit () {
		//
	}
	#endregion

	#region showing highest
	private void ShowingHighest_Start () {
		throwing = false;
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
		case states.settling :
			Settling_Exit();
			break;
		case states.removingCriticals :
			RemovingCriticals_Exit();
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
		case states.settling :
			updateState = Settling_Update;
			Settling_Start ();
			break;
		case states.removingCriticals :
			updateState = RemovingCriticals_Update;
			RemovingCriticals_Start ();
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

	#region feedback
	public void ShowFeedbackDice () {
		feedbackDice.SetActive (true);
	}
	public void HideFeedbackDice () {
		feedbackDice.SetActive (false);
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
