using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IslandInfo : MonoBehaviour {

	[Header("Island Info")]
	[SerializeField]
	private GameObject obj;
	[SerializeField]
	private RectTransform rect;
	[SerializeField]
	private Text uiText;

	// Use this for initialization
	void Start () {
		Visible = false;
		MapImage.Instance.showIslandInfo += ShowIslandInfo;
	}
	
	public void ShowIslandInfo ( string info , Vector2 p ) {
		
		Visible = true;

		if ( p.x > 0 ) {
			p.x -= rect.rect.width;
		}

		if ( p.y > 0 ) {
			p.y -= rect.rect.height;
		}

		rect.transform.localPosition = p;
		uiText.text = info;
		//
	}
	public void HideIslandInfo () {
		Visible = false;
	}

	public bool Visible {
		get {
			return obj.activeSelf;
		}
		set {
			obj.SetActive (value);
		}
	}
}
