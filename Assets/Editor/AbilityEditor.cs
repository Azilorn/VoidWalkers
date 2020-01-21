using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AbilityEditor : EditorWindow
{
    string windowTitle = "Ability";
    public static EditorWindow window;
    public static Ability ability;
    Vector2 scrollPos;
    public static bool windowOpen;
    //List<Rect>

    [MenuItem("GameElements/Ability %&h")]
    public static void ShowWindow()
    {
        window = GetWindow(typeof(AbilityEditor));
        window.minSize = new Vector2(700, 1015);
        window.maxSize = new Vector2(700, 1015);
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

        if (ability == null)
            return;
        if (ability != null)
        {
            EditorUtility.SetDirty(ability);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Ability", EditorStyles.boldLabel);
        if (ability == null)
        {
            ability = (Ability)EditorGUILayout.ObjectField(ability, typeof(Ability), false);
            return;
        }
        else ability = (Ability)EditorGUILayout.ObjectField(ability, typeof(Ability), false);

        GUILayout.BeginVertical();
        ability.abilityName = GUILayout.TextField(ability.abilityName, GUILayout.Width(200));
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Label("Bio", EditorStyles.boldLabel);
        ability.abilityBio = GUILayout.TextArea(ability.abilityBio, GUILayout.Width(400), GUILayout.Height(100));
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.Label("Ability Type", EditorStyles.boldLabel);
        ability.type = (AbilityType)EditorGUILayout.EnumPopup(ability.type, GUILayout.Width(100));
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Label("Element Type", EditorStyles.boldLabel);
        ability.elementType = (ElementType)EditorGUILayout.EnumPopup(ability.elementType, GUILayout.Width(100));
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Label("Positive Ailment", EditorStyles.boldLabel);
        ability.positiveAilment = (PositiveAilment)EditorGUILayout.EnumPopup(ability.positiveAilment, GUILayout.Width(100));
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Label("Negative Ailment", EditorStyles.boldLabel);
        ability.negativeAilment = (NegativeAilment)EditorGUILayout.EnumPopup(ability.negativeAilment, GUILayout.Width(100));
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.Label("Max Count", EditorStyles.boldLabel);
        ability.abilityStats.maxCount = EditorGUILayout.IntField(ability.abilityStats.maxCount);
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Label("Power", EditorStyles.boldLabel);
        ability.abilityStats.power = EditorGUILayout.IntField(ability.abilityStats.power);
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Label("Accuracy", EditorStyles.boldLabel);
        ability.abilityStats.accuracy = EditorGUILayout.IntField(ability.abilityStats.accuracy);
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Label("Percentage", EditorStyles.boldLabel);
        ability.abilityStats.percentage = EditorGUILayout.FloatField(ability.abilityStats.percentage);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

       
        GUILayout.Label("Animations", EditorStyles.boldLabel);


        if(ability.animations.Count == 0)
        {
            ability.animations.Add(new AnimationDetail());
        }

        for (int i = 0; i < ability.animations.Count; i++) {

            if (i > 0)
                if ((i % 2) == 0)
                    EditorGUI.DrawRect(GUILayoutUtility.GetLastRect(), new Color32(125, 125, 125, 100));
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Animation", EditorStyles.boldLabel, GUILayout.Width(100));
            ability.animations[i].animation = (ImageAnimation)EditorGUILayout.EnumPopup(ability.animations[i].animation, GUILayout.Width(100));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label("Target Type", EditorStyles.boldLabel, GUILayout.Width(100));
            ability.animations[i].targetType = (TargetType)EditorGUILayout.EnumPopup(ability.animations[i].targetType, GUILayout.Width(100));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label("Animation Location", EditorStyles.boldLabel, GUILayout.Width(100));
            ability.animations[i].animationLocation = (AnimationLocation)EditorGUILayout.EnumPopup(ability.animations[i].animationLocation, GUILayout.Width(100));
            GUILayout.EndVertical();
            if (ability.animations[i].animation == ImageAnimation.SpawnAnimSprite)
            {
                GUILayout.BeginVertical();
                GUILayout.Label("Sprite", EditorStyles.boldLabel, GUILayout.Width(100));
                ability.animations[i].animSprite = (GameObject)EditorGUILayout.ObjectField(ability.animations[i].animSprite, typeof(GameObject), false, GUILayout.Width(100));
                GUILayout.EndVertical();
            }
            if (ability.animations[i].animation != ImageAnimation.SpawnAnimSprite)
            {
                GUILayout.BeginVertical();
                GUILayout.Label("Duration", EditorStyles.boldLabel, GUILayout.Width(100));
                ability.animations[i].duration = EditorGUILayout.FloatField(ability.animations[i].duration, GUILayout.Width(100));
                GUILayout.EndVertical();
            }
            GUILayout.BeginVertical();
            GUILayout.Label("Delay", EditorStyles.boldLabel, GUILayout.Width(100));
            ability.animations[i].delay = EditorGUILayout.FloatField(ability.animations[i].delay, GUILayout.Width(100));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label("Skip Coroutine Wait", EditorStyles.boldLabel, GUILayout.Width(100));
            ability.animations[i].SkipCoroutineWait = EditorGUILayout.Toggle(ability.animations[i].SkipCoroutineWait, GUILayout.Width(20));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (GUILayout.Button("↑", GUILayout.Width(20)))
            {
                AnimationDetail oldAnim = ability.animations[i];
                AnimationDetail newAnim;
                if (i == 0)
                {
                    newAnim = ability.animations[ability.animations.Count - 1];
                    ability.animations[ability.animations.Count - 1] = oldAnim;
                    ability.animations[0] = newAnim;
                    break;
                }
                else
                {
                    newAnim = ability.animations[i - 1];
                    ability.animations[i - 1] = oldAnim;
                    ability.animations[i] = newAnim;
                    break;
                }
            }
            else if (GUILayout.Button("↓", GUILayout.Width(20)))
            {
                AnimationDetail oldAnim = ability.animations[i];
                AnimationDetail newAnim;
                if (i == ability.animations.Count - 1)
                {
                    newAnim = ability.animations[0];
                    ability.animations[0] = oldAnim;
                    ability.animations[ability.animations.Count - 1] = newAnim;
                    break;
                }
                else
                {
                    newAnim = ability.animations[i + 1];
                    ability.animations[i + 1] = oldAnim;
                    ability.animations[i] = newAnim;
                    break;
                }
            }else if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                ability.animations.RemoveAt(i);
            }
            else if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                ability.animations.Insert(i, new AnimationDetail());
            }
           
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
    }
}
