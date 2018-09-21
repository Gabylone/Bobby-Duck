using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Tilable {

    public static Dictionary<Coords, Interactable> interactables = new Dictionary<Coords, Interactable>();

    public virtual void Contact(Waiter waiter)
    {
        
    }
}
