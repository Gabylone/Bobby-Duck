using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiralInventory : MonoBehaviour {

	bool opened = false;

	[Header("Parameters")]
	[SerializeField]
	private float radius = 3f;

	[SerializeField]
	private float delay = 1f;

	[SerializeField]
	private Transform pointerTransform;

	[SerializeField]
	private List<Transform> elementTransforms;

	[Header("lerp")]
	private float lerpTimer = 0f;
	[SerializeField]
	private float lerpDuration = 1f;
	private bool lerping = false;

	[SerializeField]
	private float angleDecal = 45f;

	bool showObjects = false;

	float timer = 0f;
	[SerializeField]
	private float quickTime = 1f;
	bool quick = false;

	public virtual void Start () {
		Opened = false;
	}

	public virtual void Update () {

		if (lerping) {
			Lerp ();
		} else if ( opened && !quick ) {
			Opened_Update ();
		}
	}

	public bool Opened {
		get {
			return opened;
		}
		set {

			opened = value;

			lerpTimer = 0f;

			lerping = true;

			pointerTransform.gameObject.SetActive (value);

			if (value) {
				ShowObjects = true;
			} else {
				Character.Instance.ChangeState (Character.State.moving);
			}
		}
	}


	  public void QuickShow() {
		Opened = true;
		quick = true;
		pointerTransform.gameObject.SetActive (false);
	}

	void Opened_Update ()
	{
		Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);

		pointerTransform.position = transform.position + dir.normalized * radius;

		int selectedIndex = 0;

		float angle = (360f / elementTransforms.Count / 2f);

		for (int i = 0; i < elementTransforms.Count; i++) {

			if (Vector3.Angle (dir, (elementTransforms [i].position - transform.position).normalized) < angle) {

				selectedIndex = i;

				elementTransforms [i].GetComponentInChildren<SpriteRenderer> ().color = Color.red;
			} else {
				elementTransforms [i].GetComponentInChildren<SpriteRenderer> ().color = Color.white;
			}

		}

		if (Input.GetButtonDown ("Fire2")) {
			Select (selectedIndex);
		}
	}

	public virtual void Select (int i) {

		Opened = false;

	}

	public void AddItem ( Transform t ) {
		elementTransforms.Add (t);
		QuickShow ();
	}

	public bool ShowObjects {
		get {
			return showObjects;
		}
		set {
			showObjects = value;

			foreach ( Transform tr in elementTransforms ) {
				tr.gameObject.SetActive (value);
			}
		}
	}

	#region lerp 
	void Lerp () {

		for (int elementIndex = 0; elementIndex < elementTransforms.Count; elementIndex++) {

			Transform t = elementTransforms [elementIndex];

			float l = Opened ? (lerpTimer / lerpDuration) : 1 - (lerpTimer / lerpDuration);

			Vector2 initPos = transform.position;

			float angle = angleDecal + (360f / elementTransforms.Count) * elementIndex;
			float currentAngle = Mathf.Lerp (0f, angle, l);

			var direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * currentAngle), Mathf.Cos(Mathf.Deg2Rad * currentAngle), 0f);

			var targetPos = transform.position + (direction * radius);


			t.position = Vector2.Lerp (initPos , targetPos,l);
		}


		if ( lerpTimer >= lerpDuration ) {

			if (!opened) {
				ShowObjects = false;
				lerping = false;
			} else {
				if ( quick ) {
					if (lerpTimer >= lerpDuration + quickTime) {
						lerping = false;
						quick = false;
						Opened = false;
					}
				} else {
					lerping = false;
				}

			}
		}

		lerpTimer += Time.deltaTime;

	}
	#endregion

	public List<Transform> ElementTransforms {
		get {
			return elementTransforms;
		}
	}
}
