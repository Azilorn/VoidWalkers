using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite abilityImage;
    [SerializeField] private TextMeshProUGUI optionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private GameObject purchasedGO;
    private Ability ability;
    private Item itm;
    private RelicSO relic;
    public Ability Ability { get => ability; set => ability = value; }
    public Item Itm { get => itm; set => itm = value; }
    public RelicSO Relic { get => relic; set => relic = value; }
    public GameObject PurchasedGO { get => purchasedGO; set => purchasedGO = value; }

    public void SetItem(Item i) {

        relic = null;
        ability = null;
        icon.sprite = i.itemIcon;
        optionText.text = i.itemName;
        costText.text = i.cost.ToString();
        itm = i;
    }
    public void SetItem(Ability a) {
        relic = null;
        itm = null;
        icon.sprite = abilityImage;
        optionText.text = a.abilityName;
        costText.text = a.cost.ToString();
        ability = a;
    }
    public void SetItem(RelicSO r) {
        itm = null;
        ability = null;
        icon.sprite = r.icon;
        optionText.text = r.relicName;
        costText.text = r.cost.ToString();
        relic = r;
    }
    public void GetItem() {
        
        if (Ability != null) {
            if (PreBattleSelectionController.Instance.GameDetails.Gold < Ability.cost)
            {
                shopUI.SetShopVendorText(false, false, true, false);
                AudioManager.Instance.activeSFXStitching =  StartCoroutine(AudioManager.Instance.PlayAudioWithMultipleParts(UIAudio.Instance.MobsEssentialsAudio[1].AudioList));
                return;
            }
            PurchasedGO.SetActive(true);
            InventoryController.Instance.AddAbility(Ability);
            PreBattleSelectionController.Instance.GameDetails.Gold -= Ability.cost;
            Ability = null;
            ShopUI.soldItem++;
            if (ShopUI.sellableItem != 0 && ShopUI.soldItem >= ShopUI.sellableItem)
            {
                Debug.Log("sellable Item " + ShopUI.sellableItem + " sold item " + ShopUI.soldItem);
                shopUI.SetShopVendorText(false, false, false, true);
            }
            else
            {
                shopUI.SetShopVendorText(false, true, false, false);
                AudioManager.Instance.PlayUISFX(UIAudio.Instance.MobsEssentialsAudio[0].AudioList[0].audio, 1, false);
            }
        }
        else if (Itm != null) {

            if (PreBattleSelectionController.Instance.GameDetails.Gold < Itm.cost)
            {
                shopUI.SetShopVendorText(false, false, true, false);
                AudioManager.Instance.activeSFXStitching = StartCoroutine( AudioManager.Instance.PlayAudioWithMultipleParts(UIAudio.Instance.MobsEssentialsAudio[1].AudioList));
                return;
            }
            PurchasedGO.SetActive(true);
            InventoryController.Instance.AddItem(Itm);
            PreBattleSelectionController.Instance.GameDetails.Gold -= Itm.cost;
            Itm = null;
            ShopUI.soldItem++;
            if (ShopUI.sellableItem != 0 && ShopUI.soldItem >= ShopUI.sellableItem)
            {
                Debug.Log("sellable Item " + ShopUI.sellableItem + " sold item " + ShopUI.soldItem);
                shopUI.SetShopVendorText(false, false, false, true);
            }
            else
            {
                shopUI.SetShopVendorText(false, true, false, false);
                AudioManager.Instance.PlayUISFX(UIAudio.Instance.MobsEssentialsAudio[0].AudioList[0].audio, 1, false);
            }
        }
        else if (Relic != null) {

            if (PreBattleSelectionController.Instance.GameDetails.Gold < Relic.cost)
            {
                shopUI.SetShopVendorText(false, false, true, false);
                AudioManager.Instance.activeSFXStitching = StartCoroutine(AudioManager.Instance.PlayAudioWithMultipleParts(UIAudio.Instance.MobsEssentialsAudio[1].AudioList));
                return;
            }
            PurchasedGO.SetActive(true);
            InventoryController.Instance.AddRelic(Relic);
            PreBattleSelectionController.Instance.GameDetails.Gold -= Relic.cost;
            Relic = null;
            ShopUI.soldItem++;
            if (ShopUI.sellableItem != 0 && ShopUI.soldItem >= ShopUI.sellableItem)
            {
                Debug.Log("sellable Item " + ShopUI.sellableItem + " sold item " + ShopUI.soldItem);
                shopUI.SetShopVendorText(false, false, false, true);
            }
            else
            {
                shopUI.SetShopVendorText(false, true, false, false);
                AudioManager.Instance.PlayUISFX(UIAudio.Instance.MobsEssentialsAudio[0].AudioList[0].audio, 1, false);
            }
        }
    }
 }
