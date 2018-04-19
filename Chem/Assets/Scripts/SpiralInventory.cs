using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

public class SpiralInventory : MonoBehaviour {

	bool opened = false;

	[Header("Parameters")]
	[SerializeField]
	private float radius = 3f;

	[SerializeField]
	private float delay = 1f;

	[SerializeField]
	private GameObject group;

	[SerializeField]
	private Transform pointerTransform;

	[SerializeField]
	private List<Transform> elementTransforms;

	private float openDuration = 1f;

	public delegate void OnOpenInventory ();
	public OnOpenInventory onOpenInventory;

	public delegate void OnCloseInventory ();
	public OnCloseInventory onCloseInventory;

	[SerializeField]
	private float angleDecal = 45f;

	int selectedIndex = 0;


	public virtual void Start () {
		Close ();
	}

	public virtual void Update () {

		if ( opened ) 
		{
			PointerUpdate ();

			if ( Input.GetButtonDown ("ShowIngredients") ) {
				IngredientsSpiral.Instance.Close ();
			}
		}
		else
		{
			if ( Input.GetButtonDown ("ShowIngredients") ) {
				IngredientsSpiral.Instance.Open ();
			}
		}
	}

	#region open / close
	public void Open () {

		transform.position = Character.Instance.getTransform.position;

		opened = true;

		pointerTransform.gameObject.SetActive (true);

		UpdateElementPositions ();

		group.SetActive (true);

		if (onOpenInventory != null)
			onOpenInventory ();
	}
	public void Close () {
		opened = false;

		pointerTransform.gameObject.SetActive (false);

		Character.Instance.ChangeState (Controller.State.state2);

		group.SetActive (false);

		if (onCloseInventory != null)
			onCloseInventory ();
	}
	#endregion

	void PointerUpdate ()
	{
//		Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);
//		Vector3 dir = new Vector3( Input.GetAxis("Mouse X") , Input.GetAxis("Mouse Y") , 0f );
		Vector3 dir = new Vector3( Input.GetAxis("Horizontal") , Input.GetAxis("Vertical") , 0f );

		pointerTransform.position = transform.position + dir.normalized * radius;

		float angle = (360f / elementTransforms.Count / 2f);

		for (int i = 0; i < elementTransforms.Count; i++) {

			if (Vector3.Angle (dir, (elementTransforms [i].position - transform.position).normalized) < angle) {

				if (selectedIndex == i)
					break;

				Tween.Descale (elementTransforms [selectedIndex]);
				Tween.Scale (elementTransforms [i]);

				selectedIndex = i;

//				elementTransforms [i].GetComponentInChildren<SpriteRenderer> ().color = Color.red;

			}

		}

		if (Input.GetButtonDown ("Fire2")) {
			Select (selectedIndex);
		}
	}

	public virtual void Select (int i) {

		elementTransforms.RemoveAt (i);

		Close ();

	}

	#region items
	public void AddItem ( Transform t ) {
		t.SetParent (group.transform);
		elementTransforms.Add (t);
	}

	#endregion

	#region lerp 
	void UpdateElementPositions () {

		for (int elementIndex = 0; elementIndex < elementTransforms.Count; elementIndex++) {

			Transform t = elementTransforms [elementIndex];

			float angle = angleDecal + (360f / elementTransforms.Count) * elementIndex;

			var direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle), 0f);
			var targetPos = transform.position + (direction * radius);

			HOTween.To (elementTransforms [elementIndex], openDuration, "position", targetPos);

		}

//		for (int elementIndex = 0; elementIndex < elementTransforms.Count; elementIndex++) {
//
//			Transform t = elementTransforms [elementIndex];
//
//			Vector2 initPos = transform.position;
//
//			float angle = angleDecal + (360f / elementTransforms.Count) * elementIndex;
//
//			var direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle), 0f);
//			var targetPos = transform.position + (direction * radius);
//
//			t.position = targetPos;
//		}

	}
	#endregion

	public List<Transform> ElementTransforms {
		get {
			return elementTransforms;
		}
	}
}
