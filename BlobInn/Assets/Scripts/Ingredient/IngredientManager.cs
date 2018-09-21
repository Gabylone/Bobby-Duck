using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour {

    public static IngredientManager Instance;

    public GameObject[] prefabs;

    public GameObject uiPrefab;
    public GameObject worldPrefab;

    public Sprite[] ingredientSprites;

    public List<List<Ingredient.Type>> desiredIngredients = new List<List<Ingredient.Type>>();

    public List<Ingredient.Type> GetDesiredIngredients ( Client.Type type)
    {
        if ( type == Client.Type.Prince)
        {
            return Inventory.Instance.ingredientTypes;
        }

        return desiredIngredients[(int)type];
    }

    public TextAsset desiredIngredientsTextAsset;

    void LoadDesiredIngredients()
    {
        string[] rows = desiredIngredientsTextAsset.text.Split('\n');

        Ingredient.Type ingredientType = Ingredient.Type.Tomato;

        for (int rowIndex = 2; rowIndex < rows.Length; rowIndex++)
        {
            string[] cells = rows[rowIndex].Split(';');

            Client.Type clientType = Client.Type.Regular;

            for (int cellIndex = 1; cellIndex < cells.Length; cellIndex++)
            {
                if ( (int)ingredientType == 0)
                {
                    desiredIngredients.Add(new List<Ingredient.Type>());
                }

                if (cells[cellIndex].Contains("1"))
                {
                    desiredIngredients[(int)clientType].Add(ingredientType);
                }

                ++clientType;

                if ( clientType == Client.Type.None)
                {
                    break;
                }
            }

            ++ingredientType;

            if ( ingredientType == Ingredient.Type.None)
            {
                break;
            }
        }
    }

    private void Awake()
    {
        Instance = this;

        ingredientSprites = Resources.LoadAll<Sprite>("Graphs/Ingredients/ingredients_small");

        LoadDesiredIngredients();
    }
}
