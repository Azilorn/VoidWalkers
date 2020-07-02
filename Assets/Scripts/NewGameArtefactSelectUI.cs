using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewGameArtefactSelectUI : MonoBehaviour
{
    private RelicSO relic;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI relicName;
    [SerializeField] private TextMeshProUGUI relicDescription;
    [SerializeField] private GameObject plusSignGameObject;
    [SerializeField] private GameObject IconContainer;

    private void Start()
    {
        icon.gameObject.SetActive(false);
        plusSignGameObject.SetActive(true);
        IconContainer.SetActive(false);
        relicName.text = "";
        relicDescription.text = "";
    }

    public IEnumerator SetArtefactUI(float delay, RelicSO r) {

        yield return new WaitForSeconds(delay);
        relic = r;
        icon.sprite = relic.icon;
        icon.gameObject.SetActive(true);
        IconContainer.SetActive(true);
        relicName.text = relic.relicName;
        relicDescription.text = relic.relicDescription;
        plusSignGameObject.SetActive(false);
        NewGameSelectCreatureUI.Instance.creatureSelectOptionMenu.SetActive(true);

    }
}
