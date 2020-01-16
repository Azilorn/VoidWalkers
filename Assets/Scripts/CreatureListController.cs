using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CreatureListController : MonoBehaviour
{
    public static GameObject StaticGameObject;
    public List<CreatureListOptionUI> creatureLists;
    public Transform creatureListParent;
    public GameObject creatureListTempalate;
    public TMPro.TMP_InputField textInput;


    private void Start()
    {
        StaticGameObject = this.gameObject;
    }
    public void CreateCreatureListUIOptions() {

        textInput.text = "";
        GameObject go = Resources.Load("CreatureTable") as GameObject;
        CreatureTable creatureTable = go.GetComponent<CreatureTable>();
        for (int i = 0; i < creatureTable.Creatures.Count; i++)
        {
            if (i >= creatureLists.Count)
            {
                GameObject creatureGameObjects = Instantiate(creatureListTempalate.gameObject, creatureListParent);
                CreatureListOptionUI creatureListOptionUI = creatureGameObjects.GetComponent<CreatureListOptionUI>();
                creatureLists.Add(creatureListOptionUI);
                creatureListOptionUI.SetCreatureListOption(creatureTable.Creatures[i]);
                creatureListOptionUI.gameObject.SetActive(true);
            }
            else {
                creatureLists[i].SetCreatureListOption(creatureTable.Creatures[i]);
                creatureLists[i].gameObject.SetActive(true);
            }
            for (int j = 0; j < NewGameSelectCreatureUI.creaturesSelected.Length; j++)
            {
                if (NewGameSelectCreatureUI.creaturesSelected[j] != null)
                {
                    if (NewGameSelectCreatureUI.creaturesSelected[j] == creatureTable.Creatures[i])
                    {
                        creatureLists[i].gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    public void SearchByName(TMPro.TMP_InputField inputField)
    {
        if (inputField.text == "")
        {
            for (int i = 0; i < creatureLists.Count; i++)
                creatureLists[i].gameObject.SetActive(true);
        }
        else
        {
            List<CreatureListOptionUI> searchList = new List<CreatureListOptionUI>();

            var search = inputField.text;
            searchList = creatureLists.Where(t => t.creatureName.text.ToLower().Contains(search.ToLower())).ToList();

            for (int i = 0; i < creatureLists.Count; i++)
            {
                if (searchList.Contains(creatureLists[i]))
                {
                    if (!creatureLists[i].gameObject.activeInHierarchy)
                        creatureLists[i].gameObject.SetActive(true);
                }
                else
                {
                    if (creatureLists[i].gameObject.activeInHierarchy)
                        creatureLists[i].gameObject.SetActive(false);
                }
            }
        }
    }
    public void SearchByNumber()
    {

    }

    public void QuickSortByName()
    {
        creatureLists = creatureLists.OrderBy(o => o.creatureName.text).ToList();
        for (int i = 0; i < creatureLists.Count; i++)
        {
            creatureLists[i].transform.SetSiblingIndex(i);
        }
    }
    public void QuickSortByNumber()
    {
        creatureLists = creatureLists.OrderBy(o => o.creatureNumber.text).ToList();
        for (int i = 0; i < creatureLists.Count; i++)
        {
            creatureLists[i].transform.SetSiblingIndex(i);
        }
    }
    public void QuickSortByType()
    {

    }
    public void OnDropDownValueChange(TMPro.TMP_Dropdown dropdown) {

        if (dropdown.value == 1)
        {
            QuickSortByName();
        }
        else if (dropdown.value == 0)
        {
            QuickSortByNumber();
        }
        else if (dropdown.value == 3) {

        }
    }
    public void OpenCreatureList()
    {
        CreateCreatureListUIOptions();
        gameObject.SetActive(true);
    }
}
