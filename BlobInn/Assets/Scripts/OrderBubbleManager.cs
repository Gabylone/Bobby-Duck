using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderBubbleManager : MonoBehaviour {

    public static OrderBubbleManager Instance;

    public Sprite[] feedbackSprites;

    public Sprite[] numberSprites;

    public Transform layoutAnchor;
    public Transform appearAnchor;

    public GameObject prefab;

    public RectTransform layoutGroup;

    // Use this for initialization
    void Start () {
        Instance = this;
	}

    public OrderBubble NewOrderBubble()
    {
        GameObject newOrderBubble = Instantiate(prefab, appearAnchor) as GameObject;

        return newOrderBubble.GetComponent<OrderBubble>();
    }

    public void UpdateLayoutGroup()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup);
    }

}
