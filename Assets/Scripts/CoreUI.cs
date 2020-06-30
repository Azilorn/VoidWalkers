using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum MenuStatus { Normal, ItemSelectCreature, CloseMenu, SelectNewCreaturePostDeath, WorldUIRevive, WorldTavernRevive, AddReplaceAbility }

public class CoreUI : MonoBehaviour
{
    public static CoreUI Instance;
    [SerializeField] private PartyOptions partyOptions;
    [SerializeField] private ItemOptions itemOptions;
    [SerializeField] private RelicOptions relicOptions;
    [SerializeField] private GameObject bottomBar;
    [SerializeField] private GameObject topBar;

    [SerializeField] private GameObject enemyTrainer;

    static bool locked = false;
    [SerializeField] private PlayerStatsUI[] playerStats;
    [SerializeField] private PlayerOptions playerOptions;
    [SerializeField] private DialogueBox dialogueBox;
    [SerializeField] private Transform battleCanvasTransform;
    [SerializeField] private RewardsScreen rewardsScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject runWinScreen;
    [SerializeField] private MenuStatus currentMenuStatus;
    [SerializeField] private List<GameObject> backgrounds = new List<GameObject>();

    public List<GameObject> portals = new List<GameObject>();

    public PlayerStatsUI[] PlayerStats { get => playerStats; set => playerStats = value; }
    public PlayerOptions PlayerOptions { get => playerOptions; set => playerOptions = value; }
    public DialogueBox DialogueBox { get => dialogueBox; set => dialogueBox = value; }
    public Transform BattleCanvasTransform { get => battleCanvasTransform; set => battleCanvasTransform = value; }
    public MenuStatus CurrentMenuStatus { get => currentMenuStatus; set => currentMenuStatus = value; }
    public RewardsScreen RewardsScreen { get => rewardsScreen; set => rewardsScreen = value; }
    public List<GameObject> Backgrounds { get => backgrounds; set => backgrounds = value; }
    public GameObject LoseScreen { get => loseScreen; set => loseScreen = value; }
    public static bool Locked { get => locked; set => locked = value; }
    public GameObject RunWinScreen { get => runWinScreen; set => runWinScreen = value; }
    public GameObject EnemyTrainer { get => enemyTrainer; set => enemyTrainer = value; }
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
        if (BattleCanvasTransform.gameObject.activeInHierarchy || MenuTransitionsController.Instance.transitions[1].gameObject.activeInHierarchy) 
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

    public static void UnlockUI()
    {
        Locked = false;
    }
    public void SetBattleUIAtStart()
    {
        StartCoroutine(StartBattleCoroutine());
    }
    public IEnumerator StartBattleCoroutine()
    {
        PlayerParty p = BattleController.Instance.MasterPlayerParty;
        PlayerParty e = BattleController.Instance.EnemyParty;

        RewardsScreen.gameObject.SetActive(false);
        SetPlayerBattleUIStatic();


        //Code for Setting correct sprites
        PlayerStats[0].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500, 470);
        PlayerStats[1].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(500, 0);

        CreatureSO player = p.party[p.selectedCreature].creatureSO;
        CreatureSO enemy =  e.party[e.selectedCreature].creatureSO;

        Image pImage = BattleController.Instance.PlayerCreatureImage;
        Image eImage = BattleController.Instance.EnemyCreatureImage;

        pImage.sprite = player.creaturePlayerIcon;
        pImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, player.width);
        pImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, player.height);

        eImage.sprite = enemy.creaturePlayerIcon;
        eImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, enemy.width);
        eImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, enemy.height);

        pImage.rectTransform.localScale = Vector3.zero;
        eImage.rectTransform.localScale = Vector3.zero;

        yield return new WaitForSeconds(1f);
        //Wait for TransitionManager to stop
        while (MenuTransitionsController.Instance.transitions[4].gameObject.activeInHierarchy)
            yield return null;

        //Dialogue Box 
        if (BattleController.Instance.EnemyParty.trainerDefaultImage != null)
            enemyTrainer.GetComponentInChildren<Image>().sprite = BattleController.Instance.EnemyParty.trainerDefaultImage;
        enemyTrainer.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(TypeDialogue("You have been challenged by <color=#05878a><b>" + e.trainerName + "!</color></b>", DialogueBox.Dialogue, 1f, true));
        yield return StartCoroutine(TypeDialogue("<color=#05878a><b>" + e.trainerName + "</color></b> summons " +
          e.party[e.selectedCreature].creatureSO.creatureName, DialogueBox.Dialogue, 1f, true));

        enemyTrainer.GetComponent<Animator>().SetTrigger("leave");
        while (enemyTrainer.gameObject.activeInHierarchy)
            yield return null;

        yield return StartCoroutine(OpenPortal(portals[1]));
        DoFadeIn(BattleController.Instance.EnemyCreatureImage.gameObject, 1f);
       eImage.rectTransform.DOScale(Vector3.one, 0.5f);
        while (portals[0].activeInHierarchy || portals[1].activeInHierarchy)
            yield return null;
        // Change 1st Vector Option for start and 2nd Vector Option for End
        StartCoroutine(ToggleMenuFromAtoB(PlayerStats[1].gameObject, 0f, 0.35f, new Vector2(-1100, -50), new Vector2(-550, -50)));
        yield return StartCoroutine(TypeDialogue("You summon <color=#05878a><b>" +
           p.party[p.selectedCreature].creatureSO.creatureName + "!</color></b>", DialogueBox.Dialogue, 1f, true));
        yield return StartCoroutine(OpenPortal(portals[0]));
        DoFadeIn(pImage.gameObject, 1f);
        pImage.rectTransform.DOScale(Vector3.one, 0.5f);
        while (portals[0].activeInHierarchy || portals[1].activeInHierarchy)
            yield return null;
        // Change 1st Vector Option for start and 2nd Vector Option for End
        StartCoroutine(ToggleMenuFromAtoB(PlayerStats[0].gameObject, 0f, 0.35f, new Vector2(1100, 510), new Vector2(550, 510)));
        yield return StartCoroutine(OpenMenu(PlayerOptions.gameObject, 0, 0.25f));
    }

    public static IEnumerator OpenPortal(GameObject anim)
    {
        anim.gameObject.SetActive(true);
        yield return new WaitForSeconds(anim.GetComponent<ParticleSystem>().main.duration * 0.85f);
    }

    public void SetPlayerBattleUIStatic()
    {
        PlayerParty p = BattleController.Instance.MasterPlayerParty;
        PlayerParty e = BattleController.Instance.EnemyParty;
        PlayerStats[0].SetPlayerStatsMatchStart(p.party[p.selectedCreature], p);
        PlayerStats[1].SetPlayerStatsMatchStart(e.party[e.selectedCreature], e);
    }
    public void SetPlayerBattleUI()
    {
        PlayerParty p = BattleController.Instance.MasterPlayerParty;
        PlayerParty e = BattleController.Instance.EnemyParty;
        StartCoroutine(UseRelicEvent(RelicName.PrayerBeads, false));
        while (RelicUIIcon.Instance.gameObject.activeInHierarchy)
        {

        }
        PlayerStats[0].SetPlayerStats(p.party[p.selectedCreature], p);
        PlayerStats[1].SetPlayerStats(e.party[e.selectedCreature], e);
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
    public static void DoFadeIn(GameObject go, float duration, float delay)
    {

        if (!go.activeInHierarchy)
            go.SetActive(true);
        CanvasGroup cg = go.GetComponent<CanvasGroup>();
        if (cg == null)
            return;
        if (cg.alpha == 1)
        {
            cg.alpha = 0;
        }
        cg.DOFade(1, duration).SetDelay(delay);

    }
  

    public void OpenMenuViaCoreUI(GameObject go, float delay, float duartion)
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
    public void CloseMenuViaCoreUI(GameObject go, float delay, float duartion)
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

    public IEnumerator TypeDialogue(string dialogueText, TextMeshProUGUI dialogueTextBox, float speed, bool disableBox)
    {
        dialogueTextBox.maxVisibleCharacters = 0;
        dialogueTextBox.text = dialogueText;
        dialogueBox.Container.localScale = Vector3.zero;
        dialogueBox.gameObject.SetActive(true);
        dialogueBox.Container.DOScale(Vector3.one, 0.25f);
        yield return new WaitForSeconds(0.25f);
        float timer = 0;
        bool textfinished = false;
        bool clicked = false;
        int totalVisibleCharacters = dialogueTextBox.textInfo.characterCount;
        int counter = 0;
        while (!textfinished && clicked == false)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (!textfinished)
                {
                    textfinished = true;
                    dialogueTextBox.maxVisibleCharacters = dialogueText.Length;
                    break;
                }
            }

            dialogueTextBox.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                textfinished = true;
                yield return null;
            }
            counter += 1;
            yield return new WaitForSeconds(0.01f);
        }
        while (textfinished && clicked == false)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                timer = 1f;
            }
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                clicked = true;
                if (disableBox)
                {
                    dialogueBox.gameObject.SetActive(false);
                    dialogueTextBox.text = "";
                }
            }

            yield return null;
        }
    }
    public IEnumerator TypeDialogue(bool requiresClick, string dialogueText, TextMeshProUGUI dialogueTextBox, float speed, bool disableBox)
    {
        dialogueTextBox.maxVisibleCharacters = 0;
        dialogueTextBox.text = dialogueText;
        dialogueBox.Container.localScale = Vector3.zero;
        dialogueBox.gameObject.SetActive(true);
        dialogueBox.Container.DOScale(Vector3.one, 0.25f);
        yield return new WaitForSeconds(0.25f);
        float timer = 0;
        bool textfinished = false;
        bool clicked = false;
        int totalVisibleCharacters = dialogueTextBox.textInfo.characterCount;
        int counter = 0;
        while (!textfinished && clicked == false)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (!textfinished)
                {
                    textfinished = true;
                    dialogueTextBox.maxVisibleCharacters = dialogueText.Length;
                    break;
                }
            }

            dialogueTextBox.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                textfinished = true;
                yield return null;
            }
            counter += 1;
            yield return new WaitForSeconds(0.01f);
        }
        while (textfinished && clicked == false)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                requiresClick = false;
                timer = 1f;
            }
            if (requiresClick == true)
            {
                yield return null;
            }
            else
            {
                timer += Time.deltaTime;
            }
            if (timer >= 1f)
            {
                clicked = true;
                if (disableBox)
                {
                    dialogueBox.gameObject.SetActive(false);
                    dialogueTextBox.text = "";
                }
            }

            yield return null;
        }
    }
    public IEnumerator TypeDialogue(string dialogueText, TextMeshProUGUI dialogueTextBox, float speed, bool disableBox, bool lockUI)
    {
        locked = true;
        dialogueTextBox.maxVisibleCharacters = 0;
        dialogueTextBox.text = dialogueText;
        dialogueBox.Container.localScale = Vector3.zero;
        dialogueBox.gameObject.SetActive(true);
        dialogueBox.Container.DOScale(Vector3.one, 0.25f);
        yield return new WaitForSeconds(0.25f);
        float timer = 0;
        bool textfinished = false;
        bool clicked = false;
        int totalVisibleCharacters = dialogueTextBox.textInfo.characterCount;
        int counter = 0;
        while (!textfinished && clicked == false)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (!textfinished)
                {
                    textfinished = true;
                    dialogueTextBox.maxVisibleCharacters = dialogueText.Length;
                    break;
                }
            }

            dialogueTextBox.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                textfinished = true;
                yield return null;
            }
            counter += 1;
            yield return new WaitForSeconds(0.01f);
        }
        while (textfinished && clicked == false)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                timer = 1f;
            }
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                clicked = true;
                if (disableBox)
                {
                    dialogueBox.gameObject.SetActive(false);
                    dialogueTextBox.text = "";
                    locked = false;
                }
            }

            yield return null;
        }
    }
    public IEnumerator TypeDialogue(List<string> dialogueText, TextMeshProUGUI dialogueTextBox, float speed, bool disableBox)
    {
        for (int i = 0; i < dialogueText.Count; i++)
        {
            dialogueTextBox.maxVisibleCharacters = 0;
            dialogueTextBox.text = dialogueText[i];
            if (disableBox)
            {
                dialogueBox.Container.localScale = Vector3.zero;
                dialogueBox.Container.DOScale(Vector3.one, 0.25f);
            }
            dialogueBox.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            float timer = 0;
            bool textfinished = false;
            bool clicked = false;
            int totalVisibleCharacters = dialogueTextBox.textInfo.characterCount;
            int counter = 0;
            while (!textfinished && clicked == false)
            {
                int visibleCount = counter % (totalVisibleCharacters + 1);

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    if (!textfinished)
                    {
                        textfinished = true;
                        dialogueTextBox.maxVisibleCharacters = dialogueText[i].Length;
                        break;
                    }
                }

                dialogueTextBox.maxVisibleCharacters = visibleCount;

                if (visibleCount >= totalVisibleCharacters)
                {
                    textfinished = true;
                    yield return null;
                }
                counter += 1;
                yield return new WaitForSeconds(0.01f);
            }
            while (textfinished && clicked == false)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    timer = 1f;
                }
                timer += Time.deltaTime;
                if (timer >= 1f)
                {
                    clicked = true;
                    if (disableBox)
                        dialogueTextBox.text = "";
                }

                yield return null;
            }
        }
        if (disableBox)
            dialogueBox.gameObject.SetActive(false);

    }


    public IEnumerator OpenPartyOptions()
    {
        PlayerOptions.PartyOptions.SetUI();
        PlayerOptions.PartyOptions.gameObject.SetActive(true);
        PlayerOptions.PartyOptions.transform.localScale = Vector3.one;
        DoFadeIn(PlayerOptions.PartyOptions.gameObject, 0.10f);
        yield return new WaitForEndOfFrame();
    }
    public IEnumerator ClosePartyOptions()
    {
        PlayerOptions.PartyOptions.transform.localScale = Vector3.one;
        DoFadeOut(PlayerOptions.PartyOptions.gameObject, 0.25f);
        yield return new WaitForSeconds(0.35f);
        PlayerOptions.PartyOptions.gameObject.SetActive(false);
    }

    public void OpenAndSetInventory()
    {
        StartCoroutine(OpenAndSetInventoryCoroutine());
    }
    public IEnumerator OpenAndSetInventoryCoroutine()
    {
        if (partyOptions.gameObject.activeInHierarchy)
        {
            partyOptions.OnMenuBackwards(true);
        }
        ItemOptions.SetItemMenu(ItemOptions.lastItemSelectedMenu);
        DoFadeIn(ItemOptions.gameObject, 0.15f);
        ItemOptions.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
    }
    public void OpenAndSetParty()
    {
        StartCoroutine(OpenAndSetPartyCoroutine());
    }
    public IEnumerator OpenAndSetPartyCoroutine()
    {


        if (itemOptions.gameObject.activeInHierarchy)
        {

            itemOptions.OnMenuBackwards(true);
        }
        if (partyOptions.gameObject.activeInHierarchy)
        {

            if (CurrentMenuStatus == MenuStatus.AddReplaceAbility)
                yield return null;
            else CurrentMenuStatus = MenuStatus.Normal;
        }
        PartyOptions.SetUI();
        DoFadeIn(PartyOptions.gameObject, 0.15f);
        PartyOptions.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
    }
    public void CloseParty()
    {
        StartCoroutine(PartyOptions.OnMenuBackwardsWorld());

    }
    public void OpenAndSetRelicOptions()
    {
        StartCoroutine(OpenAndSetRelicCoroutine());
    }
    public IEnumerator OpenAndSetRelicCoroutine()
    {
        if (relicOptions.gameObject.activeInHierarchy)
        {
            relicOptions.OnMenuBackwards(true);
        }
        DoFadeIn(relicOptions.gameObject, 0.15f);
        relicOptions.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
    }
    public void CloseRelicOptions()
    {
        StartCoroutine(RelicOptions.OnMenuBackwards());
    }


    public IEnumerator SelectNewCreatureAfterDeath()
    {

        currentMenuStatus = MenuStatus.SelectNewCreaturePostDeath;
        yield return StartCoroutine(OpenPartyOptions());

        while (currentMenuStatus == MenuStatus.SelectNewCreaturePostDeath)
        {

            yield return new WaitForEndOfFrame();
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
