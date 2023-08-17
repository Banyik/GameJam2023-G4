using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public int score = 0;
    public void GameOver(int mapCount, int moneyAmount)
    {
        Invoke(nameof(StopTime), 0.22f);
        CalculateScore(mapCount, moneyAmount);
        //Scoreboard UI
    }
    public void StopTime()
    {
        Time.timeScale = 0;
    }
    public void TimesUp()
    {
        StopTime();
        //Shop UI
    }

    public void CalculateScore(int mapCount, int moneyAmount)
    {
        score += (mapCount * moneyAmount) + mapCount;
    }
}
