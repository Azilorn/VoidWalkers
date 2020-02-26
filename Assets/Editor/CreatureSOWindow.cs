using System.IO;
using UnityEditor;
using UnityEngine;

public class CreatureSOWindow : EditorWindow
{
    string windowTitle = "Creature Editor";
    public static CreatureSO creatureScriptableObject;
    public static EditorWindow window;
    CreatureSO copyCreatureDetails;
    CreatureTable creatureTable;
    Vector2 scrollPos;
    bool showPosition = false;
    bool showPosition1 = false;
    bool showPosition2 = false;
    public static bool windowOpen;
    public static bool GettingAbility;
    public static bool GettingLearnedAbility;
    public static int GettingAbilityIndex;
    public static int GettingLearnedAbilityIndex;

    [MenuItem("GameElements/CreatureSO %&v")]
    public static void ShowWindow()
    {
        window = GetWindow(typeof(CreatureSOWindow));
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
            EditorStyles.textField.wordWrap = true;
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
                        if (GUILayout.Button("Select", style, GUILayout.Width(20), GUILayout.Height(20)))
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
                        else if (GUILayout.Button("Select"))
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
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.Width(200));
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }

            showPosition1 = EditorGUI.Foldout(GUILayoutUtility.GetRect(200, 20), showPosition1, "Learnable Abilities");

            if (showPosition1) {

                if (creatureScriptableObject.learnableAbility.Count == 0)
                {
                    creatureScriptableObject.learnableAbility = new System.Collections.Generic.List<LearnableAbility>();
                    creatureScriptableObject.learnableAbility.Add(new LearnableAbility());
                }
                GUILayout.BeginHorizontal();
                for (int i = 0; i < creatureScriptableObject.learnableAbility.Count; i++)
                {
                    GUILayout.BeginVertical();

                    if (creatureScriptableObject.learnableAbility.Count < 1)
                    {
                        creatureScriptableObject.learnableAbility.Add(new LearnableAbility());
                        creatureScriptableObject.learnableAbility[0] = null;
                    }

                    if (creatureScriptableObject.learnableAbility[i] == null)
                    {
                        GUILayout.Label("Empty Skill", EditorStyles.boldLabel);
                        creatureScriptableObject.learnableAbility[i].abilityToLearn = (Ability)EditorGUILayout.ObjectField(creatureScriptableObject.learnableAbility[i].abilityToLearn, typeof(Ability), false, GUILayout.Width(150));
                        creatureScriptableObject.learnableAbility[i].levelToLearn = EditorGUILayout.IntField(creatureScriptableObject.learnableAbility[i].levelToLearn, GUILayout.Width(150));
                        GUILayout.EndVertical();
                        if (GUILayout.Button("Select"))
                        {
                            GettingAbility = true;
                            GettingAbilityIndex = 0;
                            if (AbilityList.windowOpen)
                                AbilityList.window.Focus();
                            else
                            {
                                AbilityList.ShowWindow();
                                AbilityList.window.Focus();
                            }
                            break;
                        }
                        continue;
                    }
                    else
                    {

                        GUILayout.BeginHorizontal();
                        if (creatureScriptableObject.learnableAbility[i].abilityToLearn != null)
                            GUILayout.Label(creatureScriptableObject.learnableAbility[i].abilityToLearn.abilityName, EditorStyles.boldLabel);
                        else GUILayout.Label("No Ability", EditorStyles.boldLabel);
                        if (GUILayout.Button("-"))
                        {
                            creatureScriptableObject.learnableAbility.RemoveAt(i);
                            break;
                        }
                        else if (GUILayout.Button("+"))
                        {
                            if (creatureScriptableObject.learnableAbility.Count < 4)
                                creatureScriptableObject.learnableAbility.Insert(i + 1, new LearnableAbility());
                            break;
                        }
                        else if (GUILayout.Button("Select"))
                        {
                            GettingLearnedAbility = true;
                            GettingLearnedAbilityIndex = i;
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
                        if (creatureScriptableObject.learnableAbility.Count < 1)
                        {
                            break;
                        }

                        creatureScriptableObject.learnableAbility[i].abilityToLearn = (Ability)EditorGUILayout.ObjectField(creatureScriptableObject.learnableAbility[i].abilityToLearn, typeof(Ability), false, GUILayout.Width(150));
                        creatureScriptableObject.learnableAbility[i].levelToLearn = EditorGUILayout.IntSlider(creatureScriptableObject.learnableAbility[i].levelToLearn, 1, 50);
                    }

                    if (creatureScriptableObject.learnableAbility[i] == null)
                    {
                        return;
                    }
                  
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }

            showPosition2 = EditorGUI.Foldout(GUILayoutUtility.GetRect(100, 20), showPosition2, "Illegal Abilities");

            if (showPosition2)
            {
                if (creatureScriptableObject.illegalAbilities.Count == 0)
                {
                    creatureScriptableObject.illegalAbilities = new System.Collections.Generic.List<Ability>();
                    creatureScriptableObject.illegalAbilities.Add(new Ability());
                }
                GUILayout.BeginHorizontal();
                for (int i = 0; i < creatureScriptableObject.illegalAbilities.Count; i++)
                {
                    GUILayout.BeginVertical();

                    if (creatureScriptableObject.illegalAbilities.Count < 1)
                    {
                        creatureScriptableObject.illegalAbilities.Add(new Ability());
                        creatureScriptableObject.illegalAbilities[0] = null;
                    }

                    if (creatureScriptableObject.illegalAbilities[i] == null)
                    {
                        GUILayout.Label("Empty Skill", EditorStyles.boldLabel);
                        creatureScriptableObject.illegalAbilities[i] = (Ability)EditorGUILayout.ObjectField(creatureScriptableObject.illegalAbilities[i], typeof(Ability), false, GUILayout.Width(150));
                        GUILayout.EndVertical();
                        if (GUILayout.Button("Select"))
                        {
                            GettingAbility = true;
                            GettingAbilityIndex = 0;
                            if (AbilityList.windowOpen)
                                AbilityList.window.Focus();
                            else
                            {
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
                        GUILayout.Label(creatureScriptableObject.illegalAbilities[i].abilityName, EditorStyles.boldLabel);
                        if (GUILayout.Button("-"))
                        {
                            creatureScriptableObject.illegalAbilities.RemoveAt(i);
                            break;
                        }
                        else if (GUILayout.Button("+"))
                        {
                            if (creatureScriptableObject.illegalAbilities.Count < 4)
                                creatureScriptableObject.illegalAbilities.Insert(i + 1, new Ability());
                            break;
                        }
                        else if (GUILayout.Button("Select"))
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
                        if (creatureScriptableObject.illegalAbilities.Count < 1)
                        {
                            break;
                        }

                        creatureScriptableObject.illegalAbilities[i] = (Ability)EditorGUILayout.ObjectField(creatureScriptableObject.illegalAbilities[i], typeof(Ability), false, GUILayout.Width(150));
                    }


                    if (creatureScriptableObject.illegalAbilities[i] == null)
                    {
                        return;
                    }
                    //Element Type
                    GUIStyle elementStyle = new GUIStyle();
                    elementStyle.normal.textColor = ReturnElementTypeColor(creatureScriptableObject.illegalAbilities[i].elementType);
                    elementStyle.wordWrap = true;
                    elementStyle.alignment = TextAnchor.MiddleCenter;
                    elementStyle.fontStyle = FontStyle.Bold;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Element Type", EditorStyles.wordWrappedLabel, GUILayout.Width(100));
                    creatureScriptableObject.illegalAbilities[i].elementType = (ElementType)EditorGUILayout.EnumPopup(creatureScriptableObject.illegalAbilities[i].elementType, elementStyle, GUILayout.Width(100), GUILayout.Height(20));
                    Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                    texture.SetPixel(0, 0, new Color32(0, 0, 0, 45));
                    texture.Apply();
                    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
                    GUILayout.EndHorizontal();
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.Width(200));
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(20);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Base Stats", EditorStyles.boldLabel, GUILayout.Width(75));
            float total = CalculateStatsTotal(creatureScriptableObject) * creatureScriptableObject.StatMultiplier;
            EditorGUILayout.FloatField(total, GUILayout.Width(100), GUILayout.Height(20));
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
