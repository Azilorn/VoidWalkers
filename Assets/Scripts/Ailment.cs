using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ailment
{

    public virtual void RunAilment(bool playersTurn) { }

    public virtual void RemoveAilment() { }

    public virtual void ChangeAilmentCount() { }

    public virtual bool CheckAilmentCanBeApplied() {
        return true;
    }
}
