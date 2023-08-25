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
    public GameObject GameOverUI;
    public GameObject NoMoneyUI;
    GameObject UI;

    bool isPaused = false;

    public bool IsPaused { get => isPaused; set => isPaused = value; }

    public void GameOver(float moneyAmount)
    {
        if (!isActivated)
        {
            isActivated = true;
            UI = GameOverUI;
            Invoke(nameof(ShowUI), 1f);
        }
    }
    void ShowUI()
    {
        UI.SetActive(true);
    }
    public void TimesUp(float moneyAmount)
    {
        GetScore(moneyAmount);
        if (!isActivated)
        {
            isActivated = true;
            if (mapGeneration.HasEnoughScore(score))
            {
                CalculateScore(moneyAmount);
                UI = ShopUI;
            }
            else
            {
                UI = NoMoneyUI;
            }
            score = 0;
            Invoke(nameof(ShowUI), 1f);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void ResetIsActivatedBool()
    {
        isActivated = false;
    }

    public void GetScore(float moneyAmount)
    {
        score = (int)moneyAmount;
    }

    public void CalculateScore(float moneyAmount)
    {
        buyScore += mapGeneration.CheckScore((int)moneyAmount);
    }
}
