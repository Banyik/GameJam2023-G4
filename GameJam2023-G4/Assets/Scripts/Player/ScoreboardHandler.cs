using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreboardHandler : MonoBehaviour
{
    public GameObject[] texts;
    public GameObject ContinueButton;
    public Text[] scores;
    int[] scoresText;
    int index = 0;
    public void ShowScoreboard(int[] scoresText)
    {
        this.scoresText = scoresText;
        Invoke(nameof(ShowElements), 1f);
    }

    void ShowElements()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            scores[i].text = scoresText[i].ToString();
            Invoke(nameof(ShowText), i + 0.25f);
        }
        Invoke(nameof(ShowContinueButton), texts.Length + 0.25f);
    }

    void ShowContinueButton()
    {
        ContinueButton.SetActive(true);
    }

    void ShowText()
    {
        texts[index++].SetActive(true);
    }
}
