using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class AbilityList : EditorWindow
{
    string windowTitle = "Ability List";
    public static EditorWindow window;
    public AbilityTable abilityTable;
    Vector2 scrollPos;
    public static bool windowOpen;
    //List<Rect>

    [MenuItem("GameElements/AbilityList %&t")]
    public static void ShowWindow()
    {
        window = GetWindow(typeof(AbilityList));
        window.minSize = new Vector2(880, 1015);
        window.maxSize = new Vector2(880, 1015);
        windowOpen = true;
    }
    public static void CloseWindow() {
        windowOpen = false;
        window.Close();
    }
    private void OnDisable()
    {
        windowOpen = false;
    }
    private void OnInspectorUpdate()
    {
        Repaint();

        if (abilityTable == null)
            return;
        for (int i = 0; i < abilityTable.Abilities.Count; i++)
        {
            EditorUtility.SetDirty(abilityTable.Abilities[i]);
        }

        if (abilityTable != null)
        {
            EditorUtility.SetDirty(abilityTable);
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;

        switch (e.type)
        {
            case EventType.KeyDown:
                {
                 
                 if (Event.current.keyCode == (KeyCode.RightArrow))
                    {              
                            if (CreatureSOWindow.windowOpen) {
                                Debug.Log("Focus");
                                CreatureSOWindow.window.Focus();
                            }
                            CloseWindow();
                    }
                }
                break;
        }

        GUILayout.Label("AbilityList", EditorStyles.boldLabel);
        if (abilityTable == null)
        {
            GameObject go = Resources.Load("AbilityTable") as GameObject;
            abilityTable = go.GetComponent<AbilityTable>();
        }
        if (abilityTable != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("#", GUILayout.Width(75));
            GUILayout.Label("Name", GUILayout.Width(150));
            GUILayout.Label("Type", GUILayout.Width(75));
            GUILayout.Label("Element", GUILayout.Width(75));
            GUILayout.Label("Positive", GUILayout.Width(75));
            GUILayout.Label("Negative", GUILayout.Width(75));
            GUILayout.Label("Count", GUILayout.Width(75));
            GUILayout.Label("Power", GUILayout.Width(75));
            GUILayout.Label("Accuracy", GUILayout.Width(75));
            GUILayout.Label("Percentage", GUILayout.Width(75));
            GUILayout.EndHorizontal();

            scrollPos = GUILayout.BeginScrollView(scrollPos);
            for (int i = 0; i < abilityTable.Abilities.Count; i++)
            {
                if (i > 0)
                    if ((i % 2) == 0)
                        EditorGUI.DrawRect(GUILayoutUtility.GetLastRect(), new Color32(125, 125, 125, 100));
                   
                GUILayout.BeginHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label(abilityTable.Abilities[i].name, GUILayout.Width(75));

                GUIStyle style = new GUIStyle();
                if (CreatureSOWindow.GettingAbility)
                    style.normal.textColor = Color.green;
                else style.normal.textColor = Color.red;
                style.alignment = TextAnchor.MiddleCenter;

                GUIStyle plusStyle = new GUIStyle();
                plusStyle.normal.textColor = Color.green;
                if (GUILayout.Button("+", plusStyle, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    AbilityEditor.ability = abilityTable.Abilities[i];
                    AbilityEditor.ShowWindow();
                }
                    if (GUILayout.Button("0", style, GUILayout.Width(20), GUILayout.Height(20))) {
                    if (CreatureSOWindow.GettingAbility) {

                        CreatureSOWindow.GettingAbility = false;
                        CreatureSOWindow.creatureScriptableObject.startingAbilities[CreatureSOWindow.GettingAbilityIndex] = abilityTable.Abilities[i];
                        CreatureSOWindow.GettingAbilityIndex = 0;
                    }
                }
                GUILayout.EndHorizontal();
                abilityTable.Abilities[i].abilityName = GUILayout.TextField(abilityTable.Abilities[i].abilityName, GUILayout.Width(150));

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
                abilityTable.Abilities[i].type = (AbilityType)EditorGUILayout.EnumPopup(abilityTable.Abilities[i].type, typeLable, GUILayout.Width(75));

                GUIStyle elementType = new GUIStyle();
                elementType.normal.textColor = ReturnElementTypeColor(abilityTable.Abilities[i].elementType);
                abilityTable.Abilities[i].elementType = (ElementType)EditorGUILayout.EnumPopup(abilityTable.Abilities[i].elementType, elementType, GUILayout.Width(75));
                GUIStyle positiveType = new GUIStyle();
                if (abilityTable.Abilities[i].positiveAilment != PositiveAilment.None)
                    positiveType.normal.textColor = Color.green;
                else positiveType.normal.textColor = Color.grey;
                abilityTable.Abilities[i].positiveAilment = (PositiveAilment)EditorGUILayout.EnumPopup(abilityTable.Abilities[i].positiveAilment, positiveType, GUILayout.Width(75));
                GUIStyle negativeType = new GUIStyle();
                if (abilityTable.Abilities[i].negativeAilment != NegativeAilment.None)
                    negativeType.normal.textColor = Color.red;
                else negativeType.normal.textColor = Color.grey;
                abilityTable.Abilities[i].negativeAilment = (NegativeAilment)EditorGUILayout.EnumPopup(abilityTable.Abilities[i].negativeAilment, negativeType, GUILayout.Width(75));
                abilityTable.Abilities[i].abilityStats.maxCount = EditorGUILayout.IntField(abilityTable.Abilities[i].abilityStats.maxCount, GUILayout.Width(75));
                abilityTable.Abilities[i].abilityStats.power = EditorGUILayout.IntField(abilityTable.Abilities[i].abilityStats.power, GUILayout.Width(75));
                abilityTable.Abilities[i].abilityStats.accuracy = EditorGUILayout.IntField(abilityTable.Abilities[i].abilityStats.accuracy, GUILayout.Width(75));
                abilityTable.Abilities[i].abilityStats.percentage = EditorGUILayout.FloatField(abilityTable.Abilities[i].abilityStats.percentage, GUILayout.Width(75));
                GUILayout.EndHorizontal();
            }
                GUILayout.Space(20);
                if (GUILayout.Button("Create New Ability"))
                {

                    Ability newAbility = CreateInstance<Ability>();
                    AssetDatabase.CreateAsset(newAbility, "Assets/GameElements/Ability/A.0" + (abilityTable.Abilities.Count + 1) + ".asset");
                    abilityTable.Abilities.Add(newAbility);
                    AssetDatabase.SaveAssets();
                }
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
            case ElementType.Rock:
                return Color.black;
            case ElementType.Metal:
                return Color.grey;
            case ElementType.None:
                return Color.grey;
            default:
                return Color.black;
        }
    }

 
}
