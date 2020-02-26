using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMenuUI : MonoBehaviour
{
    public static WorldMenuUI Instance;
    [SerializeField] private PartyOptions partyOptions;
    [SerializeField] private ItemOptions itemOptions;
    [SerializeField] private RelicOptions relicOptions;
    [SerializeField] private GameObject bottomBar;
    [SerializeField] private GameObject topBar;
 

    public PartyOptions PartyOptions { get => partyOptions; set => partyOptions = value; }
    public ItemOptions ItemOptions { get => itemOptions; set => itemOptions = value; }
    public GameObject BottomBar { get => bottomBar; set => bottomBar = value; }
    public GameObject TopBar { get => topBar; set => topBar = value; }
    public RelicOptions RelicOptions { get => relicOptions; set => relicOptions = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy || MenuTransitionsController.Instance.transitions[1].gameObject.activeInHierarchy) 
            return;

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (PartyOptions.gameObject.activeInHierarchy)
            {
                CloseParty();
                return;
            }
            else {
                OpenAndSetParty();
                return;
            }
        }
    }

    public void ToggleMenuBars(bool active) {

        if (active) {
            BottomBar.SetActive(true);
            BottomBar.transform.localScale = Vector3.one;
            TopBar.SetActive(true);
            TopBar.transform.localScale = Vector3.one;
        }
        else
        {
            BottomBar.SetActive(false);
            TopBar.transform.localScale = Vector3.one;
            TopBar.SetActive(false);
            BottomBar.transform.localScale = Vector3.one;
        }

    }
    public void OpenAndSetInventory() {
        StartCoroutine(OpenAndSetInventoryCoroutine());
    }
    public IEnumerator OpenAndSetInventoryCoroutine()
    {
        if (partyOptions.gameObject.activeInHierarchy)
        {
            partyOptions.OnMenuBackwards(true);
        }
        BattleUI.DoFadeIn(itemOptions.gameObject, 0.35f);
        ItemOptions.SetItemMenu(ItemOptions.lastItemSelectedMenu);
        ItemOptions.transform.localScale = Vector3.one;
        ItemOptions.gameObject.SetActive(true);
        DoFadeIn(ItemOptions.MainBody, 0.35f);
        StartCoroutine(OpenMenu(ItemOptions.MainBody, 0, 0.25f));
        StartCoroutine(BattleUI.ToggleMenuFromAtoB(itemOptions.Header, 0, 0.25f, new Vector3(0, 200, 0), Vector3.zero));
        StartCoroutine(BattleUI.ToggleMenuFromAtoB(itemOptions.BottomBar, 0, 0.25f, new Vector3(0, -250, 0), Vector3.zero));
        yield return new WaitForSeconds(0.45f);
    }
    public void OpenAndSetParty() {
        StartCoroutine(OpenAndSetPartyCoroutine());
    }
    public IEnumerator OpenAndSetPartyCoroutine() {

       
        if (itemOptions.gameObject.activeInHierarchy)
        {
                
            itemOptions.OnMenuBackwards(true);
        }
        if (partyOptions.gameObject.activeInHierarchy ) {

            if (BattleUI.Instance.CurrentMenuStatus == MenuStatus.AddReplaceAbility)
               yield return null ;
            else BattleUI.Instance.CurrentMenuStatus = MenuStatus.Normal;
        }
        PartyOptions.transform.localScale = Vector3.one;
        PartyOptions.transform.GetChild(0).GetChild(1).GetComponent<VerticalLayoutGroup>().enabled = false;
        PartyOptions.SetUI(BattleController.Instance.MasterPlayerParty);
        PartyOptions.gameObject.SetActive(true);
        DoFadeIn(PartyOptions.gameObject, 0.35f);
        StartCoroutine(OpenMenuFromSideToCenter(PartyOptions.GetGameObjects(), 0, 0.25f, 1080));
        StartCoroutine(ToggleMenuFromBottomToCenter(PartyOptions.BottomBar, 0f, 0.25f, -250, 0));
        StartCoroutine(ToggleMenuFromBottomToCenter(PartyOptions.Header, 0f, 0.25f, 250, 0));
        yield return new WaitForSeconds(0.45f);
        PartyOptions.transform.GetChild(0).GetChild(1).GetComponent<VerticalLayoutGroup>().enabled = true;
    }
    public void CloseParty()
    {
        StartCoroutine(PartyOptions.OnMenuBackwardsWorld());
        
    }

    public void OpenAndSetRelicOptions() {
        StartCoroutine(OpenAndSetRelicCoroutine());
    }
    public IEnumerator OpenAndSetRelicCoroutine() {
        if (relicOptions.gameObject.activeInHierarchy)
        {
            relicOptions.OnMenuBackwards(true);
        }
        BattleUI.DoFadeIn(relicOptions.gameObject, 0.35f);
        relicOptions.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.45f);
    }
    public void CloseRelicOptions() {
        StartCoroutine(RelicOptions.OnMenuBackwards());
    }
    public void OpenMenuViaBattleUI(GameObject go, float delay, float duartion)
    {
        StartCoroutine(OpenMenu(go, delay, duartion));
    }
    public static IEnumerator OpenMenu(GameObject go, float delay, float duartion)
    {

        yield return new WaitForSeconds(delay);

        go.SetActive(true);
        DoFadeIn(go, 0.25f);
        go.transform.DOScale(Vector3.one, duartion);
        yield return new WaitForSeconds(duartion);
    }
    public void CloseMenuViaBattleUI(GameObject go, float delay, float duartion)
    {
        StartCoroutine(CloseMenu(go, delay, duartion));
    }

    public static IEnumerator CloseMenu(GameObject go, float delay, float duartion)
    {
        yield return new WaitForSeconds(delay);
        go.transform.DOScale(Vector3.zero, duartion);
        DoFadeOut(go, 0.25f);
        yield return new WaitForSeconds(duartion);
        go.SetActive(false);
    }
    public static IEnumerator OpenMenuFromSideToCenter(List<GameObject> gameObjects, float delay, float duration, float startingX)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            RectTransform rect = gameObjects[i].GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector3(startingX, rect.anchoredPosition.y);
            rect.localScale = Vector3.one;
            yield return new WaitForSeconds(delay);
            rect.DOAnchorPos(new Vector3(0, rect.anchoredPosition.y, 0), duration);
        }
    }
    public static IEnumerator CloseMenuFromSideToCenter(List<GameObject> gameObjects, float delay, float duration, float endingX)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            RectTransform rect = gameObjects[i].GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.anchoredPosition = new Vector3(0, rect.anchoredPosition.y);
            yield return new WaitForSeconds(delay);
            rect.DOAnchorPos(new Vector3(endingX, rect.anchoredPosition.y, 0), duration);
        }
    }
    public static IEnumerator OpenMenuFromBottomToCenter(List<GameObject> gameObjects, float delay, float duration, float startingY, float endingY)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            RectTransform rect = gameObjects[i].GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.anchoredPosition = new Vector3(rect.anchoredPosition.x, startingY);
            yield return new WaitForSeconds(delay);
            rect.DOAnchorPos(new Vector3(rect.anchoredPosition.x, endingY), duration);
        }
    }
    public static IEnumerator ToggleMenuFromBottomToCenter(GameObject gameObject, float delay, float duration, float startingY, float endingY)
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.localScale = Vector3.one;
        rect.anchoredPosition = new Vector3(rect.anchoredPosition.x, startingY);
        yield return new WaitForSeconds(delay);
        rect.DOAnchorPos(new Vector3(rect.anchoredPosition.x, endingY, 0), duration);
    }
    public static IEnumerator ToggleMenuFromAtoB(GameObject gameObject, float delay, float duration, Vector2 startPos, Vector2 endPos)
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.anchoredPosition = startPos;
        rect.localScale = Vector3.one;
        yield return new WaitForSeconds(delay);
        rect.DOAnchorPos(endPos, duration);
    }
    public static void DoFadeIn(GameObject go, float duration)
    {

        if (!go.activeInHierarchy)
            go.SetActive(true);
        if (go.GetComponent<CanvasGroup>())
        {
            go.GetComponent<CanvasGroup>().DOFade(1, duration);
        }
    }
    public static void DoFadeOut(GameObject go, float duration)
    {
        go.SetActive(true);
        if (go.GetComponent<CanvasGroup>())
        {
            go.GetComponent<CanvasGroup>().DOFade(0, duration);
        }
    }

    public IEnumerator UseRelicEvent(RelicName relicName, bool skipChanceCheck)
    {
        if (InventoryController.Instance.ownedRelics.ContainsKey((int)relicName) && InventoryController.Instance.ownedRelics[(int)relicName] == true)
        {
            if (InventoryController.Instance.relicsScripts[(int)relicName].CalculateChance() == true || skipChanceCheck == true)
            {
                gameObject.SetActive(true);

                GameObject relicGO = Instantiate(InventoryController.Instance.relicsScripts[(int)relicName].gameObject) as GameObject;
                Relics relic = relicGO.GetComponent<Relics>();
                yield return StartCoroutine(relic.RunEffect());
            }
        }
        yield return null;
    }
    public bool CheckRelicChange(RelicName relicName) {

        if (InventoryController.Instance.ownedRelics.ContainsKey((int)relicName) && InventoryController.Instance.ownedRelics[(int)relicName] == true)
        {
            if (InventoryController.Instance.relicsScripts[(int)relicName].CalculateChance() == true)
                return true;
            else return false;
        }
        else return false;
    }
}
