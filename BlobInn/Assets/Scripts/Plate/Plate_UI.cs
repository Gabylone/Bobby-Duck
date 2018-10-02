using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Plate_UI : Plate {

    public static Plate_UI Current;
    public static Plate_UI Previous;

    public KeyCode keyCode;

    public int id = 0;

    public GameObject group;

    public void Show()
    {
        group.SetActive(true);
    }

    public void Hide()
    {
        group.SetActive(false);
    }

    public override void Start()
    {
        base.Start();

        Player.Instance.plates[id].onAddIngredient += AddIngredient;
        Player.Instance.plates[id].onClearPlate += Clear;
    }

    #region selection
    public void OnPointerClick()
    {
        if (Current == this)
        {
            return;
        }

        SoundManager.Instance.Play(SoundManager.SoundType.Plate);


        Select();
    }

    public override void AddIngredient(Ingredient.Type type)
    {
        base.AddIngredient(type);

    }

    public override void Clear()
    {
        base.Clear();

        foreach (var item in GetComponentsInChildren<Ingredient_UI>())
        {
            Destroy(item.gameObject);
        }

    }

    public void Select()
    {
        if (Current != null)
        {
            Current.Deselect();
        }



        Tween.Scale(transform, scaleDuration, scaleAmount);

        Current = this;

        Player.Instance.CurrentPlate = Player.Instance.plates[id];
        Player.Instance.CurrentPlate.Show();


    }

    public void Deselect()
    {
        Tween.Scale(transform, scaleDuration, 1);
        Player.Instance.CurrentPlate.Hide();
            
    }
    #endregion
}
