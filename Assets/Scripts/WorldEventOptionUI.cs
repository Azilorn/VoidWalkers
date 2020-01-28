using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using DG.Tweening;

public class WorldEventOptionUI : MonoBehaviour
{

   [SerializeField][ResizableTextArea] private string eventOptionString;
   [SerializeField] private TextMeshProUGUI eventOptionText;
   

    public string EventOptionString { get => eventOptionString; set => eventOptionString = value; }
    public TextMeshProUGUI EventOptionText { get => eventOptionText; set => eventOptionText = value; }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.35f);
    }

    public void SetOptionActive() {
        eventOptionText.text = eventOptionString;
        gameObject.SetActive(true);
    }
}
