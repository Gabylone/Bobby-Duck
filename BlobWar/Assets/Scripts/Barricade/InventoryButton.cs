using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour {

    public GameObject group;

    public virtual void Start()
    {

    }

    public virtual void Show()
    {
        group.SetActive(true);
    }

    public virtual void Hide()
    {
        group.SetActive(false);
    }

    public virtual void Activate()
    {
        
    }
}
