using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTest : MonoBehaviour {

	public RectTransform rectTransform;

	public float top = 0f;
	public float bottom = 0f;
	public float left = 0f;
	public float right = 0f;

	void Update () {

		rectTransform.offsetMin = new Vector2(left, bottom);
		rectTransform.offsetMax = new Vector2(-right, -top);

	}
}
