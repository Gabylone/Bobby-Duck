using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryCard : Card {

	[Header("Deploy")]
	[SerializeField]
	private float fullScale = 200f;
	private float initScale = 100f;

	[SerializeField]
	private RectTransform backGroundTransform;

	private Vector3[] stats_InitPos = new Vector3[3];
	[SerializeField]
	private Transform[] stats_DeployedAnchors;

	[SerializeField]
	private GameObject itemParent;
	private ItemButton[] itemButtons;

	[SerializeField]
	private Transform[] stats_Transforms;

	private bool deployed = false;

	void Start () {
		
		Init ();

		initScale = backGroundTransform.sizeDelta.y;

		itemButtons = itemParent.GetComponentsInChildren<ItemButton> ();

		int a = 0;
		foreach ( Transform statTransform in stats_Transforms ) {
			stats_InitPos[a] = statTransform.position;
			++a;
		}

		Deployed = false;

	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.O))
			Deployed = !Deployed;
	}

	public void Deploy () {

		Vector2 scale = backGroundTransform.sizeDelta;
		scale.y = fullScale;
		backGroundTransform.sizeDelta = scale;

		CrewMember crewMember = CrewNavigator.Instance.SelectedMember;

		int a = 0;
		foreach (ItemButton itemButton in itemButtons) {

			stats_Transforms [a].transform.position = stats_DeployedAnchors [a].position;

			itemButton.gameObject.SetActive (crewMember.Equipment [a] != null);
			if ( crewMember.Equipment [a] != null ) {
				itemButton.Name = crewMember.Equipment [a].name;
				itemButton.Param = crewMember.Equipment [a].value;
				itemButton.Price = crewMember.Equipment [a].price;
			}
			++a;
		}
	}

	public void Reset () {
		
		Vector2 scale = backGroundTransform.sizeDelta;
		scale.y = initScale;
		backGroundTransform.sizeDelta = scale;

		int a = 0;
		foreach (ItemButton itemButton in itemButtons) {

			stats_Transforms [a].transform.position = stats_InitPos[a];

			itemButton.gameObject.SetActive (false);

		}
	}

	public bool Deployed {
		get {
			return deployed;
		}
		set {
			deployed = value;

			if (deployed == true)
				Deploy ();
			else
				Reset ();
		}
	}
}
