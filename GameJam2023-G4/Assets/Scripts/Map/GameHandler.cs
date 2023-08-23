using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public int score = 0;
    public int buyScore = 0;
    public MapGeneration mapGeneration;
    bool isActivated = false;
    public GameObject ShopUI;
    GameObject UI;

    bool isPaused = false;

    public bool IsPaused { get => isPaused; set => isPaused = value; }

    public void GameOver(float moneyAmount)
    {
        if (!isActivated)
        {
            isActivated = true;
            CalculateScore(moneyAmount);
            UI = ShopUI;
            Invoke(nameof(ShowUI), 1f);
        }
        //Scoreboard UI
    }
    void ShowUI()
    {
        UI.SetActive(true);
    }
    public void TimesUp(float moneyAmount)
    {
        if (!isActivated)
        {
            isActivated = true;
            CalculateScore(moneyAmount);
            UI = ShopUI;
            Invoke(nameof(ShowUI), 1f);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetIsActivatedBool()
    {
        isActivated = false;
    }

    public void CalculateScore(float moneyAmount)
    {
        score = (int)moneyAmount;
        buyScore += mapGeneration.CheckScore((int)moneyAmount);
    }
}
