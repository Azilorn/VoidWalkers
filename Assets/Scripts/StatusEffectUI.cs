using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectUI : MonoBehaviour
{
    public List<GameObject> statusEffects = new List<GameObject>();

    public GameObject ReturnElement(NegativeAilment negativeAilment) {


        switch (negativeAilment)
        {
            case NegativeAilment.Burnt:
                return statusEffects[0];
            case NegativeAilment.Shocked:
                return statusEffects[1];
            case NegativeAilment.Poisoned:
                return statusEffects[2];
            case NegativeAilment.Frozen:
                return statusEffects[3];
            case NegativeAilment.Confused:
                return statusEffects[4];
            case NegativeAilment.Etheral:
                return statusEffects[5];
            case NegativeAilment.Sleep:
                return statusEffects[6];
            case NegativeAilment.Bleeding:
                return statusEffects[7];
        }

        return null;
    }
    public GameObject ReturnElement(Ailment ailment)
    {
        if (ailment is Burnt)
            return statusEffects[0];
        else if (ailment is Shocked)
            return statusEffects[1];
        else if (ailment is Poison)
            return statusEffects[2];
        else if (ailment is Frozen)
            return statusEffects[3];
        else if (ailment is Confused)
            return statusEffects[4];
        else if (ailment is Ethereal)
            return statusEffects[5];
        else if (ailment is Sleep)
            return statusEffects[6];
        else if (ailment is Bleeding)
            return statusEffects[7];

        return null;
    }
}
