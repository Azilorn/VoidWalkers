using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewGameSelectCreatureOptionUI : MonoBehaviour
{
    [SerializeField] private int creatureSelectedID;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI creatureText;
    [SerializeField] private GameObject plusSignGameObject;
    [SerializeField] private GameObject creatureIconGameObject;

    public int CreatureSelectedID { get => creatureSelectedID; set => creatureSelectedID = value; }
    public Image Icon { get => icon; set => icon = value; }
    public TextMeshProUGUI CreatureText { get => creatureText; set => creatureText = value; }
    public GameObject PlusSignGameObject { get => plusSignGameObject; set => plusSignGameObject = value; }
    public GameObject CreatureIconGameObject { get => creatureIconGameObject; set => creatureIconGameObject = value; }

    public void Start()
    {
        CreatureSelectedID = transform.GetSiblingIndex() - 1;
    }

    public IEnumerator SetCreatureOptionCoroutine(CreatureSO creature, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        NewGameSelectCreatureUI.Instance.creatureSelectOptionMenu.SetActive(true);
        Icon.sprite = creature.creaturePlayerIcon;
        CreatureText.text = creature.creatureName;
        PlusSignGameObject.gameObject.SetActive(false);
        CreatureIconGameObject.gameObject.SetActive(true);
    }
}
