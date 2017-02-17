using UnityEngine;
using System.Collections;

public class NavigationTrigger : MonoBehaviour {

	public int texID = 0;
	public Texture2D[] arrowTextures;

	public void OnMouseEnter() {
		NavigationManager.Instance.CursorEnters (texID);
//		Cursor.SetCursor(arrowTextures[texID], hotSpot, CursorMode.Auto);
	}
	public void OnMouseExit() {
		NavigationManager.Instance.CursorExits (texID);
//		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
	public void OnMouseClick () {
		NavigationManager.Instance.Move (texID);
	}
}