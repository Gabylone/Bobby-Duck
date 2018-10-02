using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class Tween : MonoBehaviour {

	public static float defaultAmount = 1.2f;
	public static float defaultDuration = 0.2f;

	public static Transform lastTransform;

	public static void Scale (Transform t, float dur , float amount) {

		EaseType eT = amount > 1 ? EaseType.EaseOutBounce : EaseType.Linear;
		HOTween.To ( t , dur , "localScale" , Vector3.one * amount , false , eT , 0f );
	}

	public static void Bounce (Transform t) {
		Bounce ( t, defaultDuration , defaultAmount );
	}

	public static void Bounce (Transform t, float dur , Vector3 targetScale , float amount ) {
		HOTween.To ( t , dur/2f, "localScale" , targetScale * amount , false , EaseType.EaseOutBounce , 0f);
		HOTween.To ( t , dur/2f, "localScale" , targetScale , false , EaseType.Linear , dur/2f);
	}
	public static void Bounce (Transform t, float dur , float amount) {
		HOTween.To ( t , dur/2f, "localScale" , Vector3.one * amount , false , EaseType.EaseOutBounce , 0f);
		HOTween.To ( t , dur/2f, "localScale" , Vector3.one , false , EaseType.Linear , dur/2f);
	}

	#region fade ui
	public static void FadeUI (Transform t, float dur ) {

		foreach ( Image image in t.GetComponentsInChildren<Image>(true) ) {
			Color c = image.color;
			c.a = 0f;
			HOTween.To ( image, dur , "color" , c  );
		}

		foreach ( Text text in t.GetComponentsInChildren<Text>(true) ) {
			Color c = text.color;
			c.a = 0f;
			HOTween.To ( text, dur , "color" , c  );
		}

	}

	public static void ClearFadeUi (Transform t ) {

		foreach ( Image image in t.GetComponentsInChildren<Image>(true) ) {
			HOTween.Kill (image);
			Color c = image.color;
			c.a = 1f;
			image.color = c;
		}

		foreach ( Text text in t.GetComponentsInChildren<Text>(true) ) {
			HOTween.Kill (text);
			Color c = text.color;
			c.a = 1f;
			text.color = c;
		}
	}
	#endregion

	#region fade 3d
	public static void Fade (Transform t, float dur ) {

		HOTween.To (t , dur , "localScale" , Vector3.one * 1.2f);

		foreach ( Renderer rend in t.GetComponentsInChildren<Renderer>(true) ) {
			Color c = rend.material.color;
			c.a = 0f;
			HOTween.To ( rend.material, dur , "color" , Color.clear  );
		}

	}

	public static void ClearFade (Transform t ) {

		foreach ( Renderer rend in t.GetComponentsInChildren<Renderer>(true) ) {
			Color c = rend.material.color;
			c.a = 0f;
			rend.material.color = c;
		}
	}
	#endregion
}
