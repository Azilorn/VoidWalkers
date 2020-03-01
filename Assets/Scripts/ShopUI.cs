using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class ShopUI : MonoBehaviour
{
    public int currentProgressOnFloor = 0;
    public static int soldItem = 0;
    public static int sellableItem = 0;
    public List<ShopItemUI> shopItemUIs = new List<ShopItemUI>();
    public static List<ShopSaveData> shopSaveData = new List<ShopSaveData>();
    public GameObject Greeting, Sold, NotEnoughMoney, Empty, NoGold, Face;
    public Sprite Face1, Face2, Face3, Face4;

    private void OnEnable()
    {
        SetShopVendorText(true, false, false, false, false);

        AudioManager.Instance.PlaySFX(UIAudio.Instance.MobsEssentialsAudio[2].AudioList[0].audio);

        if (shopSaveData.Count > 0 && shopSaveData != null)
        {
            Debug.Log("1");
            for (int i = 0; i < shopSaveData.Count; i++)
            {

                if (shopSaveData[i].ShopTypeID == 0)
                {
                    Item itm = InventoryController.Instance.gameItems[shopSaveData[i].ShopItemID];
                    shopItemUIs[i].SetItem(itm);
                }
                else if (shopSaveData[i].ShopTypeID == 1)
                {
                    Ability ab = InventoryController.Instance.abilities[shopSaveData[i].ShopItemID];
                    shopItemUIs[i].SetItem(ab);

                }
                else if (shopSaveData[i].ShopTypeID == 2)
                {
                    RelicSO relic = InventoryController.Instance.relics[shopSaveData[i].ShopItemID];
                    shopItemUIs[i].SetItem(relic);
                }
                shopItemUIs[i].gameObject.SetActive(true);
                shopItemUIs[i].PurchasedGO.SetActive(false);
            }
            for (int i = shopSaveData.Count; i < shopItemUIs.Count; i++)
            {
                shopItemUIs[i].gameObject.SetActive(false);
                shopItemUIs[i].PurchasedGO.SetActive(false);
            }
        }
        else if (currentProgressOnFloor != PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor)
        {
            shopSaveData = new List<ShopSaveData>();
            currentProgressOnFloor = PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor;
            soldItem = 0;
            for (int i = 0; i < shopItemUIs.Count; i++)
            {
                int rnd = Random.Range(0, 99);
                //Itm
                if (rnd <= 75)
                {
                    int shopR = Random.Range(0, InventoryController.Instance.gameItems.Count);

                    if ((int)InventoryController.Instance.gameItems[shopR].floorAvailable <= PreBattleSelectionController.Instance.GameDetails.Floor)
                    {

                        shopSaveData.Add(new ShopSaveData(0, shopR));
                        Item itm = InventoryController.Instance.gameItems[shopR];
                        shopItemUIs[i].SetItem(itm);
                    }
                    else
                    {
                        i--;
                        continue;
                    }
                }
                //Ability
                else if (rnd > 75 && rnd <= 90)
                {
                    int shopR = Random.Range(0, InventoryController.Instance.abilities.Count);

                    if ((int)InventoryController.Instance.abilities[shopR].floorAvailable <= PreBattleSelectionController.Instance.GameDetails.Floor)
                    {
                        shopSaveData.Add(new ShopSaveData(1, shopR));
                        Ability ab = InventoryController.Instance.abilities[shopR];
                        shopItemUIs[i].SetItem(ab);
                    }
                    else
                    {
                        i--;
                        continue;
                    }

                }
                //Relic
                else if (rnd > 90 && rnd <= 100)
                {
                    int shopR = Random.Range(0, InventoryController.Instance.relics.Count);

                    if ((int)InventoryController.Instance.relics[shopR].floorAvailable <= PreBattleSelectionController.Instance.GameDetails.Floor)
                    {
                        RelicSO relic = InventoryController.Instance.relics[shopR];

                        if (InventoryController.Instance.ownedRelics.Count == InventoryController.Instance.relics.Count)
                        {
                            Debug.Log("already have all Relics");
                            i--;
                            continue;

                        }
                        if (InventoryController.Instance.ownedRelics.ContainsKey(InventoryController.Instance.ReturnRelic(relic)) && InventoryController.Instance.ownedRelics[InventoryController.Instance.ReturnRelic(relic)] == true)
                        {

                            i--;
                            continue;
                        }
                        else
                        {
                            shopSaveData.Add(new ShopSaveData(1, shopR));
                            shopItemUIs[i].SetItem(relic); shopItemUIs[i].gameObject.SetActive(true);
                            shopItemUIs[i].PurchasedGO.SetActive(false);
                        }
                    }
                    else
                    {
                        i--;
                        continue;
                    }
                }
            }
            for (int i = 0; i < shopItemUIs.Count; i++)
            {
                shopItemUIs[i].gameObject.SetActive(true);
                shopItemUIs[i].PurchasedGO.SetActive(false);
            }
        }
        SaveLoadManager.Save();
    }
    public void SetShopVendorText(bool greeting, bool sold, bool notEnoughMoney, bool emptyWares, bool noGold)
    {

        Greeting.SetActive(greeting);
        if (Sold.activeInHierarchy)
            Sold.SetActive(false);
        Sold.SetActive(sold);
        NotEnoughMoney.SetActive(notEnoughMoney);
        Empty.SetActive(emptyWares);
        NoGold.SetActive(noGold);

        if (greeting == true) {
            Face.SetActive(false);
            Face.GetComponent<Image>().sprite = Face1;
        }
        else if (sold == true) {

            Face.SetActive(true);

            List<string> textOptions = new List<string>();
            textOptions.Add("All purchases go to supporting the lord of the void!");
            textOptions.Add("Another one bites the dust!");
            textOptions.Add("That one will come in useful, i can tell!");
            textOptions.Add("When you buy items it makes my fur stand up!");
            textOptions.Add("Please do consider buying meorw!");

            int r = Random.Range(0, textOptions.Count);
            Sold.GetComponentInChildren<TextMeshProUGUI>().text = textOptions[r];
            Face.GetComponent<Image>().sprite = Face1;
        }
        else if (notEnoughMoney == true)
        {
            Face.SetActive(true);
            Face.GetComponent<Image>().sprite = Face2;
        }
        else if (emptyWares == true)
        {
            Face.SetActive(true);
            Face.GetComponent<Image>().sprite = Face3;
        } else if (noGold == true)
        {
            Face.SetActive(true);
            Face.GetComponent<Image>().sprite = Face4;
        }
    }
    public void MoveToNextFloor()
    {
        if (soldItem > 0)
        {
            PreBattleSelectionController.Instance.SetFloor(PreBattleSelectionController.Instance.GameDetails.Floor, PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor + 1);
        }
    }

}
[Serializable]
public class ShopSaveData
{
    public int ShopTypeID;
    public int ShopItemID;

    public ShopSaveData(int shopTypeID, int shopItemID)
    {
        ShopTypeID = shopTypeID;
        ShopItemID = shopItemID;
    }
}
