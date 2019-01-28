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
    public Image[] crossImages;

    public RectTransform layoutGroup;

    private void Awake()
    {
        Instance = this;
    }

	public override void Update ()
	{
		base.Update ();
	}


    public void Display(Client.Type type)
    {
        Open();

        Tutorial.Instance.Show(TutorialStep.NewClients);

		uiText_Title.text = Client.infos[(int)type].names[(int)Inventory.currentLanguageType];
		/*if ( (int)type >= Client.infos.Length ) {
			Debug.LogError("type : " + type + " longueur : " + Client.infos.Length);

		}
		if ( (int)Inventory.currentLanguageType >= Client.infos[(int)type].names.Length ) {
			Debug.LogError("language : " + Inventory.currentLanguageType + " longueur : " + Client.infos[(int)type].names.Length);
		}*/
        //uiText_Description.text = Level.Current.newClientDescriptions[(int)Inventory.currentLanguageType];

        List<Ingredient.Type> types = IngredientManager.Instance.GetDesiredIngredients(type);

        int i = 0;
        foreach (var image in ingredientImages)
        {
            if ( i < types.Count)
            {
                image.gameObject.SetActive(true);

                image.rectTransform.sizeDelta = new Vector2(IngredientManager.Instance.ingredientSprites[(int)types[i]].rect.width , IngredientManager.Instance.ingredientSprites[(int)types[i]].rect.height);

                image.sprite = IngredientManager.Instance.ingredientSprites[(int)types[i]];

				if ( Inventory.Instance.ingredientTypes.Contains(types[i])  ) {
					crossImages [i].gameObject.SetActive (false);
					//
				} else {
					crossImages [i].gameObject.SetActive (true);
					//
				}
            }
            else
            {
                image.gameObject.SetActive(false);
				crossImages [i].gameObject.SetActive (false);
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
