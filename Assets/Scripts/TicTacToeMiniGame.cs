using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public enum TicTacToeEnum {Empty, X, O }
public class TicTacToeMiniGame : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI[] tictactoeTexts;
    public TicTacToeEnum[] tictactoeIntGrid;
    int turnCount;
    bool EnemyTurn = false;
    bool matchOver = false;
    bool matchWon = false;
    bool finishedOpening = false;
    bool timerFinished = false;
    float timer = 1f % 60;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }
    private void Start()
    {
        for (int i = 0; i < tictactoeTexts.Length; i++)
        {
            tictactoeTexts[i].text = "";
        }
        int rnd = UnityEngine.Random.Range(0, 1);

        switch (rnd)
        {

            case 0:
                EnemyTurn = false;
                break;
            case 1:
                EnemyTurn = true;
                break;
            default:
                break;
        }
        finishedOpening = false;
        transform.DOScale(1, 0.5f).onComplete += IsFinishedOpening;

    }
    public void IsFinishedOpening() {
        finishedOpening = true;
    }
    private void Update()
    {
        if (!finishedOpening) 
            return;
        if (matchOver)
            return;
        if (EnemyTurn == true) {

            for (int i = 0; i < 1; i++)
            {
                int rnd = UnityEngine.Random.Range(0, 9);
                if (tictactoeIntGrid[rnd] == TicTacToeEnum.X || tictactoeIntGrid[rnd] == TicTacToeEnum.O)
                {
                    i--;    
                    return;
                }
                else {

                    timer -= Time.deltaTime;

                    if (timer > 0)
                    {
                        timerFinished = false;
                    }
                    else timerFinished = true;

                    if (timerFinished)
                    {
                        tictactoeIntGrid[rnd] = TicTacToeEnum.O;
                        tictactoeTexts[rnd].text = "O";
                        turnCount++;
                        if (CheckWin(false) == true)
                        {
                            matchOver = true;
                            matchWon = false;
                            Debug.Log("lose");
                            return;
                        }
                        if (turnCount == 9)
                        {
                            matchOver = true;
                            Debug.Log("Draw");
                        }
                        EnemyTurn = false;
                        timerFinished = false;
                        timer = 1f % 60;
                    }
                }
            }
        }

    }
    public void SetPosition(int position) {

        if (!finishedOpening)
            return;
        if (EnemyTurn == true)
            return;
        if (matchOver == true)
            return;
        if (tictactoeIntGrid[position] == TicTacToeEnum.Empty)
        {
            tictactoeIntGrid[position] = TicTacToeEnum.X;
            tictactoeTexts[position].text = "X";
            
            turnCount++;
            if (CheckWin(true) == true)
            {
                matchOver = true;
                matchWon = true;
                Debug.Log("win");
                return;
            }
            if (turnCount == 9)
            {
                matchOver = true;
                Debug.Log("Draw");
            }
            else EnemyTurn = true;
        }
        else {
            return;
        }
    }

    private bool CheckWin(bool player)
    {
        if (player)
        {
            if (tictactoeIntGrid[0] == TicTacToeEnum.X && tictactoeIntGrid[1] == TicTacToeEnum.X && tictactoeIntGrid[2] == TicTacToeEnum.X)
            {
                return true;
            }
            else if (tictactoeIntGrid[3] == TicTacToeEnum.X && tictactoeIntGrid[4] == TicTacToeEnum.X && tictactoeIntGrid[5] == TicTacToeEnum.X)
            {
                return true;
            }
            else if (tictactoeIntGrid[6] == TicTacToeEnum.X && tictactoeIntGrid[7] == TicTacToeEnum.X && tictactoeIntGrid[8] == TicTacToeEnum.X)
            {
                return true;
            }
            else if (tictactoeIntGrid[0] == TicTacToeEnum.X && tictactoeIntGrid[3] == TicTacToeEnum.X && tictactoeIntGrid[6] == TicTacToeEnum.X)
            {
                return true;
            }
            else if (tictactoeIntGrid[1] == TicTacToeEnum.X && tictactoeIntGrid[4] == TicTacToeEnum.X && tictactoeIntGrid[7] == TicTacToeEnum.X)
            {
                return true;
            }
            else if (tictactoeIntGrid[2] == TicTacToeEnum.X && tictactoeIntGrid[5] == TicTacToeEnum.X && tictactoeIntGrid[8] == TicTacToeEnum.X)
            {
                return true;
            }
            else if (tictactoeIntGrid[0] == TicTacToeEnum.X && tictactoeIntGrid[4] == TicTacToeEnum.X && tictactoeIntGrid[8] == TicTacToeEnum.X)
            {
                return true;
            }
            else if (tictactoeIntGrid[2] == TicTacToeEnum.X && tictactoeIntGrid[4] == TicTacToeEnum.X && tictactoeIntGrid[6] == TicTacToeEnum.X)
            {
                return true;
            }
            else return false;
        }
        else if (!player)
        {
            if (tictactoeIntGrid[0] == TicTacToeEnum.O && tictactoeIntGrid[1] == TicTacToeEnum.O && tictactoeIntGrid[2] == TicTacToeEnum.O)
            {
                return true;
            }
            else if (tictactoeIntGrid[3] == TicTacToeEnum.O && tictactoeIntGrid[4] == TicTacToeEnum.O && tictactoeIntGrid[5] == TicTacToeEnum.O)
            {
                return true;
            }
            else if (tictactoeIntGrid[6] == TicTacToeEnum.O && tictactoeIntGrid[7] == TicTacToeEnum.O && tictactoeIntGrid[8] == TicTacToeEnum.O)
            {
                return true;
            }
            else if (tictactoeIntGrid[0] == TicTacToeEnum.O && tictactoeIntGrid[3] == TicTacToeEnum.O && tictactoeIntGrid[6] == TicTacToeEnum.O)
            {
                return true;
            }
            else if (tictactoeIntGrid[1] == TicTacToeEnum.O && tictactoeIntGrid[4] == TicTacToeEnum.O && tictactoeIntGrid[7] == TicTacToeEnum.O)
            {
                return true;
            }
            else if (tictactoeIntGrid[2] == TicTacToeEnum.O && tictactoeIntGrid[5] == TicTacToeEnum.O && tictactoeIntGrid[8] == TicTacToeEnum.O)
            {
                return true;
            }
            else if (tictactoeIntGrid[0] == TicTacToeEnum.O && tictactoeIntGrid[4] == TicTacToeEnum.O && tictactoeIntGrid[8] == TicTacToeEnum.O)
            {
                return true;
            }
            else if (tictactoeIntGrid[2] == TicTacToeEnum.O && tictactoeIntGrid[4] == TicTacToeEnum.O && tictactoeIntGrid[6] == TicTacToeEnum.O)
            {
                return true;
            }
            else return false;
        }
        else return false;
    }
}
