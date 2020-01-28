using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class CreatureEvolutionUI : MonoBehaviour
{

    public Image icon;

    public void EvolveCreature() {
        StartCoroutine(EvolveCreatureCoroutine(BattleController.Instance.MasterPlayerParty));
    }
    public IEnumerator EvolveCreatureCoroutine(PlayerParty party) {

        for (int i = 0; i < party.party.Length; i++) {

            for (int j = 0; j < party.party[i].creatureSO.evolutions.Count; j++)
            {
                if (party.party[i].creatureSO == party.party[i].creatureSO.evolutions[j].evolutionSO)
                    continue;
                 else if (party.party[i].creatureStats.level >= party.party[i].creatureSO.evolutions[j].levelRequirement) {

                    MenuTransitionsController.Instance.StartTransition(2, false);
                    yield return new WaitForSeconds(0.3f);
                    icon.sprite = party.party[i].creatureSO.creaturePlayerIcon;
                    if(!gameObject.activeInHierarchy)
                        gameObject.SetActive(true);
                    yield return new WaitForSeconds(1f);
                    yield return StartCoroutine(EvolveCreatureAnimation(party.party[i].creatureSO.creaturePlayerIcon, party.party[i].creatureSO.evolutions[j].evolutionSO.creaturePlayerIcon));
                    party.party[i].creatureSO = party.party[i].creatureSO.evolutions[j].evolutionSO;
                    party.party[i].SetLevel(party.party[i].creatureStats.level, true);
                }
            }

        }
        MenuTransitionsController.Instance.StartTransition(0, false);
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }

    private IEnumerator EvolveCreatureAnimation(Sprite currentSprite, Sprite newSprite)
    {
        icon.sprite = currentSprite;
        icon.DOColor(Color.black, 1f);
        yield return new WaitForSeconds(1f);
        icon.transform.DOShakeScale(5, 0.25f, 2, 10, true);
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(0.25f);
            icon.sprite = currentSprite;
            yield return new WaitForSeconds(0.25f);
            icon.sprite = newSprite;
        }
       
        yield return new WaitForSeconds(0.5f);
        icon.DOColor(Color.white, 0.5f);

        yield return new WaitForSeconds(3f);
    }
}
