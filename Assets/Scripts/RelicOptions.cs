using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicOptions : MonoBehaviour, IUIMenu
{
    //[SerializeField] private RelicDetailsUI 
    private List<RelicMenuDetails> objectPool = new List<RelicMenuDetails>();

    [SerializeField] private GameObject relicTemplate;
    [SerializeField] private Transform relicParent;

    [SerializeField] private GameObject bottomBar;
    [SerializeField] private GameObject header;

    bool menuClosing;
    public GameObject BottomBar { get => bottomBar; set => bottomBar = value; }
    public GameObject Header { get => header; set => header = value; }
    public List<RelicMenuDetails> ObjectPool { get => objectPool; set => objectPool = value; }

    public void OnEnable()
    {
        ClearObjectPoolDetails();
        for (int i = 0; i < InventoryController.Instance.ownedRelics.Count; i++)
        {
            if (!InventoryController.Instance.ownedRelics.ContainsKey(i))
                continue;
            if (InventoryController.Instance.ownedRelics[i] == true)
            {
                if (ObjectPool.Count <= i)
                {
                    AddToPool();
                    ObjectPool[ObjectPool.Count - 1].SetDetail(InventoryController.Instance.ReturnRelic(i));
                }
                else
                {
                    ObjectPool[i].SetDetail(InventoryController.Instance.ReturnRelic(i));
                    ObjectPool[i].gameObject.SetActive(true);
                }
            }
        }
        WorldMenuUI.DoFadeIn(gameObject.gameObject, 0.3f);
        AudioManager.Instance.PlayUISFX(UIAudio.Instance.PartyMenuOpenAudio, 1, false);
    }
    public void AddToPool()
    {
        GameObject r = Instantiate(relicTemplate, relicParent);
        ObjectPool.Add(r.GetComponent<RelicMenuDetails>());
        r.SetActive(true);
    }
    private void ClearObjectPoolDetails()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            objectPool[i].gameObject.SetActive(false);
        }
    }
    public IEnumerator OnMenuActivated()
    {
        throw new System.NotImplementedException();
    }
    public void OnMenuBackwards(bool option)
    {
        if (menuClosing == false)
        {
                StartCoroutine(OnMenuBackwards());
        }
    }
    public IEnumerator OnMenuBackwards()
    {
        menuClosing = true;
        AudioManager.Instance.PlayUISFX(UIAudio.Instance.PartyMenuCloseAudio, 1, false);
        
        BattleUI.DoFadeOut(gameObject, 0.35f);
        yield return new WaitForSeconds(0.35f);
        BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
        gameObject.SetActive(false);
        menuClosing = false;

    }

    public IEnumerator OnMenuBackwardsBattle()
    {
        throw new System.NotImplementedException();
    }

    //public List<GameObject> GetGameObjects()
    //{
    //    List<GameObject> go = new List<GameObject>();

    //    for (int i = 0; i < partyCreatureUIs.Count; i++)
    //    {
    //        go.Add(partyCreatureUIs[i].gameObject);
    //    }
    //    return go;
    //}

    public IEnumerator OnMenuDeactivated()
    {
        throw new System.NotImplementedException();
    }
    public IEnumerator OnMenuFoward()
    {
        throw new System.NotImplementedException();
    }
}
