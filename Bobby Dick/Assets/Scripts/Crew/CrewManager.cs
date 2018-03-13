﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CrewManager : MonoBehaviour {

		// managed crew
	public Crew managedCrew;

	List<CrewMember> crewMembers = new List<CrewMember> ();

	[SerializeField] private Crews.Side side;
	[SerializeField] private Transform[] crewAnchors;

	public int CurrentMemberCapacity {
		get {
			return Boats.playerBoatInfo.crewCapacity;
		}
		set {
			Boats.playerBoatInfo.crewCapacity = value;
		}
	}

	public int maxMemberCapacity = 4;

	private bool placingCrew = false;

	[SerializeField]
	private float placingDuration = 0.5f;

	void Start () {
		if (side == Crews.Side.Player) {
			NavigationManager.Instance.EnterNewChunk += AddToStates;
			StoryFunctions.Instance.getFunction += HandleGetFunction;
			NameGeneration.onDiscoverFormula += HandleOnDiscoverFormula;
		}
		CombatManager.Instance.onFightStart += HandleFightStarting;
	}

	void HandleOnDiscoverFormula (Formula Formula)
	{
		foreach (var item in CrewMembers) {
			item.AddXP ( 20 );
		}
	}

	void HandleFightStarting ()
	{
		foreach (var item in crewMembers) {
			item.ResetSkills ();
		}
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		if (func == FunctionType.ChangeTimeOfDay)
			AddToStates ();

	}

	void AddToStates ()
	{
		for (int i = 0; i < CrewMembers.Count; i++) {
			CrewMembers [i].UpdateHunger ();
		}
	}

	#region crew placement
	public void UpdateCrew ( Crews.PlacingType placingType ) {

		foreach ( CrewMember member in crewMembers ) {
			member.Icon.MoveToPoint (placingType);
		}
	}
	public float PlacingDuration {
		get {
			return placingDuration;
		}
		set {
			placingDuration = value;
		}
	}

	public Transform[] mapAnchors;

	public Transform[] CrewAnchors {
		get {
			return crewAnchors;
		}
	}
	#endregion

	#region crew list
	public void AddMember ( CrewMember member ) {

		if (crewMembers.Count == CurrentMemberCapacity)
			return;

		managedCrew.Add (member.MemberID);
		crewMembers.Add (member);
	}
	public void RemoveMember ( CrewMember member ) {

		if ( member.Icon != null )
			Destroy (member.Icon.gameObject);

		managedCrew.Remove (member.MemberID);
		crewMembers.Remove (member);

		if ( side == Crews.Side.Player) {

			if (CrewMembers.Count == 0) {
				GameManager.Instance.GameOver (1f);
				return;
			}

		}


	}

	public List<CrewMember> CrewMembers {
		get {
			return crewMembers;
		}
	}
	public CrewMember captain {
		get {
			return crewMembers [0];
		}
	}

	public void DeleteCrew () {

		foreach ( CrewMember member in CrewMembers )
			Destroy (member.Icon.gameObject);

		crewMembers.Clear ();
	}

	#endregion

	#region creation
	public void SetCrew (Crew crew) {

		DeleteCrew ();

		CrewCreator.Instance.TargetSide = side;

		for (int memberIndex = 0; memberIndex < crew.MemberIDs.Count; ++memberIndex ) {

			Member memberID = crew.MemberIDs [memberIndex];

			CrewMember member = CrewCreator.Instance.NewMember (memberID);
			CrewMembers.Add (member);

		}

		managedCrew = crew;

		Invoke ("ShowCrew",0.1f);
	}
	void ShowCrew () {
		UpdateCrew (Crews.PlacingType.Map);
	}
	#endregion
}
