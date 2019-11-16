using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CanvasSwipeHandler : MonoBehaviour, IDragHandler, IBeginDragHandler,IEndDragHandler
{

    Vector2 startPos;
    Vector2 currentPos;
    [SerializeField] float timer = 0f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = Input.mousePosition;
    }
    public void OnDrag(PointerEventData eventData)
    {

        if (WorldMenuUI.Instance.PartyOptions.gameObject.activeInHierarchy)
            return;
        timer += Time.deltaTime;
        if (timer > 1) {
            timer = 0;
            return;
        }
        currentPos = Input.mousePosition;
        if (currentPos.x < startPos.x - 100) {
            if (timer < 1)
            {
                WorldMenuUI.Instance.OpenAndSetParty();
                return;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        timer = 0;
    }

  
}
