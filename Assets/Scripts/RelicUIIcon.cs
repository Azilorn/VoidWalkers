using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RelicUIIcon : MonoBehaviour
{
    public static RelicUIIcon Instance;
    public Image img;
    public CanvasGroup canvasGroup;

    public void SetDeactive(float duration)
    {
        StartCoroutine(SetDeactiveCoroutine(duration));
    }

    public IEnumerator SetDeactiveCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration - 0.2f);
        canvasGroup.DOFade(0, 0.2f);
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

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
        if (img == null) {
            img = GetComponentInChildren<Image>();
        }
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        canvasGroup.DOFade(1, 0.45f);
        transform.DOScale(1, 0.45f);
    }
    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0;
    }

    

}
