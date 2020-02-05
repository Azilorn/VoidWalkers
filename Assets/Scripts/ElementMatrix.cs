using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementImpactType {NotEffective, VeryWeak, Weak, Normal, Crit, MegaCrit }
public enum ElementType { Normal, Fire, Water, Nature, Electric, Spectre, Fighting, Ice, Wind, Earth, Metal, Insect, Unholy, Holy, Ancient, None }
public class ElementMatrix : MonoBehaviour
{
    public static ElementMatrix Instance;
    public List<ElementImpactTypeContainer> elements = new List<ElementImpactTypeContainer>();
    public List<Color> elementColors = new List<Color>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }  
    }

    public ElementImpactType ReturnImpactType(PlayerCreatureStats p2, Ability abilityUsed) {

        foreach (ElementImpactTypeContainer e in elements)
        {
            if (e.elementType == p2.creatureSO.primaryElement)
            {
                foreach (ElementImpactTypeListItem listItem in e.elementList)
                {
                    if (listItem.elementType == abilityUsed.elementType)
                    {
                        return listItem.elementImpactType;
                    }
                }
            }
            else if (e.elementType == p2.creatureSO.secondaryElement) {
                foreach (ElementImpactTypeListItem listItem in e.elementList)
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
    public Color ReturnElementColor(ElementType e)
    {
        switch (e)
        {
            case ElementType.Normal:
                return elementColors[0];
            case ElementType.Fire:
                return elementColors[1];
            case ElementType.Water:
                return elementColors[2];
            case ElementType.Nature:
                return elementColors[3];
            case ElementType.Electric:
                return elementColors[4];
            case ElementType.Spectre:
                return elementColors[5];
            case ElementType.Fighting:
                return elementColors[6];
            case ElementType.Ice:
                return elementColors[7];
            case ElementType.Wind:
                return elementColors[8];
            case ElementType.Earth:
                return elementColors[9];
            case ElementType.Metal:
                return elementColors[10];
            case ElementType.Insect:
                return elementColors[11];
            case ElementType.Unholy:
                return elementColors[12];
            case ElementType.Holy:
                return elementColors[13];
            case ElementType.Ancient:
                return elementColors[14];
            case ElementType.None:
                return Color.grey;
        }
        return Color.grey;
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
