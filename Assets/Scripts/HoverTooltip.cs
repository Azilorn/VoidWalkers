using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HoverTooltip : MonoBehaviour
{
    public static HoverTooltip Instance;

    public RectTransform rectTransform;
    public GameObject toolTip;
    public TextMeshProUGUI tooltipText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetUI(int width, int height, string text)
    {
        tooltipText.text = text;
        rectTransform.sizeDelta = new Vector2(width, height);
    }

    public void OpenUI(int width, int height, string text)
    {
        SetUI(width, height, text);
        toolTip.SetActive(true);

        if (Input.mousePosition.x < Camera.main.pixelWidth / 2)
        {
            rectTransform.position = Input.mousePosition + new Vector3(Camera.main.scaledPixelWidth * 0.2f, -Camera.main.scaledPixelHeight * 0.05f);
        }
        else {
            rectTransform.position = Input.mousePosition - new Vector3(Camera.main.scaledPixelWidth * 0.2f, Camera.main.scaledPixelHeight * 0.05f);
        }
    }
    public void CloseUI()
    {
        toolTip.SetActive(false);
    }
}
