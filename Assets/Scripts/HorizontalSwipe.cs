using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HorizontalSwipe : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform content;
    Vector2 startPos;
    Vector2 currentPos;
    [SerializeField] float timer = 0;
    public static bool finishedSwipe = true;
    public static Tween tween;

    public RectTransform Content { get => content; set => content = value; }

    public void Update()
    {
        if (finishedSwipe)
        {
            ManualReset();
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentPos = Input.mousePosition;
        timer += Time.deltaTime;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (timer > 0.01f)
        {
            if (currentPos.x > startPos.x + 100)
            {

                MoveLeft();
                return;
            }
            else if (currentPos.x < startPos.x - 100)
            {
                MoveRight();
                return;
            }
        }
    }
    public void ManualReset()
    {
        if (Mathf.Abs(Content.anchoredPosition.x) > Mathf.Abs(transform.GetChild(0).GetComponent<RectTransform>().rect.width * (Content.transform.childCount - 1)))
        {
            finishedSwipe = false;
            Vector2 endPos = Vector2.zero;
            Content.DOAnchorPos(endPos, 0.15f).onComplete += SetFinishedSwipe;
            PreBattleSelectionController.Instance.selectedInt = 0;
        }
    }
    public void MoveRight()
    {
        if (finishedSwipe)
        {
            if (Mathf.Approximately(Content.anchoredPosition.x, -(transform.GetChild(0).GetComponent<RectTransform>().rect.width * (GetActiveChildCount(Content.transform) - 1))))
            {
                finishedSwipe = false;
                Vector2 endPos = Vector2.zero;
                Content.DOAnchorPos(endPos, 0.15f).onComplete += SetFinishedSwipe;
                PreBattleSelectionController.Instance.selectedInt = 0;
            }
            else
            {
                finishedSwipe = false;
                Vector2 endPos = Content.anchoredPosition - new Vector2(transform.GetChild(0).GetComponent<RectTransform>().rect.width, 0);
                Content.DOAnchorPos(endPos, 0.15f).onComplete += SetFinishedSwipe;
                PreBattleSelectionController.Instance.selectedInt++;
            }
        }

        timer = 0;
        return;
    }

    public void MoveLeft()
    {
        if (finishedSwipe)
        {
            if (Content.anchoredPosition.x >= 0 && Content.anchoredPosition.x <= 10)
            {
                finishedSwipe = false;
                Vector2 endPos = Content.anchoredPosition - new Vector2(transform.GetChild(0).GetComponent<RectTransform>().rect.width * (GetActiveChildCount(Content.transform) - 1), 0);
               Content.DOAnchorPos(endPos, 0.15f).onComplete += SetFinishedSwipe;
                PreBattleSelectionController.Instance.selectedInt = Content.transform.childCount - 1;
            }
            else
            {
                finishedSwipe = false;
                Vector2 endPos = Content.anchoredPosition + new Vector2(transform.GetChild(0).GetComponent<RectTransform>().rect.width, 0);
                Content.DOAnchorPos(endPos, 0.15f).onComplete += SetFinishedSwipe;
                PreBattleSelectionController.Instance.selectedInt--;
            }
        }

        timer = 0;
        return;
    }
    public static void SetFinishedSwipe() {
        finishedSwipe = true;
    }
    public int GetActiveChildCount(Transform t)
    {

        int i = 0;

        foreach (Transform c in t)
        {
            if (c == t)
            {
                continue;
            }

            if (c.gameObject.activeInHierarchy)
            {
                i++;
            }
        }
        return i;
    }
}
