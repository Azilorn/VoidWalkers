using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopItemUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite abilityImage;
    [SerializeField] private TextMeshProUGUI optionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private GameObject purchasedGO;


    [SerializeField] private ItemDetailsUI itemDetails;
    [SerializeField] private AttackDetailsUI attackDetails;
    //[SerializeField] private RelicDetalsUI relicDetails;

    private Ability ability;
    private Item itm;
    private RelicSO relic;
    public Ability Ability { get => ability; set => ability = value; }
    public Item Itm { get => itm; set => itm = value; }
    public RelicSO Relic { get => relic; set => relic = value; }
    public GameObject PurchasedGO { get => purchasedGO; set => purchasedGO = value; }

    [SerializeField] private float holdTimer;
    [SerializeField] private float holdDurationRequired = 0.35f;
    [SerializeField] private bool buttonClicked;
    [SerializeField] private bool buttonHeld;

    public void Update()
    {
        if (buttonHeld)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer > holdDurationRequired)
            {
                if (Ability != null)
                {
                    attackDetails.SetMenu(Ability);
                    attackDetails.gameObject.SetActive(true);
                    BattleUI.DoFadeIn(attackDetails.gameObject, 0.15f);
                    StartCoroutine(BattleUI.OpenMenu(attackDetails.MainBody.gameObject, 0f, 0.25f));
                }
                else if (Itm != null)
                {
                    itemDetails.SetMenu(Itm); 
                    itemDetails.gameObject.SetActive(true);
                    BattleUI.DoFadeIn(itemDetails.gameObject, 0.15f);
                    StartCoroutine(BattleUI.OpenMenu(itemDetails.MainBody.gameObject, 0f, 0.25f));
                }
                else if (Relic != null)
                {
                   
                }
                buttonHeld = false;
                buttonClicked = false;
                return;
            }
        }
        else if (!buttonHeld)
        {
            if (buttonClicked)
            {
                GetItem();
                buttonClicked = false;
            }
        }
    }

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
                shopUI.SetShopVendorText(false, false, true, false, false);
                AudioManager.Instance.activeSFXStitching =  StartCoroutine(AudioManager.Instance.PlayAudioWithMultipleParts(UIAudio.Instance.MobsEssentialsAudio[1].AudioList));
                return;
            }
            PurchasedGO.SetActive(true);
            InventoryController.Instance.AddAbility(Ability);
            PreBattleSelectionController.Instance.GameDetails.Gold -= Ability.cost;
            CoreGameInformation.currentRunDetails.GoldSpent += ability.cost;
            Ability = null;
            ShopUI.soldItem++;
            if (ShopUI.sellableItem != 0 && ShopUI.soldItem >= ShopUI.sellableItem)
            {
                Debug.Log("sellable Item " + ShopUI.sellableItem + " sold item " + ShopUI.soldItem);
                shopUI.SetShopVendorText(false, false, false, true, false);
            }
            else
            {
                if (PreBattleSelectionController.Instance.GameDetails.Gold <= 0)
                {
                    shopUI.SetShopVendorText(false, false, false, false, true);
                }
                else
                {
                    shopUI.SetShopVendorText(false, true, false, false, false);
                    AudioManager.Instance.PlayUISFX(UIAudio.Instance.MobsEssentialsAudio[0].AudioList[0].audio, 1, false);
                }
            }
        }
        else if (Itm != null) {

            if (PreBattleSelectionController.Instance.GameDetails.Gold < Itm.cost)
            {
                shopUI.SetShopVendorText(false, false, true, false, false);
                AudioManager.Instance.activeSFXStitching = StartCoroutine( AudioManager.Instance.PlayAudioWithMultipleParts(UIAudio.Instance.MobsEssentialsAudio[1].AudioList));
                return;
            }
            PurchasedGO.SetActive(true);
            InventoryController.Instance.AddItem(Itm);
            PreBattleSelectionController.Instance.GameDetails.Gold -= Itm.cost;
            CoreGameInformation.currentRunDetails.GoldSpent += Itm.cost;
            Itm = null;
            ShopUI.soldItem++;
            if (ShopUI.sellableItem != 0 && ShopUI.soldItem >= ShopUI.sellableItem)
            {
                Debug.Log("sellable Item " + ShopUI.sellableItem + " sold item " + ShopUI.soldItem);
                shopUI.SetShopVendorText(false, false, false, true, false);
            }
            else
            {
                if (PreBattleSelectionController.Instance.GameDetails.Gold <= 0)
                {
                    shopUI.SetShopVendorText(false, false, false, false, true);
                }
                else
                {
                    shopUI.SetShopVendorText(false, true, false, false, false);
                    AudioManager.Instance.PlayUISFX(UIAudio.Instance.MobsEssentialsAudio[0].AudioList[0].audio, 1, false);
                }
            }
        }
        else if (Relic != null) {

            if (PreBattleSelectionController.Instance.GameDetails.Gold < Relic.cost)
            {
                shopUI.SetShopVendorText(false, false, true, false, false);
                AudioManager.Instance.activeSFXStitching = StartCoroutine(AudioManager.Instance.PlayAudioWithMultipleParts(UIAudio.Instance.MobsEssentialsAudio[1].AudioList));
                return;
            }
            PurchasedGO.SetActive(true);
            InventoryController.Instance.AddRelic(Relic);
            PreBattleSelectionController.Instance.GameDetails.Gold -= Relic.cost;
            CoreGameInformation.currentRunDetails.GoldSpent += Relic.cost;
            Relic = null;
            ShopUI.soldItem++;
            if (ShopUI.sellableItem != 0 && ShopUI.soldItem >= ShopUI.sellableItem)
            {
                Debug.Log("sellable Item " + ShopUI.sellableItem + " sold item " + ShopUI.soldItem);
                shopUI.SetShopVendorText(false, false, false, true, false);
            }
            else
            {
                if (PreBattleSelectionController.Instance.GameDetails.Gold <= 0)
                {
                    shopUI.SetShopVendorText(false, false, false, false, true);
                }
                else
                {
                    shopUI.SetShopVendorText(false, true, false, false, false);
                    AudioManager.Instance.PlayUISFX(UIAudio.Instance.MobsEssentialsAudio[0].AudioList[0].audio, 1, false);
                }
            }
         
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        holdTimer = 0;
        buttonHeld = true;
        buttonClicked = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
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

   
}
