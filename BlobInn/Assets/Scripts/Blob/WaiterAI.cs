using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterAI : Waiter {

    public static List<WaiterAI> waiterAIs = new List<WaiterAI>();

    public static int id = 0;

    public Coords initCoords;

    // Use this for initialization

    bool servingFood = false;

    Table currentTable;

    public List<Ingredient.Type> targetOrder = new List<Ingredient.Type>();

    public override void Start()
    {
        id = waiterAIs.Count;

        if (id >= Inventory.Instance.waiterAmount)
        {
            gameObject.SetActive(false);
            return;
        }

        base.Start();

        GetComponentInChildren<Blob_Apparence>().SetSprite(Blob_Apparence.Type.Eyes, 0);

        waiterAIs.Add(this);

        CurrentPlate = plates[0];

        initCoords = coords;

        Table.onTouchTable += HandleOnTouchTable;
    }

    public void HandleOnTouchTable(Table table)
    {
        currentTable = table;

        currentTable.EnterWait();

        currentTable.client.TakeOrder();

        if ( table.client != null && table.client.currentState == Client.State.WaitForDish)
        {
            GetClientIngredients();
            return;
        }

        GoToCoords(currentTable.coords + new Coords(1, 0), currentTable.coords);
    }

    void GetClientIngredients()
    {
        servingFood = true;

        targetOrder.Clear();
        foreach (var item in currentTable.client.order)
        {
            targetOrder.Add(item);
        }

        ingredientIndex = 0;
        Debug.Log("d'abord je m'occupe de " + targetOrder[ingredientIndex]);
        GoToNextIngredient();

    }

    int ingredientIndex = 0;

    void GoToNextIngredient()
    {
        // d'abbord, desencds
        Dispencer targetDispencer = Dispencer.GetDispencer(targetOrder[ingredientIndex]);

        Coords targetCoords = targetDispencer.coords;

        List<Coords> tmpCoords = new List<Coords>();

        if ( ingredientIndex > 0)
        {
            // il descend
            tmpCoords.Add(new Coords(coords.x, 2));
        }

        // juste à côté du distrib
        if (targetCoords.x > 0)
        {
            tmpCoords.Add(targetCoords + new Coords(-1, 0));
        }
        else
        {
            tmpCoords.Add(targetCoords + new Coords(1, 0));
        }

        // sur le distrib
        tmpCoords.Add(targetCoords);

        GoToCoords(tmpCoords);

    }

    public override void ReachTargetCoords()
    {
        base.ReachTargetCoords();

        if (servingFood)
        {
            ++ingredientIndex;

            if (ingredientIndex == targetOrder.Count)
            {
                Debug.Log("j'ai finis tous les ingrédients");
                List<Coords> tmpCoords = new List<Coords>();
                tmpCoords.Add(new Coords(coords.x, 2));
                tmpCoords.Add(currentTable.coords + new Coords(1, 0));
                tmpCoords.Add(currentTable.coords);
                GoToCoords(tmpCoords);

                servingFood = false;
                return;
            }
            else
            {
                GoToNextIngredient();
            }

        }
        else
        {
            if ( coords != initCoords)
            {
                GoToCoords(initCoords);
            }

            currentTable.ExitWait();

        }
    }
}
