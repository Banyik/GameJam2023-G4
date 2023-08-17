using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public int score = 0;
    public void GameOver(int mapCount, int moneyAmount)
    {
        Time.timeScale = 0;
        CalculateScore(mapCount, moneyAmount);
        //Scoreboard UI
    }
    public void TimesUp()
    {
        Time.timeScale = 0;
        //Shop UI
    }

    public void CalculateScore(int mapCount, int moneyAmount)
    {
        score += (mapCount * moneyAmount) + mapCount;
    }
}
