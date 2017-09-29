using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMemberIcons : MonoBehaviour {

	public Crews.Side side;

	public RectTransform parent;

	public MemberIcon[] memberIcons;

	void Awake () {
		Instances [(int)side] = this;
	}

	// Use this for initialization
	void Start () {

		memberIcons = parent.GetComponentsInChildren<MemberIcon> (true);

		int index = 0;
		foreach (var item in memberIcons) {
			
//			item.GetTransform.SetParent (parent);
			item.index = index;

			++index;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#region static
	public static DisplayMemberIcons[] Instances = new DisplayMemberIcons[2];
	public static DisplayMemberIcons GetInstance ( Crews.Side side ) {
		return Instances [(int)side];
	}
	#endregion
}
