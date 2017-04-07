using UnityEngine;
using System.Collections;

public class Percentage : MonoBehaviour {

	public static int getRandomIndex ( int[] percents ) {

		float chance = Random.value * 100f;

		float prevPercent = 0f;

		for (int i = 0; i < percents.Length; ++i) {

			float percent = prevPercent + percents [i];

			if (chance > prevPercent && chance < prevPercent + percents [i]) {
				return i;
			}

			prevPercent += percents [i];
		}

		Debug.LogError ("index out of percentage");
		return 0;
	}
}
