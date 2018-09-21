using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ingredient_UI : Ingredient {

    Image image;

    public override void Init(Type type)
    {
        base.Init(type);

        image = GetComponentInChildren<Image>();
        image.sprite = IngredientManager.Instance.ingredientSprites[(int)type];

        image.SetNativeSize();
    }
}
