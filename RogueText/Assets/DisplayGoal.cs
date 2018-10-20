using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayGoal : TextTyper {

    bool displayedGoal = false;

    public string goal = "";

    public override void Start()
    {
        base.Start();
    }

    public override void UpdateCurrentTileDescription()
    {
        base.UpdateCurrentTileDescription();

        if ( displayedGoal )
        {
            Hide();
            return;
        }

        Direction dir = Coords.GetDirectionFromCoords(Player.Instance.coords , ClueManager.Instance.clueCoords);
        string direction_str = Coords.GetWordsDirection(dir).GetDescription(Word.Def.Defined, Word.Preposition.A, Word.Number.Singular);

        goal = goal.Replace("CLUEPOSITION",direction_str);

        //Display(goal);
        textToType = goal;

        displayedGoal = true;
    }
}
