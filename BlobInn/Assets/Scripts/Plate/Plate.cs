using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Plate : MonoBehaviour {

    public List<Ingredient.Type> ingredientTypes = new List<Ingredient.Type>();

    public float scaleAmount = 1.2f;
    public float scaleDuration = 0.2f;
    public float decalY = 100f;
    public float fallDuration = 0.3f;

    public int ingredientCount = 0;

    public float rangeX = 1f;
    public float rangeY = 1f;

    public Transform[] anchors;

    public GameObject prefab;

    public virtual void Start()
    {

    }
    public virtual void AddIngredient(Ingredient.Type type)
    {
        if ( ingredientCount == anchors.Length )
        {
            return;
        }

        ingredientTypes.Add(type);

        /// instance ///
        GameObject ingredientObj = Instantiate(prefab, anchors[ingredientCount]) as GameObject;

        ingredientObj.transform.localPosition = new Vector2(0, decalY);


        HOTween.To(ingredientObj.transform, fallDuration, "localPosition", Vector3.zero);

        ingredientObj.GetComponent<Ingredient>().Init(type);
        //ingredientObj.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-10, 10));

        Tween.Bounce(ingredientObj.transform);

        ++ingredientCount;
    }

    public virtual void Clear()
    {
        Tween.Bounce(transform);

        ingredientTypes.Clear();


        ingredientCount = 0;
    }

}
