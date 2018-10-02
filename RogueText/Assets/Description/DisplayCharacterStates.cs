using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCharacterStates : TextTyper {

	public override void Start ()
	{
		base.Start ();

		Player.onPlayerMove += HandleOnPlayerMove;

		ActionManager.onAction += HandleOnAction;

	}

	void HandleOnAction (Action action)
	{
		switch (action.type) {
		case Action.Type.Take:
		case Action.Type.Eat:
                UpdateCurrentTileDescription();
			break;
		default:
			break;
		}
    }

    void HandleOnPlayerMove(Coords prevCoords, Coords newCoords)
    {
        //UpdateCurrentTileDescription();
    }

    public override void UpdateCurrentTileDescription()
    {
        base.UpdateCurrentTileDescription();

		Clear ();

        List<string> phrases = new List<string>();

		if ( Player.Instance.health == Player.Instance.maxHealth ){

            string str = "" +
                "Assoifé, affamé ou bléssé, vous n'avez pas tenu le coup" +
                "\n" +
                "Vous êtes mort...";

            Display(str);
            //phrases.Add(str);
            DisplayInput.Instance.EndInput();
			return;
		}

		if ( Player.Instance.sleep == Player.Instance.maxSleep ) {

            string str = "" +
                "Vos paupières se ferment toutes seules, vous êtes épuisé";

            phrases.Add(str);

		} else if ( Player.Instance.sleep > (float)(Player.Instance.maxSleep * 0.65f) ) {

            string str = "" +
                "Vous vous sentez légerement fatigué...";

            phrases.Add(str);
		}

		if ( Player.Instance.hunger == Player.Instance.maxHunger ) {

            string str = "" +
                "Votre ventre est vide, vous avez faim...";

            phrases.Add(str);

		} else if ( Player.Instance.hunger > (float)(Player.Instance.maxHunger * 0.65f) ) {

            string str = "" +
                "Votre ventre commence à gargouiller...";

            phrases.Add(str);

		}

		if ( Player.Instance.thirst == Player.Instance.maxThirst ) {

            string str = "" +
                "Votre gorge est sêche, vous avez soif";

            phrases.Add(str);

		} else if ( Player.Instance.thirst > (float)(Player.Instance.maxThirst * 0.65f) ) {

            string str = "" +
                "Vos lévres se durcissent, vous ressentez une légère soif...";

            phrases.Add(str);

		}

		if (Player.Instance.health > Player.Instance.maxHealth * 0.8f) {

            string str = "" +
                "Vous n'avez plus de force, vos yeux se ferment et votre souffle se raccourci" +
                "\n" +
                "Votre état est critique...";

            phrases.Add(str);

		} else if (Player.Instance.health > Player.Instance.maxHealth * 0.5f) {

            string str = "" +
                "Vous vous sentez faible, et avancez avec de plus en plus de mal";

            phrases.Add(str);

		} else if (Player.Instance.health > Player.Instance.maxHealth * 0.25f) {

            string str = "" +
                "Vous vous sentez légèrement faiblissant, vous n'êtes pas au haut de votre forme...";

            phrases.Add(str);

		}

        string text = "";

        int a = 0;
        foreach (var phrase in phrases)
        {
            text += phrase;

            if ( a < phrases.Count -1)
            {
                text += "\n";
            }

            ++a;
        }

        Display(text);
	}
}
