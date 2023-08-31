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
    public GameObject ScoreUI;
    GameObject UI;
    int soundIndex;
    bool isPaused = false;

    public bool IsPaused { get => isPaused; set => isPaused = value; }

    public void GameOver(float moneyAmount)
    {
        if (!isActivated)
        {
            isActivated = true;
            UI = GameOverUI;
            soundIndex = 5;
            Invoke(nameof(ShowUI), 1f);
            Invoke(nameof(PlaySound), 1f);
        }
    }
    void PlaySound()
    {
        GameObject.Find("ButtonSound").GetComponent<SoundEffectHandler>().PlaySound(soundIndex);
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
                UI = ScoreUI;
                soundIndex = 4;
                mapGeneration.ShowScoreboardUI(score);
            }
            else
            {
                soundIndex = 5;
                UI = NoMoneyUI;
            }
            score = 0;
            Invoke(nameof(ShowUI), 1f);
            Invoke(nameof(PlaySound), 1f);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
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
