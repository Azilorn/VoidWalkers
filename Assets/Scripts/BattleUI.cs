using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public enum MenuStatus {Normal, ItemSelectCreature, CloseMenu, SelectNewCreaturePostDeath, WorldUIRevive, WorldTavernRevive }
public class BattleUI : MonoBehaviour
{

    public static BattleUI Instance;
       
    [SerializeField] private PlayerStatsUI[] playerStats;
    [SerializeField] private PlayerOptions playerOptions;
    [SerializeField] private DialogueBox dialogueBox;
    [SerializeField] private Transform battleCanvasTransform;
    [SerializeField] private TransitionManager battleTransitionManager;
    [SerializeField] private RewardsScreen rewardsScreen;
    [SerializeField] private MenuStatus currentMenuStatus;

    public List<Animator> portals = new List<Animator>();

    public PlayerStatsUI[] PlayerStats { get => playerStats; set => playerStats = value; }
    public PlayerOptions PlayerOptions { get => playerOptions; set => playerOptions = value; }
    public DialogueBox DialogueBox { get => dialogueBox; set => dialogueBox = value; }
    public Transform BattleCanvasTransform { get => battleCanvasTransform; set => battleCanvasTransform = value; }
    public MenuStatus CurrentMenuStatus { get => currentMenuStatus; set => currentMenuStatus = value; }
    public TransitionManager BattleTransitionManager { get => battleTransitionManager; set => battleTransitionManager = value; }
    public RewardsScreen RewardsScreen { get => rewardsScreen; set => rewardsScreen = value; }

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

    public void SetBattleUIAtStart()
    {
        StartCoroutine(StartBattleCoroutine());
    }
    public IEnumerator StartBattleCoroutine() {

        WorldMenuUI.Instance.ToggleMenuBars(false);
        RewardsScreen.gameObject.SetActive(false);
        SetPlayerBattleUIStatic();
        
        //Code for Setting correct sprites
        PlayerStats[0].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500, 470);
        PlayerStats[1].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(500, 0);

        CreatureSO player = BattleController.Instance.TurnController.PlayerParty.party[BattleController.Instance.TurnController.PlayerParty.selectedCreature].creatureSO;
        CreatureSO enemy = BattleController.Instance.TurnController.EnemyParty.party[BattleController.Instance.TurnController.EnemyParty.selectedCreature].creatureSO;

        BattleController.Instance.Player1CreatureImage.sprite = player.creaturePlayerIcon;
        BattleController.Instance.Player1CreatureImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, player.width);
        BattleController.Instance.Player1CreatureImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, player.height);

        BattleController.Instance.Player2CreatureImage.sprite = enemy.creatureEnemyIcon;
        BattleController.Instance.Player2CreatureImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, enemy.width);
        BattleController.Instance.Player2CreatureImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, enemy.height);

        BattleController.Instance.Player1CreatureImage.rectTransform.localScale = Vector3.zero;
        BattleController.Instance.Player2CreatureImage.rectTransform.localScale = Vector3.zero;

        //Wait for TransitionManager to stop
        while (BattleTransitionManager.gameObject.activeInHierarchy)
            yield return null;

        //Dialogue Box 
        yield return StartCoroutine(TypeDialogue("Get Ready to Fight <color=#8E4040><b>Void Walker!</color></b>", DialogueBox.Dialogue, 1f, true));

        yield return StartCoroutine(OpenPortal(portals[1]));
        DoFadeIn(BattleController.Instance.Player2CreatureImage.gameObject, 0.5f);
        BattleController.Instance.Player2CreatureImage.rectTransform.DOScale(Vector3.one, 0.5f);
        StartCoroutine(ToggleMenuFromAtoB(PlayerStats[1].gameObject, 0f, 0.35f, new Vector2(500, 0), new Vector2(-50, 0)));
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(OpenPortal(portals[0]));
        DoFadeIn(BattleController.Instance.Player1CreatureImage.gameObject, 0.5f);  
        BattleController.Instance.Player1CreatureImage.rectTransform.DOScale(Vector3.one, 0.5f);
        StartCoroutine(ToggleMenuFromAtoB(PlayerStats[0].gameObject, 0f, 0.35f, new Vector2(-500, 470), new Vector2(50, 470)));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(OpenMenu(PlayerOptions.gameObject, 0, 0.25f));
    }
    public static IEnumerator OpenPortal(Animator anim) {
        anim.gameObject.SetActive(true);
        anim.Play("Entry Portal", 0);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length - 0.2f);
    }
    public void SetPlayerBattleUIStatic()
    {
        PlayerStats[0].SetPlayerStatsMatchStart(BattleController.Instance.TurnController.PlayerParty.party[BattleController.Instance.TurnController.PlayerParty.selectedCreature], BattleController.Instance.TurnController.PlayerParty);
        PlayerStats[1].SetPlayerStatsMatchStart(BattleController.Instance.TurnController.EnemyParty.party[BattleController.Instance.TurnController.EnemyParty.selectedCreature], BattleController.Instance.TurnController.EnemyParty);
    }
    public void SetPlayerBattleUI()
    {
        PlayerStats[0].SetPlayerStats(BattleController.Instance.TurnController.PlayerParty.party[BattleController.Instance.TurnController.PlayerParty.selectedCreature], BattleController.Instance.TurnController.PlayerParty);
        PlayerStats[1].SetPlayerStats(BattleController.Instance.TurnController.EnemyParty.party[BattleController.Instance.TurnController.EnemyParty.selectedCreature], BattleController.Instance.TurnController.EnemyParty);
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
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                timer = 1f;
            }
            timer += Time.deltaTime;
            if (timer >= 1f) {
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
        if(disableBox)
            dialogueBox.gameObject.SetActive(false);

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
        for (int i = 0; i < gameObjects.Count; i++) {
            RectTransform rect = gameObjects[i].GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(startingX, rect.anchoredPosition.y);
            rect.transform.localScale = Vector3.one;
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
            rect.transform.localScale = Vector3.one;
            rect.anchoredPosition = new Vector3(rect.anchoredPosition.x, startingY);
            yield return new WaitForSeconds(delay);
            rect.DOAnchorPos(new Vector3(rect.anchoredPosition.x, endingY), duration);
        }
    }
    public static IEnumerator ToggleMenuFromBottomToCenter(GameObject gameObject, float delay, float duration, float startingY, float endingY)
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        gameObject.transform.localScale = Vector3.one;
        rect.anchoredPosition = new Vector3(rect.anchoredPosition.x, startingY);
        yield return new WaitForSeconds(delay);
        rect.DOAnchorPos(new Vector3(rect.anchoredPosition.x, endingY, 0), duration);
    }
    public static IEnumerator ToggleMenuFromAtoB(GameObject gameObject, float delay, float duration, Vector2 startPos, Vector2 endPos)
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.anchoredPosition = startPos;
        gameObject.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(delay);
        rect.DOAnchorPos(endPos, duration, true);
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

    public IEnumerator OpenPartyOptions()
    {
        PlayerOptions.PartyOptions.SetUI();
        PlayerOptions.PartyOptions.gameObject.SetActive(true);
        PlayerOptions.PartyOptions.transform.localScale = Vector3.one;
        DoFadeIn(PlayerOptions.PartyOptions.gameObject, 0.10f);
        StartCoroutine(OpenMenuFromSideToCenter(PlayerOptions.PartyOptions.GetGameObjects(), 0.02f, 0.35f, Camera.main.pixelWidth * 2));
        StartCoroutine(ToggleMenuFromBottomToCenter(PlayerOptions.PartyOptions.BottomBar, 0f, 0.25f, -250, 0));
        StartCoroutine(ToggleMenuFromBottomToCenter(PlayerOptions.PartyOptions.Header, 0f, 0.25f, 250, 0));

        yield return new WaitForEndOfFrame();
    }
    public IEnumerator ClosePartyOptions()
    {
        PlayerOptions.PartyOptions.transform.localScale = Vector3.one;
        DoFadeOut(PlayerOptions.PartyOptions.gameObject, 0.25f);
        StartCoroutine(CloseMenuFromSideToCenter(PlayerOptions.PartyOptions.GetGameObjects(), 0.02f, 0.35f, 1115));
        StartCoroutine(ToggleMenuFromBottomToCenter(PlayerOptions.PartyOptions.BottomBar, 0f, 0.25f, 0, 0));
        StartCoroutine(ToggleMenuFromBottomToCenter(PlayerOptions.PartyOptions.Header, 0f, 0.25f, 0, 0));
        yield return new WaitForSeconds(0.35f);
        PlayerOptions.PartyOptions.gameObject.SetActive(false);
    }
    public static void DoFadeIn(GameObject go, float duration) {

        if(!go.activeInHierarchy)
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
}
