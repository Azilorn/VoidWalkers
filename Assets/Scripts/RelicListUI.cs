using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicListUI : MonoBehaviour
{
    public static GameObject StaticGameObject;
    public List<RelicSO> startingRelics = new List<RelicSO>();
    public List<RelicMenuDetails> objectPool = new List<RelicMenuDetails>();
    public Transform poolParent;
    public GameObject template;

    private void Start()
    {
        StaticGameObject = this.gameObject;
    }
    private void OnEnable()
    {
        SetRelicList();
    }
    public void SetRelicList()
    {
        for (int i = 0; i < startingRelics.Count; i++)
        {
            if (objectPool.Count <= i)
            {
                GameObject go = Instantiate(template, poolParent) as GameObject;
                RelicMenuDetails relicMenuDetails = go.GetComponent<RelicMenuDetails>();
                objectPool.Add(relicMenuDetails);
                relicMenuDetails.SetDetail(startingRelics[i]);
                go.SetActive(true);
            }
            else {
                RelicMenuDetails relicMenuDetails = objectPool[i].GetComponent<RelicMenuDetails>();
                relicMenuDetails.SetDetail(startingRelics[i]);
                objectPool[i].gameObject.SetActive(true);
            }
        }
    }
}
