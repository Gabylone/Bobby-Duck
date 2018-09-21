using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient_World : Ingredient {


    public SpriteRenderer rend;

    public override void Init(Type type)
    {
        base.Init(type);

        rend = GetComponentInChildren<SpriteRenderer>();
        rend.sprite = IngredientManager.Instance.ingredientSprites[(int)type];
    }
}
