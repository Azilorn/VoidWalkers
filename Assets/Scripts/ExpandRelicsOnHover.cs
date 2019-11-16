using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ExpandRelicsOnHover : MonoBehaviour, IPointerClickHandler
{
    public Transform relicParent;
    public List<RelicUIOnHover> relics = new List<RelicUIOnHover>();
    private Vector3 startPos;
    private Vector3 currentPos;
    
    [SerializeField] bool menuOpen;

    public void OnPointerClick(PointerEventData eventData)
    {
            if (menuOpen == false)
            {
                    relicParent.DOLocalMoveX(100, 0.35f);
                    SetRelics();
                    menuOpen = true;
            }
            else if (menuOpen == true)
            {
      
                    relicParent.DOLocalMoveX(980, 0.35f);
                    CloseMenu();
                    menuOpen = false;
            }
    }

    public void SetRelics() {

        int relicsCount = 0;
        for (int i = 0; i < InventoryController.Instance.ownedRelics.Count; i++)
        {
            if (InventoryController.Instance.ownedRelics[i] == true)
            {
                relics[relicsCount].SetUI(InventoryController.Instance.ReturnRelic(i));
                relics[relicsCount].gameObject.SetActive(true);
                relicsCount++;
            }
        }
    }
   
    private void CloseMenu()
    {
        foreach (RelicUIOnHover r in relics)
        {
            if (r.transform.GetSiblingIndex() > 6)
            {
                r.gameObject.SetActive(false);
            }
        }
    }

}
