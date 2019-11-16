using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementImpactType {NotEffective, VeryWeak, Weak, Normal, Crit, MegaCrit }
public enum ElementType { Normal, Fire, Water, Nature, Electric, Spectre, Fighting, Ice, Wind, Rock, Metal, None }
public class ElementMatrix : MonoBehaviour
{
    public List<ElementImpactTypeContainer> elements = new List<ElementImpactTypeContainer>();


    public ElementImpactType ReturnImpactType(PlayerCreatureStats p2, Ability abilityUsed) {

        foreach (ElementImpactTypeContainer e in elements)
        {
            if (e.elementType == p2.creatureSO.primaryElement)
            {
                foreach(ElementImpactTypeListItem listItem in e.elementList)
                {
                    if (listItem.elementType == abilityUsed.elementType)
                    {
                        return listItem.elementImpactType;
                    }
                }
            }
        }
        Debug.Log("normal");
        return ElementImpactType.Normal;
    }
}
[Serializable]
public struct ElementImpactTypeContainer {

    public ElementType elementType;
    public List<ElementImpactTypeListItem> elementList;
}
[Serializable]
public class ElementImpactTypeListItem {
    public ElementType elementType;
    public ElementImpactType elementImpactType;
}
