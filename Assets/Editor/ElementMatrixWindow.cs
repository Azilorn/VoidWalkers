using UnityEditor;
using UnityEngine;

public class ElementMatrixWindow : EditorWindow
{

    string windowTitle = "Element Matrix Editor";
    static EditorWindow window;
    public GameObject elementMatrixPrefab;
    ElementMatrix elementMatrix;
    Vector2 scrollPos = Vector2.zero;
    [MenuItem("GameElements/ElementMatrix %&e")]
    public static void ShowWindow()
    {
        window = GetWindow(typeof(ElementMatrixWindow));
    }
    private void OnDisable()
    {
        if (elementMatrix != null)
        {
            EditorUtility.SetDirty(elementMatrix);
        }
    }
    private void OnInspectorUpdate()
    {
        Repaint();
    }
    private void OnGUI()
    {

        GUILayout.Label(windowTitle, EditorStyles.boldLabel);
        GUILayout.Label("Element Matrix", EditorStyles.label);
        elementMatrixPrefab = (GameObject)EditorGUILayout.ObjectField(elementMatrixPrefab, typeof(GameObject), false);
        GUILayout.Label("Top is what you use, side is what effect it has against", EditorStyles.boldLabel);
        if (elementMatrixPrefab != null)
        {
            elementMatrix = elementMatrixPrefab.GetComponent<ElementMatrix>();
            if (elementMatrix == null)
            {
                return;
            }
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            GUILayout.BeginVertical();

            for (int i = 0; i < elementMatrix.elements.Count; i++)
            {
                GUIStyle elementTitle = new GUIStyle();
                elementTitle.normal.textColor = ReturnLabelColor(i);
                elementTitle.alignment = TextAnchor.MiddleCenter;
                elementTitle.fontStyle = FontStyle.Bold;
                elementTitle.fontSize = 11;
                GUILayout.BeginHorizontal();
                  
                GUILayout.Label(elementMatrix.elements[i].elementType.ToString(), elementTitle, GUILayout.Width(50));
                if (i == 0)
                {
                    EditorGUILayout.LabelField("", GUI.skin.verticalSlider, GUILayout.Width(10), GUILayout.Height(30));
                }
                else EditorGUILayout.LabelField("", GUI.skin.verticalSlider, GUILayout.Width(10), GUILayout.Height(25));
                for (int k = 0; k < elementMatrix.elements[i].elementList.Count; k++)
                { 
                  
                   
                    GUILayout.BeginVertical();
                    if (i == 0)
                    {
                        GUIStyle elementTitle2 = new GUIStyle();
                        elementTitle2.normal.textColor = ReturnLabelColor(k);
                        elementTitle2.alignment = TextAnchor.MiddleCenter;
                        elementTitle2.fontStyle = FontStyle.Bold;
                        elementTitle2.fontSize = 11;
                        Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                        GUILayout.Label(elementMatrix.elements[k].elementType.ToString(), elementTitle2, GUILayout.Width(50), GUILayout.Height(20));
                        texture.SetPixel(0, 0, new Color32(0, 0, 0, 25));
                        texture.Apply();
                        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
                        GUILayout.Space(5f);

                    }
                    // Rect lastrect = GUILayoutUtility.GetRect(100, 20, GUIStyle.none, GUILayout.Width(100), GUILayout.Height(20));
                    GUIStyle style3 = new GUIStyle();
                    style3.normal.textColor = ReturnImpactLabelColor(elementMatrix.elements[i].elementList[k].elementImpactType);
                    style3.fontSize = 11; 
                    elementMatrix.elements[i].elementList[k].elementType = ReturnElementType(k);
                    elementMatrix.elements[i].elementList[k].elementImpactType = (ElementImpactType)EditorGUILayout.EnumPopup(elementMatrix.elements[i].elementList[k].elementImpactType, style3, GUILayout.Width(50), GUILayout.Height(20));

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.Width(50));
                    GUILayout.EndVertical();
                    if(i == 0)
                        EditorGUILayout.LabelField("", GUI.skin.verticalSlider, GUILayout.Width(10), GUILayout.Height(50));
                    else EditorGUILayout.LabelField("", GUI.skin.verticalSlider, GUILayout.Width(10), GUILayout.Height(25));
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            if (GUILayout.Button("SaveMatrix"))
            {
                if (elementMatrix != null)
                {
                    EditorUtility.SetDirty(elementMatrix);
                }
            }
        }
    }

    private ElementType ReturnElementType(int k)
    {
        //ELEMENT must update if more added
        switch (k)
        {
            case 0:
                return ElementType.Normal;
            case 1:
                return ElementType.Fire;
            case 2:
                return ElementType.Water;
            case 3:
                return ElementType.Nature;
            case 4:
                return ElementType.Electric;
            case 5:
                return ElementType.Spectre;
            case 6:
                return ElementType.Fighting;
            case 7:
                return ElementType.Ice;
            case 8:
                return ElementType.Wind;
            case 9:
                return ElementType.Earth;
            case 10:
                return ElementType.Metal;
            case 11:
                return ElementType.Insect;
            case 12:
                return ElementType.Unholy;
            case 13:
                return ElementType.Holy;
            case 14:
                return ElementType.Ancient;

            default:
                return ElementType.None;
        }
    }
    private Color ReturnImpactLabelColor(ElementImpactType elementImpactType)
    {
        switch (elementImpactType)
        {
            case ElementImpactType.NotEffective:
                return Color.grey;
            case ElementImpactType.VeryWeak:
                return Color.red;
            case ElementImpactType.Weak:
                return new Color(255, 119, 0);
            case ElementImpactType.Normal:
                return Color.black;
            case ElementImpactType.Crit:
                return new Color(0, 255, 162);
            case ElementImpactType.MegaCrit:
                return Color.green;
            default:
                return Color.black;
        }
    }
    //ELEMENT must update if more added
    private string ReturnLabel(int index)
    {

        string s = "";

        switch (index)
        {
            case 0:
                return "Normal";
            case 1:
                return "Fire";
            case 2:
                return "Water";
            case 3:
                return "Nature";
            case 4:
                return "Electric";
            case 5:
                return "Spectre";
            case 6:
                return "Fighting";
            case 7:
                return "Ice";
            case 8:
                return "Wind";
            case 9:
                return "Rock";
            case 10:
                return "Metal";
            case 11:
                return "Insect";
            case 12:
                return "Unholy";
            case 13:
                return "Holy";
            case 14:
                return "Ancient";
            default:
                break;
        }

        return s;
    }
    private Color ReturnLabelColor(int index)
    {
        switch (index)
        {
            case 0:
                return new Color(0, 0, 0);
            case 1:
                return new Color(255, 0, 0);
            case 2:
                return new Color(0, 0, 255);
            case 3:
                return new Color(0, 255, 0);
            case 4:
                return new Color(255, 255, 0);
            case 5:
                return new Color(100, 0, 255);
            case 6:
                return new Color(70, 45, 45);
            case 7:
                return new Color(0, 255, 255);
            case 8:
                return new Color(137, 255, 0);
            case 9:
                return new Color(60, 15, 15);
            case 10:
                return new Color(130, 130, 130);
            case 11:
                return new Color(130, 130, 130);
            case 12:
                return new Color(130, 130, 130);
            case 13:
                return new Color(130, 130, 130);
            case 14:
                return new Color(130, 130, 130);
            default:
                break;
        }

        return Color.black;
    }
}