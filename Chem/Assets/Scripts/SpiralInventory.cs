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

    public int inventoryScale = 10;

    [SerializeField]
    private float angleDecal = 45f;

    public float scaleAmount = 2f;

    public float pointerAngle = 10f;

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
			if ( Input.GetButtonDown ("ShowIngredients") && elementTransforms.Count > 0) {
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
            onOpenInventory();

	}
	public void Close () {

		opened = false;

		pointerTransform.gameObject.SetActive (false);

		Character.Instance.ChangeState (Character.State.moving);

		group.SetActive (false);

        if (onCloseInventory != null)
            onCloseInventory();

	}
	#endregion

	void PointerUpdate ()
	{
		Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);

		pointerTransform.position = transform.position + dir.normalized * radius;

		for (int i = 0; i < elementTransforms.Count; i++) {

			if (Vector3.Angle (dir, (elementTransforms [i].position - transform.position).normalized) < pointerAngle) {

				if (selectedIndex == i)
					break;

				Tween.Descale (elementTransforms [selectedIndex]);
				Tween.Scale (elementTransforms [i], scaleAmount);

				selectedIndex = i;

			}

		}

		if (Input.GetButtonDown ("Fire2") ) {
			Select (selectedIndex);
		}
	}

	public virtual void Select (int i) {

        Tween.Descale(elementTransforms[i]);

        elementTransforms[i].transform.position = Character.Instance.transform.position + Vector3.up * 2f + Character.Instance.bodyTransform.right * 1.5f;

        elementTransforms.RemoveAt(i);



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

			float angle = angleDecal + (360f / inventoryScale) * elementIndex;

			var direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle), 0f);
			var targetPos = transform.position + (direction * radius);

            int a = 0;
            foreach (var item in elementTransforms[elementIndex].GetComponentsInChildren<SpriteRenderer>())
            {
                item.sortingOrder = 3 + a;

                ++a;
            }

            elementTransforms[elementIndex].position = transform.position;

			HOTween.To (elementTransforms [elementIndex], openDuration, "position", targetPos);

		}
	}
	#endregion

	public List<Transform> ElementTransforms {
		get {
			return elementTransforms;
		}
	}
}
