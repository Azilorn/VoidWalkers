using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopUI : MonoBehaviour
{
    public int currentProgressOnFloor = 0;
    public static int soldItem = 0;
    public static int sellableItem = 0;
    public List<ShopItemUI> shopItemUIs = new List<ShopItemUI>();
    public static List<ShopSaveData> shopSaveData = new List<ShopSaveData>();
    public GameObject Greeting, Sold, NotEnoughMoney, Empty;

    private void OnEnable()
    {
        SetShopVendorText(true, false, false, false);

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
            int r = Random.Range(2, shopItemUIs.Count + 1);
            sellableItem = r;
            soldItem = 0;
            for (int i = 0; i < r; i++)
            {
                int rnd = Random.Range(0, 99);
                //Itm
                if (rnd <= 75)
                {
                    int shopR = Random.Range(0, InventoryController.Instance.gameItems.Count);

                    if ((int)InventoryController.Instance.gameItems[shopR].floorAvailable == PreBattleSelectionController.Instance.GameDetails.Floor)
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

                    if ((int)InventoryController.Instance.abilities[shopR].floorAvailable == PreBattleSelectionController.Instance.GameDetails.Floor)
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

                    if ((int)InventoryController.Instance.relics[shopR].floorAvailable == PreBattleSelectionController.Instance.GameDetails.Floor)
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
            for (int i = 0; i < r; i++)
            {
                shopItemUIs[i].gameObject.SetActive(true);
                shopItemUIs[i].PurchasedGO.SetActive(false);
            }
            for (int i = r; i < shopItemUIs.Count; i++)
            {
                shopItemUIs[i].gameObject.SetActive(false);
                shopItemUIs[i].PurchasedGO.SetActive(false);
            }
        }
        SaveLoadManager.Save();
    }
    public void SetShopVendorText(bool greeting, bool sold, bool notEnoughMoney, bool emptyWares)
    {

        Greeting.SetActive(greeting);
        Sold.SetActive(sold);
        NotEnoughMoney.SetActive(notEnoughMoney);
        Empty.SetActive(emptyWares);
    }
    public void MoveToNextFloor()
    {
        if (soldItem > 0)
        {
            PreBattleSelectionController.Instance.SetPostFloorOptionDetails(PreBattleSelectionController.Instance.GameDetails.Floor, PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor + 1);
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
