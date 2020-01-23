using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PartyEditor : EditorWindow
{
    int partySelectedCreature;
    static int partySize;
    static int averageLevel;
    string windowTitle = "Party Editor";
    static EditorWindow window;
    GameObject partyPrefab;
    PlayerParty party;
    PlayerCreatureStats pcs;
    AbilityTable abilityTable;
    Vector2 scrollPos;

 [MenuItem("GameElements/PartyEditor %&w")]
    public static void ShowWindow()
    {
        window = GetWindow(typeof(PartyEditor));
        window.minSize = new Vector2(880, 1015);
        window.maxSize = new Vector2(880, 1015);
    }
    private void OnDisable()
    {
        if (party != null)
            EditorUtility.SetDirty(party);
    }
    private void OnInspectorUpdate()
    {
        Repaint();
        if (abilityTable != null)
            for (int i = 0; i < abilityTable.Abilities.Count; i++) {
            EditorUtility.SetDirty(abilityTable.Abilities[i]);
        }

        if (abilityTable != null) {
            EditorUtility.SetDirty(abilityTable);
        }
        if (party == null)
            return;
        EditorUtility.SetDirty(party);
        for (int i = 0; i < pcs.creatureAbilities.Length; i++) {
            if(pcs.creatureAbilities[i].ability != null)
                EditorUtility.SetDirty(pcs.creatureAbilities[i].ability);
        }
    }
    private void OnGUI() {

        GUILayout.Label(windowTitle, EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Width", EditorStyles.label, GUILayout.Width(75));
        float width = EditorGUILayout.FloatField(position.width, GUILayout.Width(50));
        GUILayout.Label("Height", EditorStyles.label, GUILayout.Width(75));
        float height = EditorGUILayout.FloatField(position.height, GUILayout.Width(50));
        GUILayout.EndHorizontal();

        position.Set(position.x, position.y, width, height);
        
        partyPrefab = (GameObject)EditorGUILayout.ObjectField(partyPrefab, typeof(GameObject), false);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Random Party Count:", EditorStyles.boldLabel, GUILayout.Width(150));
        partySize = GUILayout.Toolbar(partySize, new string[] { "1", "2", "3", "4", "5", "6" });
        GUILayout.Label("Avg Lvl:", EditorStyles.boldLabel, GUILayout.Width(50));
        averageLevel = EditorGUILayout.IntField(averageLevel);
        if (GUILayout.Button("Randomize", GUILayout.Width(100)))
        {
            GameObject go = Resources.Load("CreatureTable") as GameObject;
            CreatureTable creatureTable = go.GetComponent<CreatureTable>();
            for (int i = 0; i < partySize + 1; i++)
            {
              
                party.party[i].creatureSO = creatureTable.Creatures[UnityEngine.Random.Range(0, creatureTable.Creatures.Count)];
            }
        }
        if (GUILayout.Button("Set Avg Lvl", GUILayout.Width(100)))
        {
            for (int i = 0; i < partySize + 1; i++)
            {
                party.party[i].SetLevel(averageLevel, true);
            }
        }
        GUILayout.EndHorizontal();

        if (partyPrefab != null)
        {
            party = partyPrefab.GetComponent<PlayerParty>();

            GUILayout.Label("Player Party", EditorStyles.boldLabel);
            partySelectedCreature = GUILayout.Toolbar(partySelectedCreature, new string[] { "1", "2", "3", "4", "5", "6" });

            if (party.party.Length < partySelectedCreature - 1)
                return;

            pcs = party.party[partySelectedCreature];

            //Creature Name;
            if(pcs.creatureSO == null)
                GUILayout.Label("Empty Creature Slot", EditorStyles.boldLabel);
            else GUILayout.Label(pcs.creatureSO.creatureName, EditorStyles.boldLabel);

            Rect lastrect = GUILayoutUtility.GetRect(75, 75, GUIStyle.none, GUILayout.Width(75), GUILayout.Height(75));
            if(pcs.creatureSO != null)
            EditorGUI.DrawTextureTransparent(lastrect, pcs.creatureSO.creaturePlayerIcon.texture, ScaleMode.ScaleToFit);

            pcs.creatureSO = (CreatureSO)EditorGUILayout.ObjectField(pcs.creatureSO, typeof(CreatureSO), false);

            if (pcs.creatureSO == null)
                return;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.BeginHorizontal();
            //Middle Centered Bold Wrapped Label
            GUIStyle centeredLable = new GUIStyle();
            centeredLable.wordWrap = true;
            centeredLable.alignment = TextAnchor.MiddleCenter;
            centeredLable.fontStyle = FontStyle.Bold;
            //Primary Element Style
            GUIStyle creaturePrimaryElementStyle = new GUIStyle();
            creaturePrimaryElementStyle.normal.textColor = ReturnElementTypeColor(pcs.creatureSO.primaryElement);
            creaturePrimaryElementStyle.wordWrap = true;
            creaturePrimaryElementStyle.alignment = TextAnchor.MiddleCenter;
            creaturePrimaryElementStyle.fontStyle = FontStyle.Bold;
            GUILayout.Label("Primary Element: ", centeredLable, GUILayout.Width(125));
            GUILayout.Label(pcs.creatureSO.primaryElement.ToString(), creaturePrimaryElementStyle, GUILayout.Width(100));
            //Secondary Element Style
            GUIStyle creatureSecondaryElementStyle = new GUIStyle();
            creatureSecondaryElementStyle.normal.textColor = ReturnElementTypeColor(pcs.creatureSO.secondaryElement);
            creatureSecondaryElementStyle.wordWrap = true;
            creatureSecondaryElementStyle.alignment = TextAnchor.MiddleCenter;
            creatureSecondaryElementStyle.fontStyle = FontStyle.Bold;
            GUILayout.Label("Secondary Element: ", centeredLable, GUILayout.Width(125));
            GUILayout.Label(pcs.creatureSO.secondaryElement.ToString(), creatureSecondaryElementStyle, GUILayout.Width(100));
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
           
            //Stats
            GUILayout.BeginHorizontal();
            //Level
            GUILayout.Label("Level: ", EditorStyles.boldLabel, GUILayout.Width(40));
            pcs.creatureStats.level = EditorGUILayout.IntField(pcs.creatureStats.level, GUILayout.Width(50));
            if (GUILayout.Button("Update Stats", GUILayout.Width(100)))
                if (party != null)
                    party.UpdateStats(partySelectedCreature);
            if (GUILayout.Button(" + ", GUILayout.Width(50)))
                if (party != null)
                    party.LevelUp(partySelectedCreature);
            if (GUILayout.Button(" - ", GUILayout.Width(50)))
                if (party != null)
                    party.LevelDown(partySelectedCreature);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            //HP
            GUILayout.BeginVertical();
            GUILayout.Label("Max HP", EditorStyles.boldLabel);
            pcs.creatureStats.MaxHP = EditorGUILayout.IntField(pcs.creatureStats.MaxHP, GUILayout.Width(50));
            GUILayout.EndVertical();

            //Strength
            GUILayout.BeginVertical();
            GUILayout.Label("Strength", EditorStyles.boldLabel);
            pcs.creatureStats.strength = EditorGUILayout.IntField(pcs.creatureStats.strength, GUILayout.Width(50));
            GUILayout.EndVertical();

            //Defence
            GUILayout.BeginVertical();
            GUILayout.Label("Defence", EditorStyles.boldLabel);
            pcs.creatureStats.defence = EditorGUILayout.IntField(pcs.creatureStats.defence, GUILayout.Width(50));
            GUILayout.EndVertical();

            //Speed
            GUILayout.BeginVertical();
            GUILayout.Label("Speed", EditorStyles.boldLabel);
            pcs.creatureStats.speed = EditorGUILayout.IntField(pcs.creatureStats.speed, GUILayout.Width(50));
            GUILayout.EndVertical();

            //Critical Attack
            GUILayout.BeginVertical();
            GUILayout.Label("Critical Attack", EditorStyles.boldLabel);
            pcs.creatureStats.criticalAttack = EditorGUILayout.IntField(pcs.creatureStats.criticalAttack, GUILayout.Width(50));
            GUILayout.EndVertical();

            //Critical Defence
            GUILayout.BeginVertical();
            GUILayout.Label("Critical Defence", EditorStyles.boldLabel);
            pcs.creatureStats.criticalDefence = EditorGUILayout.IntField(pcs.creatureStats.criticalDefence, GUILayout.Width(50));
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.Label("Abilities", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

          
            if (GUILayout.Button("Set Starting Abilities"))
            {
                if (pcs.creatureSO.startingAbilities.Count < 1) {
                    Debug.Log("Set Starting Abilities");
                    return;
                }
                for (int i = 0; i < 4; i++)
                {
                    if (i >= pcs.creatureSO.startingAbilities.Count)
                        continue;
                    if (pcs.creatureSO.startingAbilities[i] != null)
                    {
                        pcs.creatureAbilities[i].ability = pcs.creatureSO.startingAbilities[i];
                        pcs.creatureAbilities[i].remainingCount = pcs.creatureSO.startingAbilities[i].abilityStats.maxCount;
                    }
                }
            }
            GUILayout.BeginHorizontal();
            for (int i = 0; i < pcs.creatureAbilities.Length; i++)
            {
                GUILayout.BeginVertical();

                if (pcs.creatureAbilities[i].ability == null)
                {
                    GUILayout.Label("Empty Skill", EditorStyles.boldLabel);
                    pcs.creatureAbilities[i].ability = (Ability)EditorGUILayout.ObjectField(pcs.creatureAbilities[i].ability, typeof(Ability), false, GUILayout.Width(150));
                    GUILayout.EndVertical();
                    continue;
                }
                else {
                    GUILayout.Label(pcs.creatureAbilities[i].ability.abilityName, EditorStyles.boldLabel);
                    pcs.creatureAbilities[i].ability = (Ability)EditorGUILayout.ObjectField(pcs.creatureAbilities[i].ability, typeof(Ability), false, GUILayout.Width(150));
                }


                if (pcs.creatureAbilities[i].ability == null)
                    return;
                //Ability Type
                GUILayout.BeginHorizontal();
                GUILayout.Label("Ability Type", EditorStyles.wordWrappedLabel, GUILayout.Width(100));
                if (pcs.creatureAbilities[i].ability != null)
                    pcs.creatureAbilities[i].ability.type = (AbilityType)EditorGUILayout.EnumPopup(pcs.creatureAbilities[i].ability.type, GUILayout.Width(100), GUILayout.Height(20));
                GUILayout.EndHorizontal();

                //Element Type
                GUIStyle elementStyle = new GUIStyle();
                elementStyle.normal.textColor = ReturnElementTypeColor(pcs.creatureAbilities[i].ability.elementType);
                elementStyle.wordWrap = true;
                elementStyle.alignment = TextAnchor.MiddleCenter;
                elementStyle.fontStyle = FontStyle.Bold;
                GUILayout.BeginHorizontal();
                GUILayout.Label("Element Type", EditorStyles.wordWrappedLabel, GUILayout.Width(100));
                pcs.creatureAbilities[i].ability.elementType = (ElementType)EditorGUILayout.EnumPopup(pcs.creatureAbilities[i].ability.elementType, elementStyle, GUILayout.Width(100), GUILayout.Height(20));
                Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                texture.SetPixel(0, 0, new Color32(0, 0, 0, 45));
                texture.Apply();
                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
                GUILayout.EndHorizontal();
                //Positive Ailment

                if(pcs.creatureAbilities[i].ability.positiveAilment == PositiveAilment.None && pcs.creatureAbilities[i].ability.negativeAilment == NegativeAilment.None)
                    GUILayout.Space(20);
                if (pcs.creatureAbilities[i].ability.positiveAilment != PositiveAilment.None)
                {
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.green;
                    style.wordWrap = true;
                    style.alignment = TextAnchor.MiddleCenter;
                    style.fontStyle = FontStyle.Bold;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Positive Ailment", GUILayout.Width(100));
                    pcs.creatureAbilities[i].ability.positiveAilment = (PositiveAilment)EditorGUILayout.EnumPopup(pcs.creatureAbilities[i].ability.positiveAilment, style, GUILayout.Width(100), GUILayout.Height(20));
                    texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                    texture.SetPixel(0, 0, new Color32(125, 125, 125, 125));
                    texture.Apply();
                    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
                    GUILayout.EndHorizontal();
                }
                //Negative Ailment
                if (pcs.creatureAbilities[i].ability.negativeAilment != NegativeAilment.None)
                {
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.red;
                    style.wordWrap = true;
                    style.alignment = TextAnchor.MiddleCenter;
                    style.fontStyle = FontStyle.Bold;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Negative Ailment",   GUILayout.Width(100));
                    pcs.creatureAbilities[i].ability.negativeAilment = (NegativeAilment)EditorGUILayout.EnumPopup(pcs.creatureAbilities[i].ability.negativeAilment, style, GUILayout.Width(100), GUILayout.Height(20));
                     texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                    texture.SetPixel(0, 0, new Color32(0, 0, 0, 45));
                    texture.Apply();
                    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
                    GUILayout.EndHorizontal();
                }
                //Remainging Count
                GUILayout.BeginHorizontal();
                GUILayout.Label("Remaining Count", EditorStyles.wordWrappedLabel, GUILayout.Width(100));
                pcs.creatureAbilities[i].remainingCount = EditorGUILayout.IntField(pcs.creatureAbilities[i].remainingCount, GUILayout.Width(50));
                GUILayout.EndHorizontal();
                //Max Count
                GUILayout.BeginHorizontal();
                GUILayout.Label("Max Count", EditorStyles.wordWrappedLabel, GUILayout.Width(100));
                pcs.creatureAbilities[i].ability.abilityStats.maxCount = EditorGUILayout.IntField(pcs.creatureAbilities[i].ability.abilityStats.maxCount, GUILayout.Width(50));
                GUILayout.EndHorizontal();
                //Power
                GUILayout.BeginHorizontal();
                GUILayout.Label("Power", EditorStyles.wordWrappedLabel, GUILayout.Width(100));
                pcs.creatureAbilities[i].ability.abilityStats.power = EditorGUILayout.IntField(pcs.creatureAbilities[i].ability.abilityStats.power, GUILayout.Width(50));
                GUILayout.EndHorizontal();
                //Accuracy
                GUILayout.BeginHorizontal();
                GUILayout.Label("Accuracy", EditorStyles.wordWrappedLabel, GUILayout.Width(100));
                pcs.creatureAbilities[i].ability.abilityStats.accuracy = EditorGUILayout.IntField(pcs.creatureAbilities[i].ability.abilityStats.accuracy, GUILayout.Width(50));
                GUILayout.EndHorizontal();
                //Percentage
                GUILayout.BeginHorizontal();
                GUILayout.Label("Percentage", EditorStyles.wordWrappedLabel);
                pcs.creatureAbilities[i].ability.abilityStats.percentage = EditorGUILayout.Slider((int)pcs.creatureAbilities[i].ability.abilityStats.percentage, 0, 100);
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

        }

        //Ability List
        GUILayout.Label("AbilityList", EditorStyles.boldLabel);
        if (abilityTable == null) {
            GameObject go = Resources.Load("AbilityTable") as GameObject;
            abilityTable = go.GetComponent<AbilityTable>();
        }
        if (abilityTable != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("#", GUILayout.Width(75));
            GUILayout.Label("Name" , GUILayout.Width(150));
            GUILayout.Label("Type", GUILayout.Width(75));
            GUILayout.Label("Element", GUILayout.Width(75));
            GUILayout.Label("Positive", GUILayout.Width(75));
            GUILayout.Label("Negative", GUILayout.Width(75));
            GUILayout.Label("Count", GUILayout.Width(75));
            GUILayout.Label("Power", GUILayout.Width(75));
            GUILayout.Label("Accuracy", GUILayout.Width(75));
            GUILayout.Label("Percentage", GUILayout.Width(75));
            GUILayout.EndHorizontal();
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(300));
            for (int i = 0; i < abilityTable.Abilities.Count; i++)
            {
                if(i > 0)
                    if ((i % 2) == 0)
                        EditorGUI.DrawRect(GUILayoutUtility.GetLastRect(), new Color32(125,125,125, 100));
                GUILayout.BeginHorizontal();
                GUILayout.Label(abilityTable.Abilities[i].name, GUILayout.Width(75));
                abilityTable.Abilities[i].abilityName =  GUILayout.TextArea(abilityTable.Abilities[i].abilityName, GUILayout.Width(150));

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
                abilityTable.Abilities[i].positiveAilment  = (PositiveAilment) EditorGUILayout.EnumPopup(abilityTable.Abilities[i].positiveAilment, positiveType, GUILayout.Width(75));
                GUIStyle negativeType = new GUIStyle();
                if (abilityTable.Abilities[i].negativeAilment != NegativeAilment.None)
                    negativeType.normal.textColor = Color.red;
                else negativeType.normal.textColor = Color.grey;
                abilityTable.Abilities[i].negativeAilment  = (NegativeAilment) EditorGUILayout.EnumPopup(abilityTable.Abilities[i].negativeAilment, negativeType, GUILayout.Width(75));
                abilityTable.Abilities[i].abilityStats.maxCount = EditorGUILayout.IntField(abilityTable.Abilities[i].abilityStats.maxCount, GUILayout.Width(75));
                abilityTable.Abilities[i].abilityStats.power = EditorGUILayout.IntField(abilityTable.Abilities[i].abilityStats.power, GUILayout.Width(75));
                abilityTable.Abilities[i].abilityStats.accuracy = EditorGUILayout.IntField(abilityTable.Abilities[i].abilityStats.accuracy, GUILayout.Width(75));
                abilityTable.Abilities[i].abilityStats.percentage = EditorGUILayout.FloatField(abilityTable.Abilities[i].abilityStats.percentage , GUILayout.Width(75));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();

            GUILayout.Space(20);
            if (GUILayout.Button("Create New Ability")) {

                Ability newAbility = CreateInstance<Ability>();
                AssetDatabase.CreateAsset(newAbility, "Assets/GameElements/Ability/A.0" +  (abilityTable.Abilities.Count + 1) + ".asset");
                abilityTable.Abilities.Add(newAbility);
                AssetDatabase.SaveAssets();
            }
        }
    }



    public Color ReturnElementTypeColor(ElementType type) {

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
}
