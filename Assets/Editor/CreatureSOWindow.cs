using System.IO;
using UnityEditor;
using UnityEngine;

public class CreatureSOWindow : EditorWindow
{

    string windowTitle = "Creature Editor";
    public static CreatureSO creatureScriptableObject;
    CreatureSO copyCreatureDetails;
    public static EditorWindow window;
    CreatureTable creatureTable;
    Vector2 scrollPos;
    bool showPosition = false;
    public static bool windowOpen;
    public static bool GettingAbility;
    public static int GettingAbilityIndex;

    [MenuItem("GameElements/CreatureSO %&v")]
    public static void ShowWindow()
    {
        window = GetWindow(typeof(CreatureSOWindow));
        window.maxSize = new Vector2(900, 1000);
        window.minSize = new Vector2(900, 1000);
        window.maximized = false;
        windowOpen = true;
        GettingAbility = false;
    }
    private void OnDisable()
    {
        if (AbilityList.windowOpen) {
            AbilityList.window.Close();
        }
        windowOpen = false;
    }
    private void OnFocus()
    {
        GettingAbility = false;
    }
    private void OnInspectorUpdate()
    {
        if (creatureScriptableObject != null)
        {
            EditorUtility.SetDirty(creatureScriptableObject);
        }

        if (creatureTable != null)
        {
            EditorUtility.SetDirty(creatureTable);
        }
        Repaint();
    }
    private void OnGUI()
    {

        GUILayout.Label(windowTitle, EditorStyles.boldLabel);
        GUILayout.Label("Creature Scriptable Object", EditorStyles.label);

        if (creatureTable == null)
        {
            GameObject go = Resources.Load("CreatureTable") as GameObject;
            creatureTable = go.GetComponent<CreatureTable>();
        }

        creatureScriptableObject = (CreatureSO)EditorGUILayout.ObjectField(creatureScriptableObject, typeof(CreatureSO), false);
       
        Rect lastrect = GUILayoutUtility.GetRect(100, 20, GUIStyle.none, GUILayout.Width(200), GUILayout.Height(200));

        if (creatureScriptableObject != null)
        {

            Event e = Event.current;

            switch (e.type)
            {
                case EventType.KeyDown:
                    {
                        if (Event.current.keyCode == (KeyCode.DownArrow))
                        {
                            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/GameElements/Creature/");

                            for (int i = 0; i < fileEntries.Length; i++)
                            {
                                string currentName = Application.dataPath + "/GameElements/Creature/" + creatureScriptableObject.name + ".asset";
                                if (currentName == fileEntries[i])
                                {

                                    if (i == fileEntries.Length - 2)
                                    {
                                        string filename = fileEntries[0].Replace(Application.dataPath, "");
                                        CreatureSO LoadedSO = (CreatureSO)AssetDatabase.LoadAssetAtPath("Assets/" + filename, typeof(CreatureSO));
                                        creatureScriptableObject = LoadedSO;
                                        break;
                                    }
                                    else
                                    {
                                        string filename = fileEntries[i + 2].Replace(Application.dataPath, "");
                                        CreatureSO LoadedSO = (CreatureSO)AssetDatabase.LoadAssetAtPath("Assets/" + filename, typeof(CreatureSO));
                                        creatureScriptableObject = LoadedSO;
                                        break;
                                    }
                                }
                            }

                        }
                        else if (Event.current.keyCode == (KeyCode.UpArrow))
                        {
                            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/GameElements/Creature/");

                            for (int i = 0; i < fileEntries.Length; i++)
                            {
                                string currentName = Application.dataPath + "/GameElements/Creature/" + creatureScriptableObject.name + ".asset";
                                if (currentName == fileEntries[i])
                                {

                                    if (i == 0)
                                    {
                                        string filename = fileEntries[fileEntries.Length - 2].Replace(Application.dataPath, "");
                                        Debug.Log(filename);
                                        CreatureSO LoadedSO = (CreatureSO)AssetDatabase.LoadAssetAtPath("Assets/" + filename, typeof(CreatureSO));
                                        Debug.Log(LoadedSO);
                                        creatureScriptableObject = LoadedSO;
                                        break;
                                    }
                                    else
                                    {
                                        string filename = fileEntries[i - 2].Replace(Application.dataPath, "");
                                        CreatureSO LoadedSO = (CreatureSO)AssetDatabase.LoadAssetAtPath("Assets/" + filename, typeof(CreatureSO));
                                        creatureScriptableObject = LoadedSO;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (Event.current.keyCode == (KeyCode.RightArrow))
                        {
                            if (!AbilityList.windowOpen)
                                AbilityList.ShowWindow();
                            else {
                                AbilityList.CloseWindow();
                            }
                            break;
                        }
                    }
                    break;
            }

            if (creatureScriptableObject.creaturePlayerIcon != null)
            {
                EditorGUI.DrawTextureTransparent(lastrect, creatureScriptableObject.creaturePlayerIcon.texture, ScaleMode.ScaleToFit);
            }
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            //Creature Name
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(50));
            creatureScriptableObject.creatureName = EditorGUILayout.TextField(creatureScriptableObject.creatureName, GUILayout.Width(300));
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            //Creature Icons
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Player Icon", EditorStyles.boldLabel);
            creatureScriptableObject.creaturePlayerIcon = (Sprite)EditorGUILayout.ObjectField(creatureScriptableObject.creaturePlayerIcon, typeof(Sprite), false);
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (creatureScriptableObject.creaturePlayerIcon != null)
            {
                Sprite img = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Creature Library/Frontview Batch Battlers/" + creatureScriptableObject.creaturePlayerIcon.name + ".png", typeof(Sprite));
                creatureScriptableObject.creatureEnemyIcon = img;
            }
            GUILayout.EndVertical();
            //Image Width
            GUILayout.BeginVertical();
            GUILayout.Label("Image Width", EditorStyles.boldLabel);
            creatureScriptableObject.width = EditorGUILayout.IntField(creatureScriptableObject.width, GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.EndVertical();
            //Image height
            GUILayout.BeginVertical();
            GUILayout.Label("Image Height", EditorStyles.boldLabel);
            creatureScriptableObject.height = EditorGUILayout.IntField(creatureScriptableObject.height, GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            //Bio and Characteristics
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Bio", EditorStyles.boldLabel);
            creatureScriptableObject.creatureBio = EditorGUILayout.TextArea(creatureScriptableObject.creatureBio, GUILayout.Width(300), GUILayout.Height(50));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            //Height and Weight
            GUILayout.BeginHorizontal();
            //Height (Centimeters)
            GUILayout.BeginVertical();
            GUILayout.Label("Height (Foot and Inchs)", EditorStyles.boldLabel);
            creatureScriptableObject.characteristics.height = EditorGUILayout.TextField(creatureScriptableObject.characteristics.height, GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.EndVertical();
            //Weight (Kilograms)
            GUILayout.BeginVertical();
            GUILayout.Label("Weight (Pounds)", EditorStyles.boldLabel);
            creatureScriptableObject.characteristics.weight = EditorGUILayout.TextField(creatureScriptableObject.characteristics.weight, GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            //Elements
            GUILayout.BeginHorizontal();
            //Primary Element
            GUILayout.BeginVertical();
            GUILayout.Label("Primary Element", EditorStyles.boldLabel);
            creatureScriptableObject.primaryElement = (ElementType)EditorGUILayout.EnumPopup(creatureScriptableObject.primaryElement, GUILayout.Width(75), GUILayout.Height(20));
            GUILayout.EndVertical();
            //Secondary Element
            GUILayout.BeginVertical();
            GUILayout.Label("Secondary Element", EditorStyles.boldLabel);
            creatureScriptableObject.secondaryElement = (ElementType)EditorGUILayout.EnumPopup(creatureScriptableObject.secondaryElement, GUILayout.Width(75), GUILayout.Height(20));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(20);


            //Evolution
            GUILayout.Label("Evolutions", EditorStyles.boldLabel, GUILayout.Width(100));
            creatureScriptableObject.SetEvolutionSize(EditorGUILayout.IntSlider(creatureScriptableObject.evolutions.Count, 0, 3));

            for (int i = 0; i < creatureScriptableObject.evolutions.Count; i++)
            {
                GUILayout.BeginHorizontal();
                if (creatureScriptableObject.evolutions[i] != null)
                {
                    creatureScriptableObject.evolutions[i].evolutionSO = (CreatureSO)EditorGUILayout.ObjectField(creatureScriptableObject.evolutions[i].evolutionSO, typeof(CreatureSO), false);
                }
                GUILayout.Label("Level Requirement", EditorStyles.boldLabel, GUILayout.Width(150));
                if (creatureScriptableObject.evolutions[i] != null)
                {
                    creatureScriptableObject.evolutions[i].levelRequirement = EditorGUILayout.IntSlider(creatureScriptableObject.evolutions[i].levelRequirement, 1, 50);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();

            if (creatureScriptableObject.evolutions != null)
            {
                if (creatureScriptableObject.evolutions.Count > 0 && creatureScriptableObject.evolutions[0].evolutionSO != null)
                {
                    lastrect = GUILayoutUtility.GetRect(100, 100);

                    GUILayout.Label("1#", EditorStyles.label);
                    if (creatureScriptableObject.evolutions[0].evolutionSO.creaturePlayerIcon != null)
                    {
                        EditorGUI.DrawTextureTransparent(lastrect, creatureScriptableObject.evolutions[0].evolutionSO.creaturePlayerIcon.texture, ScaleMode.ScaleToFit);
                    }
                }
                if (creatureScriptableObject.evolutions.Count > 1 && creatureScriptableObject.evolutions[1].evolutionSO != null)
                {
                    lastrect = GUILayoutUtility.GetRect(100, 100);
                    GUILayout.Label("2#", EditorStyles.label);
                    if (creatureScriptableObject.evolutions[1].evolutionSO.creaturePlayerIcon != null)
                    {
                        EditorGUI.DrawTextureTransparent(lastrect, creatureScriptableObject.evolutions[1].evolutionSO.creaturePlayerIcon.texture, ScaleMode.ScaleToFit);
                    }
                }
                if (creatureScriptableObject.evolutions.Count > 2 && creatureScriptableObject.evolutions[2].evolutionSO != null)
                {
                    lastrect = GUILayoutUtility.GetRect(100, 100);
                    GUILayout.Label("3#", EditorStyles.label);
                    if (creatureScriptableObject.evolutions[2].evolutionSO.creaturePlayerIcon != null)
                    {
                        EditorGUI.DrawTextureTransparent(lastrect, creatureScriptableObject.evolutions[2].evolutionSO.creaturePlayerIcon.texture, ScaleMode.ScaleToFit);
                    }
                }

            }
            GUILayout.EndHorizontal();
            //Starting Abilities


            showPosition = EditorGUI.Foldout(GUILayoutUtility.GetRect(100, 20), showPosition, "StartingAbilities");

            if (showPosition)
            {
                if (creatureScriptableObject.startingAbilities.Count == 0)
                {
                    creatureScriptableObject.startingAbilities = new System.Collections.Generic.List<Ability>();
                    creatureScriptableObject.startingAbilities.Add(new Ability());
                }
                GUILayout.BeginHorizontal();
                for (int i = 0; i < creatureScriptableObject.startingAbilities.Count; i++)
                {
                    GUILayout.BeginVertical();

                    if (creatureScriptableObject.startingAbilities.Count < 1)
                    {
                        creatureScriptableObject.startingAbilities.Add(new Ability());
                        creatureScriptableObject.startingAbilities[0] = null;
                    }

                    if (creatureScriptableObject.startingAbilities[i] == null)
                    {
                        GUILayout.Label("Empty Skill", EditorStyles.boldLabel);
                        creatureScriptableObject.startingAbilities[i] = (Ability)EditorGUILayout.ObjectField(creatureScriptableObject.startingAbilities[i], typeof(Ability), false, GUILayout.Width(150));
                        GUILayout.EndVertical();
                        GUIStyle style = new GUIStyle();
                        if (GettingAbility)
                            style.normal.textColor = Color.green;
                        else style.normal.textColor = Color.red;
                        style.alignment = TextAnchor.MiddleCenter;
                        if (GUILayout.Button("0", style, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            GettingAbility = true;
                            GettingAbilityIndex = 0;
                            if (AbilityList.windowOpen)
                                AbilityList.window.Focus();
                            else {
                                AbilityList.ShowWindow();
                                AbilityList.window.Focus();
                            }
                            break;
                        }
                        continue;
                    }
                    else
                    {
                        GUIStyle style = new GUIStyle();
                        if (GettingAbility)
                            style.normal.textColor = Color.green;
                        else style.normal.textColor = Color.red;
                        style.alignment = TextAnchor.MiddleCenter;

                        GUILayout.BeginHorizontal();
                        GUILayout.Label(creatureScriptableObject.startingAbilities[i].abilityName, EditorStyles.boldLabel);
                        if (GUILayout.Button("-"))
                        {
                            creatureScriptableObject.startingAbilities.RemoveAt(i);
                            break;
                        }
                        else if (GUILayout.Button("+"))
                        {
                            if (creatureScriptableObject.startingAbilities.Count < 4)
                                creatureScriptableObject.startingAbilities.Insert(i + 1, new Ability());
                            break;
                        }
                        else if (GUILayout.Button("0", style, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            GettingAbility = true;
                            GettingAbilityIndex = i;
                            if (AbilityList.windowOpen)
                                AbilityList.window.Focus();
                            else
                            {
                                AbilityList.ShowWindow();
                                AbilityList.window.Focus();
                            }
                            AbilityList.window.Focus();
                            break;
                        }

                        GUILayout.EndHorizontal();
                        if (creatureScriptableObject.startingAbilities.Count < 1)
                        {
                            break;
                        }

                        creatureScriptableObject.startingAbilities[i] = (Ability)EditorGUILayout.ObjectField(creatureScriptableObject.startingAbilities[i], typeof(Ability), false, GUILayout.Width(150));
                    }


                    if (creatureScriptableObject.startingAbilities[i] == null)
                    {
                        return;
                    }
                    //Ability Type
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Ability Type", EditorStyles.wordWrappedLabel, GUILayout.Width(100));
                    if (creatureScriptableObject.startingAbilities[i] != null)
                    {
                        creatureScriptableObject.startingAbilities[i].type = (AbilityType)EditorGUILayout.EnumPopup(creatureScriptableObject.startingAbilities[i].type, GUILayout.Width(100), GUILayout.Height(20));
                    }

                    GUILayout.EndHorizontal();

                    //Element Type
                    GUIStyle elementStyle = new GUIStyle();
                    elementStyle.normal.textColor = ReturnElementTypeColor(creatureScriptableObject.startingAbilities[i].elementType);
                    elementStyle.wordWrap = true;
                    elementStyle.alignment = TextAnchor.MiddleCenter;
                    elementStyle.fontStyle = FontStyle.Bold;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Element Type", EditorStyles.wordWrappedLabel, GUILayout.Width(100));
                    creatureScriptableObject.startingAbilities[i].elementType = (ElementType)EditorGUILayout.EnumPopup(creatureScriptableObject.startingAbilities[i].elementType, elementStyle, GUILayout.Width(100), GUILayout.Height(20));
                    Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                    texture.SetPixel(0, 0, new Color32(0, 0, 0, 45));
                    texture.Apply();
                    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
                    GUILayout.EndHorizontal();
                    //Positive Ailment

                    if (creatureScriptableObject.startingAbilities[i].positiveAilment == PositiveAilment.None && creatureScriptableObject.startingAbilities[i].negativeAilment == NegativeAilment.None)
                    {
                        GUILayout.Space(20);
                    }

                    if (creatureScriptableObject.startingAbilities[i].positiveAilment != PositiveAilment.None)
                    {
                        GUIStyle style = new GUIStyle();
                        style.normal.textColor = Color.green;
                        style.wordWrap = true;
                        style.alignment = TextAnchor.MiddleCenter;
                        style.fontStyle = FontStyle.Bold;
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Positive Ailment", GUILayout.Width(100));
                        creatureScriptableObject.startingAbilities[i].positiveAilment = (PositiveAilment)EditorGUILayout.EnumPopup(creatureScriptableObject.startingAbilities[i].positiveAilment, style, GUILayout.Width(100), GUILayout.Height(20));
                        texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                        texture.SetPixel(0, 0, new Color32(125, 125, 125, 125));
                        texture.Apply();
                        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
                        GUILayout.EndHorizontal();
                    }
                    //Negative Ailment
                    if (creatureScriptableObject.startingAbilities[i].negativeAilment != NegativeAilment.None)
                    {
                        GUIStyle style = new GUIStyle();
                        style.normal.textColor = Color.red;
                        style.wordWrap = true;
                        style.alignment = TextAnchor.MiddleCenter;
                        style.fontStyle = FontStyle.Bold;
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Negative Ailment", GUILayout.Width(100));
                        creatureScriptableObject.startingAbilities[i].negativeAilment = (NegativeAilment)EditorGUILayout.EnumPopup(creatureScriptableObject.startingAbilities[i].negativeAilment, style, GUILayout.Width(100), GUILayout.Height(20));
                        texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                        texture.SetPixel(0, 0, new Color32(0, 0, 0, 45));
                        texture.Apply();
                        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
                        GUILayout.EndHorizontal();
                    }
                    //Remainging Count

                    //Max Count
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Max Count", EditorStyles.wordWrappedLabel, GUILayout.Width(100));
                    creatureScriptableObject.startingAbilities[i].abilityStats.maxCount = EditorGUILayout.IntField(creatureScriptableObject.startingAbilities[i].abilityStats.maxCount, GUILayout.Width(50));
                    GUILayout.EndHorizontal();
                    //Power
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Power", EditorStyles.wordWrappedLabel, GUILayout.Width(100));
                    creatureScriptableObject.startingAbilities[i].abilityStats.power = EditorGUILayout.IntField(creatureScriptableObject.startingAbilities[i].abilityStats.power, GUILayout.Width(50));
                    GUILayout.EndHorizontal();
                    //Accuracy
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Accuracy", EditorStyles.wordWrappedLabel, GUILayout.Width(100));
                    creatureScriptableObject.startingAbilities[i].abilityStats.accuracy = EditorGUILayout.IntField(creatureScriptableObject.startingAbilities[i].abilityStats.accuracy, GUILayout.Width(50));
                    GUILayout.EndHorizontal();
                    //Percentage
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Percentage", EditorStyles.wordWrappedLabel);
                    creatureScriptableObject.startingAbilities[i].abilityStats.percentage = EditorGUILayout.Slider((int)creatureScriptableObject.startingAbilities[i].abilityStats.percentage, 0, 100);
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();

                //

                //Base Stats


            }
            GUILayout.Space(20);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Base Stats", EditorStyles.boldLabel, GUILayout.Width(75));
            int total = CalculateStatsTotal(creatureScriptableObject);
            creatureScriptableObject.baseStats.TotalSpend = EditorGUILayout.IntField(total, GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.Label("Average Priority", EditorStyles.boldLabel, GUILayout.Width(125));
            string averagePriority = AveragePriority(creatureScriptableObject);
            EditorGUILayout.TextField(averagePriority, GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            float step = 0.1f;
            GUILayout.Label("Stats Multiplier", EditorStyles.boldLabel, GUILayout.Width(75));
            creatureScriptableObject.StatMultiplier = Mathf.Round(EditorGUILayout.Slider(creatureScriptableObject.StatMultiplier, 1, 1.2f) * 100f) / 100;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("MAX HP", EditorStyles.boldLabel, GUILayout.Width(75));
            creatureScriptableObject.baseStats.maxHP.stat = EditorGUILayout.IntSlider(creatureScriptableObject.baseStats.maxHP.stat, 1, 11);
            creatureScriptableObject.baseStats.maxHP.prioity = (StatPrioity)EditorGUILayout.EnumPopup(creatureScriptableObject.baseStats.maxHP.prioity, GUILayout.Width(75), GUILayout.Height(20));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("STR", EditorStyles.boldLabel, GUILayout.Width(75));
            creatureScriptableObject.baseStats.strength.stat = EditorGUILayout.IntSlider(creatureScriptableObject.baseStats.strength.stat, 1, 11);
            creatureScriptableObject.baseStats.strength.prioity = (StatPrioity)EditorGUILayout.EnumPopup(creatureScriptableObject.baseStats.strength.prioity, GUILayout.Width(75), GUILayout.Height(20));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("DEF", EditorStyles.boldLabel, GUILayout.Width(75));
            creatureScriptableObject.baseStats.defence.stat = EditorGUILayout.IntSlider(creatureScriptableObject.baseStats.defence.stat, 1, 11);
            creatureScriptableObject.baseStats.defence.prioity = (StatPrioity)EditorGUILayout.EnumPopup(creatureScriptableObject.baseStats.defence.prioity, GUILayout.Width(75), GUILayout.Height(20));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("SPD", EditorStyles.boldLabel, GUILayout.Width(75));
            creatureScriptableObject.baseStats.speed.stat = EditorGUILayout.IntSlider(creatureScriptableObject.baseStats.speed.stat, 1, 11);
            creatureScriptableObject.baseStats.speed.prioity = (StatPrioity)EditorGUILayout.EnumPopup(creatureScriptableObject.baseStats.speed.prioity, GUILayout.Width(75), GUILayout.Height(20));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Crit ATK", EditorStyles.boldLabel, GUILayout.Width(75));
            creatureScriptableObject.baseStats.criticalAttack.stat = EditorGUILayout.IntSlider(creatureScriptableObject.baseStats.criticalAttack.stat, 1, 11);
            creatureScriptableObject.baseStats.criticalAttack.prioity = (StatPrioity)EditorGUILayout.EnumPopup(creatureScriptableObject.baseStats.criticalAttack.prioity, GUILayout.Width(75), GUILayout.Height(20));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Crit DEF", EditorStyles.boldLabel, GUILayout.Width(75));
            creatureScriptableObject.baseStats.criticalDefence.stat = EditorGUILayout.IntSlider(creatureScriptableObject.baseStats.criticalDefence.stat, 1, 11);
            creatureScriptableObject.baseStats.criticalDefence.prioity = (StatPrioity)EditorGUILayout.EnumPopup(creatureScriptableObject.baseStats.criticalDefence.prioity, GUILayout.Width(75), GUILayout.Height(20));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Copy Stats", GUILayout.Width(100));
            if (copyCreatureDetails != null)
            {
                GUILayout.Label(copyCreatureDetails.creatureName, GUILayout.Width(100));
            }
            copyCreatureDetails = (CreatureSO)EditorGUILayout.ObjectField(copyCreatureDetails, typeof(CreatureSO), false);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Create Creature"))
            {
                CreatureSO newCreature = CreateInstance<CreatureSO>();
                AssetDatabase.CreateAsset(newCreature, "Assets/GameElements/Creature/C.0" + (creatureTable.Creatures.Count + 1) + ".asset");
                creatureTable.Creatures.Add(newCreature);
                AssetDatabase.SaveAssets();
                creatureScriptableObject = newCreature;
            }
            if (GUILayout.Button("Copy CreatureStats"))
            {
                if (copyCreatureDetails != null)
                {

                    CreatureSO copy = copyCreatureDetails;
                    creatureScriptableObject.width = copy.width;
                    creatureScriptableObject.height = copy.height;
                    creatureScriptableObject.evolutions = copy.evolutions;
                    creatureScriptableObject.baseStats = copy.baseStats;
                    copyCreatureDetails = null;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
    }

    private int CalculateStatsTotal(CreatureSO creatureScriptableObject)
    {
        int total = 0;

        total += creatureScriptableObject.baseStats.maxHP.stat;
        total += creatureScriptableObject.baseStats.strength.stat;
        total += creatureScriptableObject.baseStats.defence.stat;
        total += creatureScriptableObject.baseStats.speed.stat;
        total += creatureScriptableObject.baseStats.criticalAttack.stat;
        total += creatureScriptableObject.baseStats.criticalDefence.stat;

        return total;
    }
    private string AveragePriority(CreatureSO creatureScriptableObject)
    {

        string s = "";

        int low = 0;
        int normal = 0;
        int high = 0;

        CheckPriority(creatureScriptableObject.baseStats.maxHP.prioity, low, normal, high, out low, out normal, out high);
        CheckPriority(creatureScriptableObject.baseStats.strength.prioity, low, normal, high, out low, out normal, out high);
        CheckPriority(creatureScriptableObject.baseStats.defence.prioity, low, normal, high, out low, out normal, out high);
        CheckPriority(creatureScriptableObject.baseStats.speed.prioity, low, normal, high, out low, out normal, out high);
        CheckPriority(creatureScriptableObject.baseStats.criticalAttack.prioity, low, normal, high, out low, out normal, out high);
        CheckPriority(creatureScriptableObject.baseStats.criticalDefence.prioity, low, normal, high, out low, out normal, out high);

        if (low > normal && low > high)
        {
            s = "Low";
        }
        else if (normal > low && normal > high)
        {
            s = "Normal";
        }
        else if (high > low && high > normal)
        {
            s = "High";
        }
        return s;
    }
    public void CheckPriority(StatPrioity prioity, int low, int normal, int high, out int lowOut, out int normalOut, out int highOut)
    {

        if (prioity == StatPrioity.Low)
        {
            int value = low + 1;
            lowOut = value;
            normalOut = normal;
            highOut = high;
            return;
        }
        else if (prioity == StatPrioity.Normal)
        {
            int value = normal + 1;
            normalOut = value;
            lowOut = low;
            highOut = high;
            return;
        }
        else if (prioity == StatPrioity.High)
        {
            int value = high + 1;
            highOut = high + 1;
            lowOut = low;
            normalOut = normal;
            return;
        }
        lowOut = low;
        normalOut = normal;
        highOut = high;

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
