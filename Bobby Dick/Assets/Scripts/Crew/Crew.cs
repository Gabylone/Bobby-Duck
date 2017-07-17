﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct CrewParams {

	public int level;

	public int amount;

	public bool overideGenre;
	public bool male;

}

public class Crew {

	public bool hostile = false;
	List<MemberID> memberIDs = new List<MemberID>();

	public int row = 0;
	public int col = 0;


	public Crew () {

	}

	public Crew (CrewParams crewParams, int r , int c) {

		row = r;
		col = c;

		if (crewParams.amount == 0) {
			int a = Random.Range ( Crews.playerCrew.CrewMembers.Count -1 ,Crews.playerCrew.CrewMembers.Count +1 );
			crewParams.amount = Mathf.Clamp (crewParams.amount, 1, 6);
		}

		for (int i = 0; i < crewParams.amount; ++i) {
			MemberID id = new MemberID (crewParams);
			if (crewParams.overideGenre) {
				id.Male = crewParams.male;
			}

			memberIDs.Add (id);
		}

	}

	public void Add ( MemberID id ) {
		memberIDs.Add (id);
	}

	public void Remove ( MemberID id ) {
		memberIDs.Remove (id);
	}

	public List<MemberID> MemberIDs {
		get {
			return memberIDs;
		}
	}
}