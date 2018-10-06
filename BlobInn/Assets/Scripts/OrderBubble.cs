using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Holoville.HOTween;

public class OrderBubble : MonoBehaviour {

    public Vector2 clientDecal = new Vector2();

    public float fadeDecal = 1f;
    public float fadeDuration = 0.5f;

    public float timeBeforeFade = 2f;

    public Image bubbleImage;

    public Client client;

    public Transform layoutGroup;

    bool showingIngredients = false;
    public float ingredientAppearRate = 0.2f;

    AudioSource audioSource;

    Image image;

    public Image tableNumberImage;

    bool onClient = false;

    public GameObject group;
    public GameObject tableNumberGroup;

    public RectTransform[] layoutGroupRectTransforms;

    void Show()
    {
        group.SetActive(true);
        tableNumberGroup.SetActive(true);

        if (image == null)
        {
            image = GetComponent<Image>();
        }
        image.enabled = true;
    }

    void Hide()
    {

        group.SetActive(false);
        tableNumberGroup.SetActive(false);

        if (image == null)
        {
            image = GetComponent<Image>();
        }
        image.enabled = false;

    }


    // Use this for initialization
    void Start()
    {
        image = GetComponent<Image>();

        audioSource = GetComponent<AudioSource>();

        Hide();
    }

    void UpdateLayoutGroups()
    {
        foreach (var layoutGroupRectTransform in layoutGroupRectTransforms)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroupRectTransform);
        }
    }

    private void Update()
    {
        if (onClient)
        {
            UpdatePositionOnClient();
        }

        bubbleImage.color = client.bodyRenderer.color; 
    }

    public void Init(Client _client)
    {
        client = _client;

        client.onChangeState += HandleOnChangeState;
    }
    public void LookingForTable()
    {
        Hide();
    }
    public void GoingToTable()
    {
        int id = client.targetTable.id;
        tableNumberImage.sprite = OrderBubbleManager.Instance.numberSprites[id];
    }
    public void WaitingForOrder()
    {
       
    }
    public void WaitingForDish()
    {
        ShowInUI();
        UpdateVisual();

    }
    public void Eating()
    {
        Hide();
    }
    public void WaitingForBill()
    {
    }
    public void Leaving()
    {
    }

    private void HandleOnChangeState(Client.State newState)
    {
        if (client.targetTable != null && client.targetTable.waited)
        {
            return;
        }

        switch (newState)
        {
            case Client.State.LookForTable:
                LookingForTable();
                break;
            case Client.State.GoToTable:
                GoingToTable();
                break;
            case Client.State.WaitForOrder:
                WaitingForOrder();
                break;
            case Client.State.WaitForDish:

                WaitingForDish();

                break;
            case Client.State.Eating:
                Eating();
                break;
            case Client.State.WaitForBill:
                WaitingForBill();
                break;
            case Client.State.Leaving:
                Leaving();
                break;
            case Client.State.None:
                break;
            default:
                break;
        }

    }

    #region movement
    void Fade()
    {
        ShowInUI();
    }

    void ShowInUI()
    {
        CancelInvoke();

        Show();

        foreach (var item in GetComponentsInChildren<Image>(true))
        {
            HOTween.To(item, fadeDuration, "color", Color.white);
        }

        bubbleImage.transform.localScale = new Vector3(1f, 1f, 1f);


        onClient = false;

        transform.SetParent(OrderBubbleManager.Instance.layoutAnchor);

        OrderBubbleManager.Instance.UpdateLayoutGroup();

        Tween.Bounce(transform);

    }

    void ShowOnClient()
    {
        CancelInvoke();
        HOTween.Kill(transform);

        foreach (var item in GetComponentsInChildren<Image>())
        {
            HOTween.Kill(item);
            item.color = Color.white;
        }

        Show();

        transform.SetParent(OrderBubbleManager.Instance.appearAnchor);

        UpdatePositionOnClient();

        onClient = true;

        Tween.Bounce(transform);


    }

    void UpdatePositionOnClient()
    {
        Vector3 p = client.transform.position;

        if (p.x < Camera.main.transform.position.x)
        {
            transform.position = p + Vector3.right * clientDecal.x + Vector3.up * clientDecal.y;
            bubbleImage.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.position = p + Vector3.left * clientDecal.x + Vector3.up * clientDecal.y;
            bubbleImage.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
    #endregion

    #region ingredients
    void UpdateVisual()
    {
        UpdateVisualCoroutine();
    }

    void UpdateVisualCoroutine()
    {
        foreach (var item in GetComponentsInChildren<Ingredient_UI>())
        {
            Destroy(item.gameObject);
        }

        int index = 0;

        foreach (var ingredientType in client.order)
        {
            GameObject ingredientObj = Instantiate(IngredientManager.Instance.uiPrefab, layoutGroup) as GameObject;

            ingredientObj.GetComponent<Ingredient>().Init(ingredientType);

            Tween.Bounce(ingredientObj.transform);

            SoundManager.Instance.Play(audioSource, SoundManager.SoundType.Ingredient1);
            UpdateLayoutGroups();

            ++index;
        }

    }
    #endregion

}
