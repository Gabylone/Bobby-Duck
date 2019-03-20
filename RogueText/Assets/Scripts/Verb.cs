using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Verb {

	public int col = 0;

	public static List<Verb> verbs = new List<Verb>();

    public string question = "";

    public string helpPhrase = "";

    public bool availableForAllItems = false;

	public string[] names;

	public Dictionary<int,string> cellContents = new Dictionary<int, string> ();
//	public List<string> cellContents = new List<string> ();


	public Verb() {
		//
	}

	public static Verb Find ( string str ) {

		foreach (var verb in verbs) {

			foreach (var name in verb.names) {

				if (name == str) {
					return verb;
				}

			}

		}

		return null;
	}
}