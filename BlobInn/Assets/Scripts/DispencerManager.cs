using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispencerManager : MonoBehaviour {

    public static DispencerManager Instance;

    public Sprite[] sprites;

    public Dispencer[] dispencers;

    public GameObject dispencerPrefab;

    public GameObject[] tileGroups;

    public Transform[] dispencerAnchors;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start() {

        Dispencer.dispencers.Clear();

        dispencers = GetComponentsInChildren<Dispencer>();

        foreach (var item in dispencers)
        {
            item.Hide();
        }

        dispencers = new Dispencer[Inventory.Instance.ingredientTypes.Count];

        int i = 0;
        foreach (var ingredientType in Inventory.Instance.ingredientTypes)
        {
            GameObject dispencer = Instantiate(dispencerPrefab, transform) as GameObject;

            dispencer.transform.position = dispencerAnchors[i].position;

            dispencers[i] = dispencer.GetComponent<Dispencer>();

            dispencers[i].Show();
            dispencers[i].Init(ingredientType);

            ++i;
        }

        foreach (var tileGroup in tileGroups)
        {
            tileGroup.SetActive(false);
        }

        tileGroups[0].SetActive(true);

        if (Inventory.Instance.ingredientTypes.Count > 3)
        {
            tileGroups[1].SetActive(true);
        }
        if (Inventory.Instance.ingredientTypes.Count > 6)
        {
            tileGroups[2].SetActive(true);
        }
        if (Inventory.Instance.ingredientTypes.Count > 9)
        {
            tileGroups[3].SetActive(true);
        }
	}
}
