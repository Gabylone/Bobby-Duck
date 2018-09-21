using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayNewClient : DisplayGroup {

    public static DisplayNewClient Instance;

    public Text uiText_Description;
    public Text uiText_Title;

    public Image newClientImage_Body;
    public Image newClientImage_Clothes;

    public Image[] ingredientImages;

    public RectTransform layoutGroup;

    private void Awake()
    {
        Instance = this;
    }

    public void Display(Client.Type type)
    {
        Open();

        Tutorial.Instance.Show(TutorialStep.NewClients);

        uiText_Title.text = Level.Current.newClientNames[(int)Inventory.currentLanguageType];

        uiText_Description.text = Level.Current.newClientDescriptions[(int)Inventory.currentLanguageType];

        List<Ingredient.Type> types = IngredientManager.Instance.GetDesiredIngredients(type);

        int i = 0;
        foreach (var image in ingredientImages)
        {
            if ( i < types.Count)
            {
                image.gameObject.SetActive(true);

                image.rectTransform.sizeDelta = new Vector2(IngredientManager.Instance.ingredientSprites[(int)types[i]].rect.width , IngredientManager.Instance.ingredientSprites[(int)types[i]].rect.height);

                image.sprite = IngredientManager.Instance.ingredientSprites[(int)types[i]];
            }
            else
            {
                image.gameObject.SetActive(false);
            }

            ++i;
        }

        Invoke("UpdateBonjour", tweenDuration);
    }

    void UpdateBonjour()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup);
    }


}
