using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class IllegalAbilities : EditorWindow
{
    string windowTitle = "Illegal Abilities List";
    public static EditorWindow window;
    public static CreatureSO creatureScriptableObject;
    public static CreatureSO copyCreature;
    public static AbilityTable abilityTable;
    public Vector3 scrollPos;

    [MenuItem("GameElements/Illegal Abilities List")]
    public static void ShowWindow()
    {
        window = GetWindow(typeof(IllegalAbilities));
        window.maximized = false;
    }
    private void OnInspectorUpdate()
    {
        if (creatureScriptableObject != null)
        {
            EditorUtility.SetDirty(creatureScriptableObject);
        }
        Repaint();
    }
    private void OnGUI()
    {
        GUILayout.Label(windowTitle, EditorStyles.boldLabel);
        GUILayout.Label("Creature Scriptable Object", EditorStyles.label);


        creatureScriptableObject = (CreatureSO)EditorGUILayout.ObjectField(creatureScriptableObject, typeof(CreatureSO), false);

        if (creatureScriptableObject == null)
            return;

        if (creatureScriptableObject != null) {
            GUILayout.Label("Copy Creature", EditorStyles.label);
            copyCreature = (CreatureSO)EditorGUILayout.ObjectField(copyCreature, typeof(CreatureSO), false);
        }
        if (copyCreature != null)
        {
            if (GUILayout.Button("Copy Creautre", GUILayout.Width(400), GUILayout.Height(50)))
            {
                List<Ability> temp = new List<Ability>();

                foreach (var item in copyCreature.illegalAbilities)
                {
                    temp.Add(item);
                }

                creatureScriptableObject.illegalAbilities.Clear();
                foreach (var item in temp)
                {
                    creatureScriptableObject.illegalAbilities.Add(item);
                }
            }
        }
        Rect lastrect = GUILayoutUtility.GetRect(35, 35, GUIStyle.none, GUILayout.Width(75), GUILayout.Height(35));

        if (creatureScriptableObject.creaturePlayerIcon != null)
        {
            EditorGUI.DrawTextureTransparent(lastrect, creatureScriptableObject.creaturePlayerIcon.texture, ScaleMode.ScaleToFit);
        }
        if (abilityTable == null)
        {
            GameObject go = Resources.Load("AbilityTable") as GameObject;
            abilityTable = go.GetComponent<AbilityTable>();
        }
        if (creatureScriptableObject != null)
        {

            scrollPos = GUILayout.BeginScrollView(scrollPos);
           

            GUILayout.BeginHorizontal();
            {

                GUILayout.BeginVertical();
                {
                    //Add To List
                    GUILayout.BeginVertical();
                    {
                        GUILayout.Label("Ability Table", EditorStyles.largeLabel, GUILayout.Width(300));
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label("#", GUILayout.Width(40));
                            GUILayout.Label("Name", GUILayout.Width(150));
                            GUILayout.Label("Type", GUILayout.Width(75));
                            GUILayout.Label("Element", GUILayout.Width(75));
                            GUILayout.Label("", GUILayout.Width(40));

                        } 
                        GUILayout.EndHorizontal();
                        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.Width(500));
                    }
                    GUILayout.EndVertical();
                    for (int i = 0; i < abilityTable.Abilities.Count; i++)
                    {
                        if (creatureScriptableObject.illegalAbilities.Contains(abilityTable.Abilities[i]))
                            continue;


                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(abilityTable.Abilities[i].name, EditorStyles.miniBoldLabel, GUILayout.Width(40));


                            GUILayout.Label(abilityTable.Abilities[i].abilityName, EditorStyles.boldLabel, GUILayout.Width(150));

                            GUIStyle typeLable = new GUIStyle();
                            switch (abilityTable.Abilities[i].type)
                            {
                                case AbilityType.None:
                                    typeLable.normal.textColor = Color.grey;
                                    break;
                                case AbilityType.Attack:
                                    typeLable.normal.textColor = Color.black;
                                    break;
                                case AbilityType.Weather:
                                    typeLable.normal.textColor = Color.cyan;
                                    break;
                                case AbilityType.Buff:
                                    typeLable.normal.textColor = Color.green;
                                    break;
                                case AbilityType.Debuff:
                                    typeLable.normal.textColor = Color.red;
                                    break;
                                case AbilityType.Other:
                                    typeLable.normal.textColor = Color.magenta;
                                    break;
                                case AbilityType.AttackSelf:
                                    typeLable.normal.textColor = Color.grey;
                                    break;
                                default:
                                    typeLable.normal.textColor = Color.grey;
                                    break;
                            }

                            GUILayout.Label(abilityTable.Abilities[i].type.ToString(), typeLable, GUILayout.Width(75));
                            Rect lastrect2 = GUILayoutUtility.GetRect(35, 35, GUIStyle.none, GUILayout.Width(75), GUILayout.Height(35));

                            EditorGUI.DrawTextureTransparent(lastrect2, ReturnElementSprite(abilityTable.Abilities[i].elementType).texture, ScaleMode.ScaleToFit);


                            if (GUILayout.Button("+", GUILayout.Width(25), GUILayout.Height(25)))
                            {
                                creatureScriptableObject.illegalAbilities.Add(abilityTable.Abilities[i]);
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndVertical();
                //Remove From List
                GUILayout.BeginVertical();
                {

                    GUILayout.BeginVertical();
                    {
                        GUILayout.Label("Illegal Abilities", EditorStyles.largeLabel, GUILayout.Width(300));
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label("#", GUILayout.Width(40));
                            GUILayout.Label("Name", GUILayout.Width(150));
                            GUILayout.Label("Type", GUILayout.Width(75));
                            GUILayout.Label("Element", GUILayout.Width(75));
                            GUILayout.Label("", GUILayout.Width(40));

                        }
                        GUILayout.EndHorizontal();
                        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.Width(500));
                    }
                    GUILayout.EndVertical();
                    for (int i = 0; i < creatureScriptableObject.illegalAbilities.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(creatureScriptableObject.illegalAbilities[i].name, EditorStyles.miniBoldLabel, GUILayout.Width(40));

                            GUILayout.Label(creatureScriptableObject.illegalAbilities[i].abilityName, EditorStyles.boldLabel, GUILayout.Width(150));

                            GUIStyle typeLable = new GUIStyle();
                            switch (abilityTable.Abilities[i].type)
                            {
                                case AbilityType.None:
                                    typeLable.normal.textColor = Color.grey;
                                    break;
                                case AbilityType.Attack:
                                    typeLable.normal.textColor = Color.black;
                                    break;
                                case AbilityType.Weather:
                                    typeLable.normal.textColor = Color.cyan;
                                    break;
                                case AbilityType.Buff:
                                    typeLable.normal.textColor = Color.green;
                                    break;
                                case AbilityType.Debuff:
                                    typeLable.normal.textColor = Color.red;
                                    break;
                                case AbilityType.Other:
                                    typeLable.normal.textColor = Color.magenta;
                                    break;
                                case AbilityType.AttackSelf:
                                    typeLable.normal.textColor = Color.grey;
                                    break;
                                default:
                                    typeLable.normal.textColor = Color.grey;
                                    break;
                            }

                            GUILayout.Label(creatureScriptableObject.illegalAbilities[i].type.ToString(), typeLable, GUILayout.Width(75));
                            Rect lastrect2 = GUILayoutUtility.GetRect(35, 35, GUIStyle.none, GUILayout.Width(75), GUILayout.Height(35));

                            EditorGUI.DrawTextureTransparent(lastrect2, ReturnElementSprite(creatureScriptableObject.illegalAbilities[i].elementType).texture, ScaleMode.ScaleToFit);


                            if (GUILayout.Button("-", GUILayout.Width(25), GUILayout.Height(25)))
                            {
                                creatureScriptableObject.illegalAbilities.RemoveAt(i);
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

        GUILayout.EndScrollView();
        }
    }
    public Color ReturnElementTypeColor(ElementType type)
    {
        switch (type)
        {
            case ElementType.Normal:
                return Color.black;
            case ElementType.Fire:
                return Color.red;
            case ElementType.Water:
                return Color.blue;
            case ElementType.Nature:
                return Color.green;
            case ElementType.Electric:
                return Color.yellow;
            case ElementType.Spectre:
                return Color.magenta;
            case ElementType.Fighting:
                return Color.grey;
            case ElementType.Ice:
                return Color.cyan;
            case ElementType.Wind:
                return Color.white;
            case ElementType.Earth:
                return Color.black;
            case ElementType.Metal:
                return Color.grey;
            case ElementType.None:
                return Color.grey;
            default:
                return Color.black;
        }
    }
    public Sprite ReturnElementSprite(ElementType elementType)
    {
        Sprite icon;
        switch (elementType)
        {
            case ElementType.Normal:
                icon = Resources.Load<Sprite>("ElementType/Normal");
                break;
            case ElementType.Fire:
                icon = Resources.Load<Sprite>("ElementType/Fire");
                break;
            case ElementType.Water:
                icon = Resources.Load<Sprite>("ElementType/Water");
                break;
            case ElementType.Nature:
                icon = Resources.Load<Sprite>("ElementType/Nature");
                break;
            case ElementType.Electric:
                icon = Resources.Load<Sprite>("ElementType/Electric");
                break;
            case ElementType.Spectre:
                icon = Resources.Load<Sprite>("ElementType/Spectre");
                break;
            case ElementType.Fighting:
                icon = Resources.Load<Sprite>("ElementType/Fighting");
                break;
            case ElementType.Ice:
                icon = Resources.Load<Sprite>("ElementType/Ice");
                break;
            case ElementType.Wind:
                icon = Resources.Load<Sprite>("ElementType/Wind");
                break;
            case ElementType.Earth:
                icon = Resources.Load<Sprite>("ElementType/Earth");
                break;
            case ElementType.Metal:
                icon = Resources.Load<Sprite>("ElementType/Metal");
                break;
            case ElementType.Insect:
                icon = Resources.Load<Sprite>("ElementType/Insect");
                break;
            case ElementType.Unholy:
                icon = Resources.Load<Sprite>("ElementType/Unholy");
                break;
            case ElementType.Holy:
                icon = Resources.Load<Sprite>("ElementType/Holy");
                break;
            case ElementType.Ancient:
                icon = Resources.Load<Sprite>("ElementType/Ancient");
                break;
            case ElementType.None:
                icon = null;
                break;
            default:
                icon = null;
                break;
        }
        return icon;
    }
}


