using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;

public class GameHandler : MonoBehaviour
{
    public int score = 0;

    public MapGeneration mapGeneration;

    public GameObject ShopUI;
    GameObject UI;
    public void GameOver(float moneyAmount)
    {
        CalculateScore(moneyAmount);
        UI = ShopUI;
        Invoke(nameof(ShowUI), 1f);
        //Scoreboard UI
    }
    void ShowUI()
    {
        UI.SetActive(true);
    }
    public void TimesUp()
    {
        UI = ShopUI;
        Invoke(nameof(ShowUI), 1f);
    }

    public void CalculateScore(float moneyAmount)
    {
        score += mapGeneration.CheckScore((int)moneyAmount);
    }
}
