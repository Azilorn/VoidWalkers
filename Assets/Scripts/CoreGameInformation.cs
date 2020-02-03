using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreGameInformation
{

    //This Class can only be Static
    public static bool isLoadedGame = true;
    public static bool isRetry = false;
    public static int currentSavedSeed;
    public static int currentXPEarned;
    public static int currentLVL;
    public static CurrentRunDetails currentRunDetails;

    public static void SetGameLoadState(bool loadedGame)
    {
        isLoadedGame = loadedGame;
    }

    public static void SetGameIsNew()
    {
        PreBattleSelectionController.Instance.GameDetails.ProgressOnCurrentFloor = 1;
        PreBattleSelectionController.Instance.GameDetails.Floor = 1;
        PreBattleSelectionController.Instance.GameDetails.Gold = 100;
        UnityEngine.Random.InitState((int)DateTime.Now.ToBinary());
        currentRunDetails = new CurrentRunDetails();
    }
    public static void AddToRetries()
    {
       currentRunDetails.Retries++;
    }
}
[Serializable]
public class CurrentRunDetails {

    public int RoutesTaken;
    public int BattlesWon;
    public int VoidWalkersDefeated;
    public int BossesDefeated;
    public int Retries;
    public int GoldMade;
    public int VoidWalkersFainted;
    public int ItemsUsed;
    public int RelicsObtained;
    public int ItemsObtained;
    public int AbilitiesObtained;
    public int GoldSpent;
}
