using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTable : MonoBehaviour
{
  [SerializeField]  List<Ability> abilities = new List<Ability>();

    public List<Ability> Abilities { get => abilities; set => abilities = value; }
}
