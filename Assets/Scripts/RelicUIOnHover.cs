using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RelicUIOnHover : MonoBehaviour, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private RelicSO relic;
    [SerializeField] private Image image;

    public Image Image { get => image; set => image = value; }
    public RelicSO Relic { get => relic; set => relic = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        HoverTooltip.Instance.OpenUI(600, 150, Relic.relicDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverTooltip.Instance.CloseUI();
    }

    public void SetUI(RelicSO r) {
        Relic = r;
        image.sprite = relic.icon;
    }
}
