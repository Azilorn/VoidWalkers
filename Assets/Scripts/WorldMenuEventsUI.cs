using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMenuEventsUI : MonoBehaviour
{
    public static WorldMenuEventsUI Instance;
    public List<GameObject> events = new List<GameObject>();

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

    public void OpenEvent() {
        StartCoroutine(OpenEventCoroutine());
    }
    public void CloseEvent()
    {
        StartCoroutine(CloseEventCoroutine());
    }
    public IEnumerator OpenEventCoroutine() {

        MenuTransitionsController.Instance.StartTransition(2, false);
        yield return new WaitForSeconds(0.3f);
        events[PreBattleSelectionController.Instance.eventInt].gameObject.SetActive(true);
    }
    public IEnumerator CloseEventCoroutine() {
        MenuTransitionsController.Instance.StartTransition(2, false);
        yield return new WaitForSeconds(0.3f);
        events[PreBattleSelectionController.Instance.eventInt].gameObject.SetActive(false);
        PreBattleSelectionController.Instance.SetFloor();
    }
}
