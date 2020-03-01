using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOptions : MonoBehaviour, IUIMenu
{
    [SerializeField] private List<ItemMenuDetails> objectPool = new List<ItemMenuDetails>();
    [SerializeField] private GameObject itemTemplate;
    [SerializeField] private Transform itemParent;
    public static int lastItemSelectedMenu = 0;
    [SerializeField] private GameObject previousMenu;
    bool menuClosing;
    [SerializeField] private GameObject mainBody;
    [SerializeField] private GameObject bottomBar;
    [SerializeField] private GameObject header;
    [SerializeField] private GameObject itemDetailsUI;

    public GameObject MainBody { get => mainBody; set => mainBody = value; }
    public GameObject Header { get => header; set => header = value; }
    public GameObject BottomBar { get => bottomBar; set => bottomBar = value; }

   

    public void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) && this.gameObject.activeInHierarchy && !menuClosing)
        {
            StartCoroutine(OnMenuBackwardsBattle());
        }
    }
    private void OnEnable()
    {
        mainBody.transform.localScale = Vector3.zero;
        SetItemMenu(lastItemSelectedMenu);
        AudioManager.Instance.PlayUISFX(UIAudio.Instance.ItemMenuOpenAudio, 1, false);
    }
   
    public void SetItemMenu(int value) {

        lastItemSelectedMenu = value;
        SetItemDetails((ItemMasterType)value);
    }
    public void SetItemDetails(ItemMasterType itemMasterType)
    {
        ClearObjectPoolDetails();

        if (itemMasterType == ItemMasterType.Scrolls)
        {
            for (int i = 0; i < InventoryController.Instance.abilities.Count; i++)
            {
                if (!InventoryController.Instance.ownedAbilities.ContainsKey(i)) 
                    continue;
                if (objectPool.Count <= i)
                {
                    AddToPool();
                    objectPool[objectPool.Count - 1].SetAbilityDetails(InventoryController.Instance.ReturnAbility(i), InventoryController.Instance.ownedAbilities[i]);
                }
                else
                {
                    objectPool[i].SetAbilityDetails(InventoryController.Instance.ReturnAbility(i), InventoryController.Instance.ownedAbilities[i]);
                    objectPool[i].gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < InventoryController.Instance.gameItems.Count; i++)
            {
                if (!InventoryController.Instance.ownedItems.ContainsKey(i))
                    continue;
                if (InventoryController.Instance.ReturnItem(i).itemMasterType == itemMasterType)
                {
                    if (objectPool.Count <= i)
                    {
                        AddToPool();
                        objectPool[objectPool.Count - 1].SetItemDetails(InventoryController.Instance.ReturnItem(i), InventoryController.Instance.ownedItems[i], i);
                    }
                    else
                    {
                        objectPool[i].SetItemDetails(InventoryController.Instance.ReturnItem(i), InventoryController.Instance.ownedItems[i], i);
                        objectPool[i].gameObject.SetActive(true);
                    }
                }
            }
            itemParent.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }
    }

    private void ClearObjectPoolDetails()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            objectPool[i].gameObject.SetActive(false);
        }
    }

    public void AddToPool()
    {
        GameObject itm = Instantiate(itemTemplate, itemParent);
        objectPool.Add(itm.GetComponent<ItemMenuDetails>());
        itm.SetActive(true);
    }

    public IEnumerator OnMenuActivated()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator OnMenuDeactivated()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator OnMenuFoward()
    {
        throw new System.NotImplementedException();
    }
    public void OnMenuBackwards(bool menuBackwards)
    {
        if (menuClosing == false)
            StartCoroutine(OnMenuBackwardsBattle());
    }
    public IEnumerator OnMenuBackwardsBattle()
    {
        menuClosing = true;
        AudioManager.Instance.PlayUISFX(UIAudio.Instance.ItemMenuCloseAudio, 1, false);
        CoreUI.DoFadeOut(mainBody, 0.15f);
        CoreUI.DoFadeOut(gameObject, 0.35f);
        mainBody.transform.DOScale(Vector3.zero, 0.25f);
        if (CoreUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy)
        {
            previousMenu.SetActive(true);
            CoreUI.DoFadeIn(previousMenu, 0.35f);
            previousMenu.transform.DOScale(Vector3.one, 0.25f);
        }
        yield return new WaitForSeconds(0.25f);
        gameObject.SetActive(false);
        menuClosing = false;
    }
}
