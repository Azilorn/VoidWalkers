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
    [SerializeField] private TextMeshProUGUI finalScore;

    public TextMeshProUGUI FinalScore { get => finalScore; set => finalScore = value; }

    private void OnEnable()
    {
        anim.SetTrigger("moveup");
        var cg = detailsContent.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.DOFade(1, 2f).SetDelay(0.75f);
        finalScore.DOFade(0, 0);
        finalScore.DOFade(1, 2f).SetDelay(0.75f);
        exitButton.transform.localScale = Vector3.zero;
        exitButton.transform.DOScale(1, 0.35f).SetDelay(1f);
        SetDetails();
    }

    public void SetDetails() {

        CurrentRunDetails Run = CoreGameInformation.currentRunDetails;
        //RoutesTaken
        textNumbers[0].text = Run.RoutesTaken.ToString();
        //Battles Won
        textNumbers[1].text = Run.BattlesWon.ToString();
        //Void Walkers Defeated
        textNumbers[2].text = Run.VoidWalkersDefeated.ToString();
        //Bosses Defeated
        textNumbers[3].text = Run.BossesDefeated.ToString();
        //Retries
        textNumbers[4].text = Run.Retries.ToString();
        //Gold Made
        textNumbers[5].text = Run.GoldMade.ToString();
        //Void Walkers Fainted
        textNumbers[6].text = Run.VoidWalkersFainted.ToString();
        //ItemsUsed
        textNumbers[7].text = Run.ItemsUsed.ToString();
        //RelicsObtained
        textNumbers[8].text = Run.RelicsObtained.ToString();
        //ItemsObtained
        textNumbers[9].text = Run.ItemsObtained.ToString();
        //AbilitiesObtained
        textNumbers[10].text = Run.AbilitiesObtained.ToString();
        //GoldSpent
        textNumbers[11].text = Run.GoldSpent.ToString();

        int PlusScore = 0;
        int MinusScore = 0;

        PlusScore = PlusScore + 
            (
            (Run.RoutesTaken) +
            (Run.BattlesWon * 2) +
            (Run.VoidWalkersDefeated) +
            (Run.BossesDefeated * 10) +
            (Mathf.RoundToInt(Run.GoldMade * 0.5f)) +
            (Run.RelicsObtained * 10) +
            (Run.ItemsObtained * 3) +
            (Run.AbilitiesObtained * 5)
            );
        MinusScore = MinusScore +
            (
            (Run.Retries * 50) +
            (Run.ItemsUsed * 2) +
            (Run.VoidWalkersFainted * 3)
            );
        int FinalRunScore = PlusScore - MinusScore;
        if (FinalRunScore <= 0)
            FinalRunScore = 0;
        CoreGameInformation.currentRunDetails.FinalRunScore = FinalRunScore;

        FinalScore.text = "Final Score: " + FinalRunScore.ToString();
    }
}
