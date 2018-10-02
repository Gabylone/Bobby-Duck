using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCatcher : MonoBehaviour {

    public delegate void OnTouchRayCatcher();
    public static OnTouchRayCatcher onTouchRayCatcher;

    public delegate void OnExitRayCatcher();
    public static OnExitRayCatcher onExitRayCatcher;

	public void OnPointerDown()
    {
        if (onTouchRayCatcher != null)
            onTouchRayCatcher();
    }

    public void OnPointerExit()
    {
        if (onExitRayCatcher != null)
            onExitRayCatcher();
    }
}
