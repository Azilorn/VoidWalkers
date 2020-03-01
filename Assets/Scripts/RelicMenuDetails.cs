using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RelicMenuDetails : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public RelicSO relic;
    public Image relicIcon;
    public Image relicBackground;
    public Color usableColor;
    public Color unusableColor;
    public TextMeshProUGUI relicName;
    public TextMeshProUGUI relicDescription;

    private float holdTimer;
    private float holdDurationRequired = 0.35f;
    private bool buttonClicked;
    private bool buttonHeld;
    private bool dragging = false;
    [SerializeField] private ScrollRect scrollRect;


    public void SetDetail(RelicSO r)
    {
        relic = r;
        relicIcon.sprite = r.icon;
        relicName.text = r.relicName;
        relicDescription.text = r.relicDescription;
        if (relic.relicUseable == RelicUseable.Yes)
            relicBackground.color = usableColor;
        else relicBackground.color = unusableColor;
    }
    public void LateUpdate()
    {
        if (dragging)
            return;
        
        if (buttonHeld)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer > holdDurationRequired)
            {
              
                buttonHeld = false;
                buttonClicked = false;
                return;
            }
        }
        else if (!buttonHeld)
        {
            if (buttonClicked)
            {
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    NewGameSelectCreatureUI.startingRelic = relic;
                    MenuTransitionsController.Instance.StartTransition(0, true);
                    StartCoroutine(NewGameSelectCreatureUI.Instance.artefactUI.SetArtefactUI(0.3f, relic));
                    buttonClicked = false;
                }
                else
                {
                    if (relic.relicUseable == RelicUseable.Yes)
                        StartCoroutine(CoreUI.Instance.UseRelicEvent(relic.relicNameID, false));
                    else if (relic.relicUseable == RelicUseable.No)
                    {
                        UIAudio.Instance.PlayDenyAudio();
                    }
                    buttonClicked = false;
                }
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (dragging)
            return;
       
        holdTimer = 0;
        buttonHeld = true;
        buttonClicked = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (dragging)
            return;
        if (!buttonHeld)
            return;
        if (!buttonClicked)
            return;
        if (holdTimer < holdDurationRequired)
        {
            buttonClicked = true;
            buttonHeld = false;
        }
        else if (holdTimer > holdDurationRequired)
        {
            buttonClicked = false;
            buttonHeld = false;
            holdTimer = 0;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        dragging = true;
        buttonClicked = false;
        buttonHeld = false;
        holdTimer = 0;
        scrollRect.OnBeginDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        buttonClicked = false;
        buttonHeld = false;
        holdTimer = 0;
        scrollRect.OnEndDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragging = true;
        buttonClicked = false;
        buttonHeld = false;
        holdTimer = 0;
        scrollRect.OnDrag(eventData);
    }

}
