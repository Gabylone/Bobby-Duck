using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour {

    public enum Type
    {
        Tomato,
        Chicken,
        Cheese,
        Beer,
        Cake,
        Fish,
        Soup,
        Grapes,
        Milk,
        Salad,
        Eggs,
        
        None,
    }

    public Type type;


    public virtual void Init (Type type)
    {
        
    }
}
