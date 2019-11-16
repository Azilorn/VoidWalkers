using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frozen : Ailment
{
    public override void ChangeAilmentCount()
    {
        base.ChangeAilmentCount();
    }

    public override bool CheckAilmentCanBeApplied()
    {
        return base.CheckAilmentCanBeApplied();
    }
    public override void RemoveAilment()
    {
        base.RemoveAilment();
    }

    public override void RunAilment(bool playersTurn)
    {
        base.RunAilment(playersTurn);
    }
}