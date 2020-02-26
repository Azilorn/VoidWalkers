using UnityEditor;
using UnityEngine;

public class AbilityList : EditorWindow
{
    string windowTitle = "Ability List";
    public static EditorWindow window;
    public AbilityTable abilityTable;
    Vector2 scrollPos;
    public static bool windowOpen;
    ElementType elementSearch;
    PositiveAilment positiveSearch;
    NegativeAilment negativeSearch;
    //List<Rect>

    [MenuItem("GameElements/AbilityList %&t")]
    public static void ShowWindow()
    {
        window = GetWindow(typeof(AbilityList));
        window.minSize = new Vector2(880, 1015);
        window.maxSize = new Vector2(1200, 900);
        windowOpen = true;
    }
    public static void CloseWindow()
    {
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
        {
            return;
        }

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
                        if (CreatureSOWindow.windowOpen)
                        {
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

            if (CreatureSOWindow.GettingAbility == true || CreatureSOWindow.GettingLearnedAbility == true || PartyEditor.GettingAbility == true)
                GUILayout.Label("#", GUILayout.Width(120));
            else
                GUILayout.Label("#", GUILayout.Width(60));
            GUILayout.Label("Name", GUILayout.Width(130));
            GUILayout.Label("Type", GUILayout.Width(75));
            elementSearch = (ElementType)EditorGUILayout.EnumPopup(elementSearch, GUILayout.Width(75));
            positiveSearch = (PositiveAilment)EditorGUILayout.EnumPopup(positiveSearch, GUILayout.Width(75));
            negativeSearch = (NegativeAilment)EditorGUILayout.EnumPopup(negativeSearch, GUILayout.Width(75));
            GUILayout.Label("Count", GUILayout.Width(75));
            GUILayout.Label("Power", GUILayout.Width(75));
            GUILayout.Label("Accuracy", GUILayout.Width(75));
            GUILayout.Label("Percentage", GUILayout.Width(75));
            GUILayout.Label("Floor", GUILayout.Width(75));
            GUILayout.EndHorizontal();
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            for (int i = 0; i < abilityTable.Abilities.Count; i++)
            {
                if (elementSearch == ElementType.None) { }
                else if (abilityTable.Abilities[i].elementType == elementSearch)
                { }
                else continue;
                if (positiveSearch == PositiveAilment.None) { }
                else if (abilityTable.Abilities[i].positiveAilment == positiveSearch) { }
                else continue;
                if (negativeSearch == NegativeAilment.None) { }
                else if (abilityTable.Abilities[i].negativeAilment == negativeSearch) { }
                else continue;

                if (i > 0)
                {
                    Rect r = GUILayoutUtility.GetLastRect();
                    if ((i % 2) == 0)
                    {
                        EditorGUI.DrawRect(r, new Color32(125, 125, 125, 100));
                    }
                    if (CreatureSOWindow.GettingAbility == true || CreatureSOWindow.GettingLearnedAbility == true || PartyEditor.GettingAbility == true)
                    {
                        if (r.Contains(Event.current.mousePosition))
                        {
                            EditorGUI.DrawRect(r, new Color32(0, 255, 0, 50));
                        }
                    }
                }

                GUIStyle noAnimations = new GUIStyle();
                noAnimations.normal.textColor = Color.red;
                GUILayout.BeginHorizontal();
                if (abilityTable.Abilities[i].animations.Count > 0)
                {
                    if (abilityTable.Abilities[i].animations.Count < 2 && abilityTable.Abilities[i].animations[0].animation != ImageAnimation.None)
                    {
                        GUILayout.Label(abilityTable.Abilities[i].name, noAnimations, GUILayout.Width(40));
                    }
                    else GUILayout.Label(abilityTable.Abilities[i].name, GUILayout.Width(40));
                }
                else if (abilityTable.Abilities[i].animations.Count == 0)
                {
                    GUILayout.Label(abilityTable.Abilities[i].name, noAnimations, GUILayout.Width(40));
                }
                else GUILayout.Label(abilityTable.Abilities[i].name, GUILayout.Width(40));

                if (GUILayout.Button("I", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    AbilityEditor.ability = abilityTable.Abilities[i];
                    AbilityEditor.ShowWindow();
                }
                if (CreatureSOWindow.GettingAbility == true || CreatureSOWindow.GettingLearnedAbility == true|| PartyEditor.GettingAbility == true)
                {
                    if (GUILayout.Button("Select", GUILayout.Width(50), GUILayout.Height(20)))
                    {
                        if (CreatureSOWindow.GettingAbility == true)
                        {
                            CreatureSOWindow.GettingAbility = false;
                            CreatureSOWindow.creatureScriptableObject.startingAbilities[CreatureSOWindow.GettingAbilityIndex] = abilityTable.Abilities[i];
                            CreatureSOWindow.GettingAbilityIndex = 0;

                        }
                        else if (CreatureSOWindow.GettingLearnedAbility == true)
                        {
                            CreatureSOWindow.GettingLearnedAbility = false;
                            CreatureSOWindow.creatureScriptableObject.learnableAbility[CreatureSOWindow.GettingLearnedAbilityIndex].abilityToLearn = abilityTable.Abilities[i];
                            CreatureSOWindow.GettingLearnedAbilityIndex = 0;
                        }
                        else if (PartyEditor.GettingAbility == true) {
                            PartyEditor.GettingAbility = false;
                            PartyEditor.pcs.creatureAbilities[PartyEditor.GettingAbilityIndex].ability = abilityTable.Abilities[i];
                            PartyEditor.pcs.creatureAbilities[PartyEditor.GettingAbilityIndex].remainingCount = abilityTable.Abilities[i].abilityStats.maxCount;
                            PartyEditor.GettingAbilityIndex = 0;
                        }
                    }
                }
                abilityTable.Abilities[i].abilityName = GUILayout.TextField(abilityTable.Abilities[i].abilityName, GUILayout.Width(140));

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
                {
                    positiveType.normal.textColor = Color.green;
                }
                else
                {
                    positiveType.normal.textColor = Color.grey;
                }

                abilityTable.Abilities[i].positiveAilment = (PositiveAilment)EditorGUILayout.EnumPopup(abilityTable.Abilities[i].positiveAilment, positiveType, GUILayout.Width(75));
                GUIStyle negativeType = new GUIStyle();
                if (abilityTable.Abilities[i].negativeAilment != NegativeAilment.None)
                {
                    negativeType.normal.textColor = Color.red;
                }
                else
                {
                    negativeType.normal.textColor = Color.grey;
                }

                abilityTable.Abilities[i].negativeAilment = (NegativeAilment)EditorGUILayout.EnumPopup(abilityTable.Abilities[i].negativeAilment, negativeType, GUILayout.Width(75));
                abilityTable.Abilities[i].abilityStats.maxCount = EditorGUILayout.IntField(abilityTable.Abilities[i].abilityStats.maxCount, GUILayout.Width(75));
                abilityTable.Abilities[i].abilityStats.power = EditorGUILayout.IntField(abilityTable.Abilities[i].abilityStats.power, GUILayout.Width(75));
                abilityTable.Abilities[i].abilityStats.accuracy = EditorGUILayout.IntField(abilityTable.Abilities[i].abilityStats.accuracy, GUILayout.Width(75));
                abilityTable.Abilities[i].abilityStats.percentage = EditorGUILayout.FloatField(abilityTable.Abilities[i].abilityStats.percentage, GUILayout.Width(75));
                abilityTable.Abilities[i].floorAvailable = (FloorAvailable)EditorGUILayout.EnumPopup(abilityTable.Abilities[i].floorAvailable, GUILayout.Width(75));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();

            GUILayout.Space(20);
            if (GUILayout.Button("Create New Ability"))
            {

                Ability newAbility = CreateInstance<Ability>();
                AssetDatabase.CreateAsset(newAbility, "Assets/GameElements/Ability/A.0" + (abilityTable.Abilities.Count + 1) + ".asset");
                abilityTable.Abilities.Add(newAbility);
                newAbility.elementType = elementSearch;
                AssetDatabase.SaveAssets();
            }

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
    public Sprite ReturnElementSprite(ElementType elementType) {
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