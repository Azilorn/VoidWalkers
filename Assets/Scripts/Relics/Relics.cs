using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RelicName { PrayerBeads, JugOfMilk, Passport, MalachiteQuill, ShatteredSkull };
[Serializable]
public class Relics : MonoBehaviour
{
    public virtual bool CalculateChance() {

        return false;
    }
    public virtual IEnumerator RunEffect() { yield return null; }

}
