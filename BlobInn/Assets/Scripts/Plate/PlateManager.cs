using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateManager : MonoBehaviour {

    public Plate_UI[] plates;

    // Use this for initialization
    void Start()
    {
        plates = GetComponentsInChildren<Plate_UI>(true);

        int id = 0;

        foreach (var plate in plates)
        {
            plate.Hide();

            plate.id = id;

            ++id;
        }

        for (int i = 0; i < Inventory.Instance.plateAmount; i++)
        {
            if (i == plates.Length)
                break;

            plates[i].Show();
        }

        plates[0].Select();


    }
}
