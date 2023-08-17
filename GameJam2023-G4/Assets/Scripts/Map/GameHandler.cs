using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public int score = 0;

    public GameObject ShopUI;
    public GameObject UI;
    public void GameOver(int mapCount, float moneyAmount)
    {
        CalculateScore(mapCount, moneyAmount);
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

    public void CalculateScore(int mapCount, float moneyAmount)
    {
        score += (int)(mapCount * moneyAmount) + mapCount;
    }
}
