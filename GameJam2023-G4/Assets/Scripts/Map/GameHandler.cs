using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public int score = 0;

    public GameObject ShopUI;
    public void GameOver(int mapCount, float moneyAmount)
    {
        CalculateScore(mapCount, moneyAmount);
        Invoke(nameof(ShopUI), 1f);
        //Scoreboard UI
    }
    void ShowUI(GameObject UI)
    {
        UI.SetActive(true);
    }
    public void TimesUp()
    {
        Invoke(nameof(ShopUI), 1f);
    }

    public void CalculateScore(int mapCount, float moneyAmount)
    {
        score += (int)(mapCount * moneyAmount) + mapCount;
    }
}
