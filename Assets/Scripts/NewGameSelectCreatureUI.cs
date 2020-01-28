using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NewGameSelectCreatureUI : MonoBehaviour
{

    public static NewGameSelectCreatureUI Instance;

    public static CreatureSO[] creaturesSelected;
    public CreatureListController creatureListController;
    public GameObject creatureSelectOptionMenu;
    public static int currentlySelectedOption;
    public static List<NewGameSelectCreatureOptionUI> creatureSelectOptions;
    public GameObject creatureSelectOptionParent;

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
    private void Start()
    {
        currentlySelectedOption = 0;
     
        creaturesSelected = new CreatureSO[6];
        creatureSelectOptions = new List<NewGameSelectCreatureOptionUI>();
            for (int i = 0; i < creatureSelectOptionParent.transform.childCount; i++)
            {
                creatureSelectOptions.Add(creatureSelectOptionParent.transform.GetChild(i).gameObject.GetComponent<NewGameSelectCreatureOptionUI>());
            }
    }

    public void RandomParty() {

        CreatureTable creatureTable = CreatureTable.Instance;

        MenuTransitionsController.Instance.StartTransition(0, true);
        creatureListController.CreateCreatureListUIOptions();

        List<CreatureSO> creatures = new List<CreatureSO>();
        for (int i = 0; i < creaturesSelected.Length; i++) {

            int rnd = Random.Range(0, creatureTable.Creatures.Count);
            if (creatureTable.UnlockedCreature[rnd] == true)
            creatures.Add(creatureTable.Creatures[rnd]);
                creatures = creatures.Distinct().ToList();

            if (creatures.Count == creaturesSelected.Length)
            {
                break;
            }
            else {
                i--;
            }
        }
        for (int i = 0; i < creaturesSelected.Length; i++)
        {
            int rnd = Random.Range(0, creatureTable.Creatures.Count);
            StartCoroutine(creatureSelectOptions[i].SetCreatureOptionCoroutine(creatures[i], 0.3f));
            creaturesSelected[i] = creatures[i];
        }


    }
    public void OpenCreatureList(GameObject gameObject)
    {
        MenuTransitionsController.Instance.StartTransition(0, false);
        StartCoroutine(OpenCreatureListCoroutine(gameObject, 0.3f));
    }
    public void ReturnToMainMenu() {

       StartCoroutine(SceneController.Instance.LoadSceneAsync(0, 0));
    }
    private IEnumerator OpenCreatureListCoroutine(GameObject gameObject, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        creatureListController.CreateCreatureListUIOptions();
        creatureListController.gameObject.SetActive(true);
        currentlySelectedOption = gameObject.GetComponent<NewGameSelectCreatureOptionUI>().CreatureSelectedID;
    }

    public void StartGameWithParty()
    {
        PlayerCreatureStats[] party = new PlayerCreatureStats[6];
        for (int i = 0; i < creaturesSelected.Length; i++)
        {
            PlayerCreatureStats creature = new PlayerCreatureStats();
            creature.creatureSO = creaturesSelected[i];
            creature.creatureStats = new CreatureStats();
            creature.creatureStats.level = 1;
            creature.creatureStats.Xp = 120;
            creature.SetLevel(1, true);
            creature.creatureAbilities = new CreatureAbility[4];
            for (int j = 0; j < creaturesSelected[i].startingAbilities.Count; j++)
            {
                Ability ability = creaturesSelected[i].startingAbilities[j];
                creature.creatureAbilities[j] = new CreatureAbility(ability, ability.abilityStats.maxCount);

            }
            party[i] = creature;
        }
        PartyBetweenScenes.party = new PlayerParty();
        PartyBetweenScenes.party.party = new PlayerCreatureStats[6];
        for (int i = 0; i < party.Length; i++)
        {
            PartyBetweenScenes.party.party[i] = party[i];
        }
        CoreGameInformation.SetGameLoadState(false);
        SceneController.Instance.LoadScene(2);
    }
}
