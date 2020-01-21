using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreGameInformation
{

    //This Class can only be Static
    public static bool isLoadedGame = true;
    public static int currentSavedSeed;
    public static int currentXPEarned;
    public static int currentLVL;

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
    }
}
