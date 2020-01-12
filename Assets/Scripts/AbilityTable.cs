using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTable : MonoBehaviour
{
  [SerializeField]  List<Ability> abilities = new List<Ability>();

    public List<Ability> Abilities { get => abilities; set => abilities = value; }


    public int ReturnAbilityID(Ability a)
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            if (a == abilities[i])
                return i;
        }
        return 0;
    }
}
