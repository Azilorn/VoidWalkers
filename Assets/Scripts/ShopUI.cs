using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public int currentProgressOnFloor = 0;
    public static int soldItem = 0;
    public static int sellableItem = 0;
    public List<ShopItemUI> shopItemUIs = new List<ShopItemUI>();
    public GameObject Greeting, Sold, NotEnoughMoney, Empty;

    private void OnEnable()
    {
        SetShopVendorText(true, false, false, false);
        
        if (currentProgressOnFloor != PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor)
        {
            currentProgressOnFloor = PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor;
            int r = Random.Range(2, shopItemUIs.Count);
            sellableItem = r;
            soldItem = 0;
            for (int i = 0; i < r; i++)
            {
                int rnd = Random.Range(0, 99);
                //Itm
                if (rnd <= 33)
                {
                    Item itm = InventoryController.Instance.gameItems[Random.Range(0, InventoryController.Instance.gameItems.Count)];
                    shopItemUIs[i].SetItem(itm);
                }
                //Ability
                else if (rnd > 33 && rnd <= 66)
                {
                    Ability ab = InventoryController.Instance.abilities[Random.Range(0, InventoryController.Instance.abilities.Count)];
                    shopItemUIs[i].SetItem(ab);
                }
                //Relic
                else if (rnd > 66 && rnd <= 99)
                {
                    RelicSO relic = InventoryController.Instance.relics[Random.Range(0, InventoryController.Instance.relics.Count)];
                    shopItemUIs[i].SetItem(relic);
                }
                shopItemUIs[i].gameObject.SetActive(true);
                shopItemUIs[i].PurchasedGO.SetActive(false);
            }
            for (int i = r; i < shopItemUIs.Count; i++)
            {
                shopItemUIs[i].gameObject.SetActive(false);
                shopItemUIs[i].PurchasedGO.SetActive(false);
            }
        }
    }
    public void SetShopVendorText(bool greeting, bool sold, bool notEnoughMoney, bool emptyWares) {

        Greeting.SetActive(greeting);
        Sold.SetActive(sold);
        NotEnoughMoney.SetActive(notEnoughMoney);
        Empty.SetActive(emptyWares);
    }
    public void MoveToNextFloor()
    {
        if(soldItem > 0)
            PreBattleSelectionController.Instance.SetPostBattleUIDetails(PreBattleSelectionController.Instance.GameDetails.Floor, PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor + 1);
    }
}
