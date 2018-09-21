using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispencer : Interactable {

    public Ingredient.Type ingredientType;

    public Transform[] ingredientAnchors;

    public GameObject group;

    public SpriteRenderer rend;

    Animator animator;

    AudioSource audioSource;

    public static List<Dispencer> dispencers = new List<Dispencer>();
    public static Dispencer GetDispencer (Ingredient.Type type)
    {
        return dispencers.Find(x => x.ingredientType == type);
    }

    public void Show()
    {
        group.SetActive(true);
    }

    public void Hide()
    {
        group.SetActive(false);
    }

    public void Init ( Ingredient.Type type )
    {
        base.Start();

        animator = GetComponentInChildren<Animator>();

        animator.SetInteger("ingredientType", (int) type);

        audioSource = GetComponent<AudioSource>();

        ingredientType = type;

        dispencers.Add(this);
        Interactable.interactables.Add(coords, this);


        Player.Instance.onMove += HandleOnPlayerMove;

        rend.sprite = DispencerManager.Instance.sprites[(int)ingredientType];


    }

    private void HandleOnPlayerMove()
    {
        if ( Player.Instance.coords.y < coords.y)
        {
            rend.sortingOrder = 12;
        }
        else
        {
            rend.sortingOrder = 20;
        }
    }

    public override void Contact(Waiter waiter)
    {
        base.Contact(waiter);

        waiter.CurrentPlate.AddIngredient(ingredientType);

        Tween.Bounce(transform);

        Invoke("ContactDelay", 1f);

        if (Random.value < 0.5f)
        {
            SoundManager.Instance.Play(audioSource, SoundManager.SoundType.Ingredient1);
        }
        else
        {
            SoundManager.Instance.Play(audioSource, SoundManager.SoundType.Ingredient2);
        }

    }

    void ContactDelay()
    {
        Tutorial.Instance.Show(TutorialStep.EmptyPlate);
    }

}
