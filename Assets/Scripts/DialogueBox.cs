using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private Transform container;
   [SerializeField] private TextMeshProUGUI dialogue;

    public TextMeshProUGUI Dialogue { get => dialogue; set => dialogue = value; }
    public Transform Container { get => container; set => container = value; }
}
