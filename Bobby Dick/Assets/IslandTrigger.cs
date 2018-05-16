using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandTrigger : MonoBehaviour {

    public Island island;

    private void OnMouseDown()
    {
        island.OnPointerDown();
    }
}
