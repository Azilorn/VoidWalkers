using UnityEditor;
using UnityEngine;

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
            GUILayout.Label("Floor", GUILayout.Width(75));
            GUILayout.EndHorizontal();
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            for (int i = 0; i < abilityTable.Abilities.Count; i++)
            {
                if (i > 0)
                {
                    if ((i % 2) == 0)
                    {
                        EditorGUI.DrawRect(GUILayoutUtility.GetLastRect(), new Color32(125, 125, 125, 100));
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
                else if (abilityTable.Abilities[i].animations.Count == 0) {
                    GUILayout.Label(abilityTable.Abilities[i].name, noAnimations, GUILayout.Width(40));
                }
                else GUILayout.Label(abilityTable.Abilities[i].name, GUILayout.Width(40));

                GUIStyle style = new GUIStyle();
                if (CreatureSOWindow.GettingAbility || CreatureSOWindow.GettingLearnedAbility)
                {
                    style.normal.textColor = Color.green;
                }
                else
                {
                    style.normal.textColor = Color.red;
                }

                style.alignment = TextAnchor.MiddleCenter;

                GUIStyle plusStyle = new GUIStyle();
                plusStyle.normal.textColor = Color.green;
                if (GUILayout.Button("+", plusStyle, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    AbilityEditor.ability = abilityTable.Abilities[i];
                    AbilityEditor.ShowWindow();
                }
                if (GUILayout.Button("0", style, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    if (CreatureSOWindow.GettingAbility == true)
                    {
                        CreatureSOWindow.GettingAbility = false;
                        CreatureSOWindow.creatureScriptableObject.startingAbilities[CreatureSOWindow.GettingAbilityIndex] = abilityTable.Abilities[i];
                        CreatureSOWindow.GettingAbilityIndex = 0;

                    } else if(CreatureSOWindow.GettingLearnedAbility == true)
                    {
                        CreatureSOWindow.GettingLearnedAbility = false;
                        CreatureSOWindow.creatureScriptableObject.learnableAbility[CreatureSOWindow.GettingLearnedAbilityIndex].abilityToLearn = abilityTable.Abilities[i];
                        CreatureSOWindow.GettingLearnedAbilityIndex = 0;
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
                AssetDatabase.SaveAssets();
            }

            #region Count
            int normalCount = 0;
            int fireCount = 0;
            int waterCount = 0;
            int natureCount = 0;
            int electricCount = 0;
            int spectreCount = 0;
            int fightingCount = 0;
            int iceCount = 0;
            int windCount = 0;
            int rockCount = 0;
            int metalCount = 0;

            for (int i = 0; i < abilityTable.Abilities.Count; i++)
            {
                switch (abilityTable.Abilities[i].elementType)
                {
                    case ElementType.Normal:
                        normalCount++;
                        break;
                    case ElementType.Fire:
                        fireCount++;
                        break;
                    case ElementType.Water:
                        waterCount++;
                        break;
                    case ElementType.Nature:
                        natureCount++;
                        break;
                    case ElementType.Electric:
                        electricCount++;
                        break;
                    case ElementType.Spectre:
                        spectreCount++;
                        break;
                    case ElementType.Fighting:
                        fightingCount++;
                        break;
                    case ElementType.Ice:
                        iceCount++;
                        break;
                    case ElementType.Wind:
                        windCount++;
                        break;
                    case ElementType.Earth:
                        rockCount++;
                        break;
                    case ElementType.Metal:
                        metalCount++;
                        break;
                    case ElementType.None:
                        break;
                }
            }
            GUILayout.BeginHorizontal();
            
            GUILayout.BeginVertical();
            GUILayout.Label("Normal", GUILayout.Width(75));
            GUILayout.Label(normalCount.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Fire", GUILayout.Width(75));
            GUILayout.Label(fireCount.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Water", GUILayout.Width(75));
            GUILayout.Label(waterCount.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Nature", GUILayout.Width(75));
            GUILayout.Label(natureCount.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Electric", GUILayout.Width(75));
            GUILayout.Label(electricCount.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Spectre", GUILayout.Width(75));
            GUILayout.Label(spectreCount.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Fighting", GUILayout.Width(75));
            GUILayout.Label(fightingCount.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Ice", GUILayout.Width(75));
            GUILayout.Label(iceCount.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Wind", GUILayout.Width(75));
            GUILayout.Label(windCount.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Rock", GUILayout.Width(75));
            GUILayout.Label(rockCount.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Metal", GUILayout.Width(75));
            GUILayout.Label(metalCount.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            #endregion

            #region PositiveCount
            int SpeedUp = 0;
            int SpeedUpUp = 0;
            int AttackUp = 0;
            int AttackUpUp = 0;
            int DefenceUp = 0;
            int DefenceUpUp = 0;
            int AccuracyUp = 0;
            int AccuracyUpUp = 0;
            int DodgeUp = 0;
            int DodgeUpUp = 0;
            int Heal = 0;
            int CritATKUp = 0;
            int CritATKUpUp = 0;
            int CritDEFUp = 0;
            int CritDEFUpUp = 0;

            for (int i = 0; i < abilityTable.Abilities.Count; i++) {

                switch (abilityTable.Abilities[i].positiveAilment)
                {
                    case PositiveAilment.None:
                        break;
                    case PositiveAilment.SpeedUp:
                        SpeedUp++;
                        break;
                    case PositiveAilment.SpeedUpUp:
                        SpeedUpUp++;
                        break;
                    case PositiveAilment.AttackUp:
                        AttackUp++;
                        break;
                    case PositiveAilment.AttackUpUp:
                        AttackUpUp++;
                        break;
                    case PositiveAilment.DefenceUp:
                        DefenceUp++;
                        break;
                    case PositiveAilment.DefenceUpUp:
                        DefenceUpUp++;
                        break;
                    case PositiveAilment.AccuracyUp:
                        AccuracyUp++;
                        break;
                    case PositiveAilment.AccuracyUpUp:
                        AccuracyUpUp++;
                        break;
                    case PositiveAilment.DodgeUp:
                        DodgeUp++;
                        break;
                    case PositiveAilment.DodgeUpUp:
                        DodgeUpUp++;
                        break;
                    case PositiveAilment.Heal:
                        Heal++;
                        break;
                    case PositiveAilment.CritATKUp:
                        CritATKUp++;
                        break;
                    case PositiveAilment.CritATKUpUp:
                        CritATKUpUp++;
                        break;
                    case PositiveAilment.CritDEFUp:
                        CritDEFUp++;
                        break;
                    case PositiveAilment.CritDEFUpUp:
                        CritDEFUpUp++;
                        break;
                }
            }
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Label("SpeedUp", GUILayout.Width(75));
            GUILayout.Label(SpeedUp.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("SpeedUpUp", GUILayout.Width(75));
            GUILayout.Label(SpeedUpUp.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("AttackUp", GUILayout.Width(75));
            GUILayout.Label(AttackUp.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("AttackUpUp", GUILayout.Width(75));
            GUILayout.Label(AttackUpUp.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("DefenceUp", GUILayout.Width(75));
            GUILayout.Label(DefenceUp.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("DefenceUpUp", GUILayout.Width(75));
            GUILayout.Label(DefenceUpUp.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("AccuracyUp", GUILayout.Width(75));
            GUILayout.Label(AccuracyUp.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("AccuracyUpUp", GUILayout.Width(75));
            GUILayout.Label(AccuracyUpUp.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("DodgeUp", GUILayout.Width(75));
            GUILayout.Label(DodgeUp.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("DodgeUpUp", GUILayout.Width(75));
            GUILayout.Label(DodgeUpUp.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Heal", GUILayout.Width(75));
            GUILayout.Label(Heal.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("CritATKUp", GUILayout.Width(75));
            GUILayout.Label(CritATKUp.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("CritATKUpUp", GUILayout.Width(75));
            GUILayout.Label(CritATKUpUp.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("CritDEFUp", GUILayout.Width(75));
            GUILayout.Label(CritDEFUp.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("CritDEFUpUp", GUILayout.Width(75));
            GUILayout.Label(CritDEFUpUp.ToString(), GUILayout.Width(75));
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            #endregion
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
}