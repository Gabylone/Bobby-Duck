using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour {

    public Table[] tables;

    public GameObject[] tileGroups;

	// Use this for initialization
	void Start () {

        Table.tables.Clear();
        Interactable.interactables.Clear();

        tables = GetComponentsInChildren<Table>();

        int tableIndex = 0;

        foreach (var item in tables)
        {
            item.Hide();

            item.id = tableIndex;
            ++tableIndex;
        }

        for (int i = 0; i < Inventory.Instance.tableAmount; i++)
        {
            if (i == tables.Length)
                break;

            tables[i].Show();
            tables[i].Init();

        }

        foreach (var tileGroup in tileGroups)
        {
            tileGroup.SetActive(false);
        }

        tileGroups[0].SetActive(true);

        if (Inventory.Instance.tableAmount > 2)
        {
            tileGroups[1].SetActive(true);
        }
        if (Inventory.Instance.tableAmount > 4)
        {
            tileGroups[2].SetActive(true);
        }
        if (Inventory.Instance.tableAmount > 6)
        {
            tileGroups[3].SetActive(true);
        }
        if (Inventory.Instance.tableAmount > 8)
        {
            tileGroups[4].SetActive(true);
        }
        if (Inventory.Instance.waiterAmount > 0)
        {
            tileGroups[5].SetActive(true);
        }
    }
}
