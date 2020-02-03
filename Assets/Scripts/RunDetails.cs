using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class RunDetails : MonoBehaviour
{
   [SerializeField] private Animator anim;
   [SerializeField] private GameObject detailsContent;
   [SerializeField] private GameObject exitButton;
   [SerializeField] private List<TextMeshProUGUI> textNumbers = new List<TextMeshProUGUI>();

    private void OnEnable()
    {
        anim.SetTrigger("moveup");
        var cg = detailsContent.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.DOFade(1, 2f).SetDelay(0.75f);
        exitButton.transform.localScale = Vector3.zero;
        exitButton.transform.DOScale(1, 0.35f).SetDelay(1f);
        SetDetails();
    }

    public void SetDetails() {

        //RoutesTaken
        textNumbers[0].text = CoreGameInformation.currentRunDetails.RoutesTaken.ToString();
        //Battles Won
        textNumbers[1].text = CoreGameInformation.currentRunDetails.BattlesWon.ToString();
        //Void Walkers Defeated
        textNumbers[2].text = CoreGameInformation.currentRunDetails.VoidWalkersDefeated.ToString();
        //Bosses Defeated
        textNumbers[3].text = CoreGameInformation.currentRunDetails.BossesDefeated.ToString();
        //Retries
        textNumbers[4].text = CoreGameInformation.currentRunDetails.Retries.ToString();
        //Gold Made
        textNumbers[5].text = CoreGameInformation.currentRunDetails.GoldMade.ToString();
        //Void Walkers Fainted
        textNumbers[6].text = CoreGameInformation.currentRunDetails.VoidWalkersFainted.ToString();
        //ItemsUsed
        textNumbers[7].text = CoreGameInformation.currentRunDetails.ItemsUsed.ToString();
        //RelicsObtained
        textNumbers[8].text = CoreGameInformation.currentRunDetails.RelicsObtained.ToString();
        //ItemsObtained
        textNumbers[9].text = CoreGameInformation.currentRunDetails.ItemsObtained.ToString();
        //AbilitiesObtained
        textNumbers[10].text = CoreGameInformation.currentRunDetails.AbilitiesObtained.ToString();
        //GoldSpent
        textNumbers[11].text = CoreGameInformation.currentRunDetails.GoldSpent.ToString();

    }
}
