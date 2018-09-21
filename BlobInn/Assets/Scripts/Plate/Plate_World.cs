using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate_World : Plate {

    public int id = 0;

    public static Plate_World Current;
    public static Plate_World Previous;

    public float decal = 0.03f;

    bool visible = false;

    public GameObject group;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        Hide();
    }

    public delegate void OnAddIngredient(Ingredient.Type type);
    public OnAddIngredient onAddIngredient;

    public override void AddIngredient(Ingredient.Type type)
    {
        base.AddIngredient(type);

        Show();

        if (onAddIngredient != null)
        {
            onAddIngredient(type);
        }

        int spriteRendererIndex = 0;

        foreach (Ingredient_World item in GetComponentsInChildren<Ingredient_World>())
        {
            item.rend.sortingOrder = 35 - spriteRendererIndex;

            ++spriteRendererIndex;
        }

    }

    public delegate void OnClearPlate();
    public OnClearPlate onClearPlate;
    public override void Clear()
    {
        base.Clear();

        foreach (var item in GetComponentsInChildren<Ingredient_World>())
        {
            Destroy(item.gameObject);
        }

        Hide();

        if (onClearPlate != null)
        {
            onClearPlate();
        }
    }

    public void Show()
    {
        if (visible)
            return;

        if (ingredientTypes.Count == 0)
            return;

        group.SetActive(true);

        Tween.Bounce(transform);

        /*if ( Previous != null && Previous != Current)
        {
            Previous.Hide();
        }*/

        Previous = Current;
        Current = this;

        visible = true;
    }

    public void Hide()
    {
        group.SetActive(false);

        visible = false;
    }
}
