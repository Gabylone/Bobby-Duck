using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour {

    public Image mapImage;

    public GameObject padlockGroup;

    public int progression = 0;
    int max = 0;

    public Image progression_FillImage;
    public Image progression_BackgroundImage;

    public Text pearls_UiText;

    public string mapName = "Map Name";

    public Text mapName_UIText;

    public GameObject iconVisualGroup;
    public IconVisual targetIconVisual;
    public Text captainUIText;
    public Text levelUIText;
    public Image jobImage;

    public GameObject newGameGroup;

    bool load = false;

    public ApparenceItem apparenceItem;

    // Use this for initialization
    void Start()
    {
        UpdateUI();

        max = System.Enum.GetValues(typeof(TutorialStep)).Length;

    }

    public void UpdateUI()
    {
        apparenceItem = CrewCreator.Instance.GetApparenceItem(apparenceItem.apparenceType, apparenceItem.id);

        mapName_UIText.text = mapName;

        if (apparenceItem.locked)
        {

            //mapImage.raycastTarget = false;

            padlockGroup.SetActive(true);
            mapImage.color = Color.black;
            newGameGroup.SetActive(false);

            pearls_UiText.text = "" + apparenceItem.price;
        }
        else
        {

            //mapImage.raycastTarget = true;

            padlockGroup.SetActive(false);
            mapImage.color = Color.white;

        }

        if (SaveTool.Instance.FileExists(mapName, "game data"))
        {
            load = true;

            iconVisualGroup.SetActive(true);

            GameData gameData = SaveTool.Instance.LoadFromSpecificPath(mapName , "game data.xml", "GameData") as GameData;

            Member captain = gameData.playerCrew.MemberIDs[0];

            targetIconVisual.InitVisual(captain);

            captainUIText.text = captain.Name;
            levelUIText.text = captain.Lvl.ToString();

            Sprite[] jobSprites = Resources.LoadAll<Sprite>("Graph/JobSprites");
            jobImage.sprite = jobSprites[(int)captain.job];

            newGameGroup.SetActive(false);

        }
        else
        {
            load = false;

            iconVisualGroup.SetActive(false);

            newGameGroup.SetActive(!apparenceItem.locked);
        }
    }

    void UpdateProgressionBar()
    {

        float w = progression_BackgroundImage.rectTransform.rect.width;
        float l1 = (float)(progression - 1) / (float)max;
        float l2 = (float)progression / (float)max;

        progression_FillImage.rectTransform.sizeDelta = new Vector2(-(w) + (l1 * w), 0);

    }

    public void LaunchMap()
    {
        if (apparenceItem.locked)
        {
            DisplayPurchase.Instance.Display(apparenceItem, transform);
            return;
        }

        Tween.Bounce(transform);

        Transitions.Instance.ScreenTransition.FadeIn(1f);

        if (load)
        {
            KeepOnLoad.dataToLoad = 0;
        }
        else
        {
            KeepOnLoad.dataToLoad = -1;
        }

        KeepOnLoad.Instance.price = 100;
        KeepOnLoad.Instance.mapName = mapName;


        SaveTool.Instance.CreateDirectories();

        Invoke("LaunchMapDelay", 1f);

    }

    void LaunchMapDelay()
    {
        SceneManager.LoadScene(1);

    }

    public void EraseMap()
    {
        Debug.Log("erasing map");

        MessageDisplay.Instance.Show("Ecraser sauvegarde ?");

        MessageDisplay.onValidate += ConfirmEraseMap;
    }


    void ConfirmEraseMap()
    {
        SaveTool.Instance.DeleteFolder(mapName);

        Tween.Bounce(transform);

        UpdateUI();
    }



}
