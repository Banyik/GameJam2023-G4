using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreboardHandler : MonoBehaviour
{
    public GameObject[] texts;
    public GameObject ContinueButton;
    public GameObject ContinueExitButton;
    public Text[] scores;
    int[] scoresText;
    int index = 0;
    public void ShowScoreboard(int[] scoresText)
    {
        this.scoresText = scoresText;
        Invoke(nameof(ShowElements), 3f);
    }

    void ShowElements()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            scores[i].text = scoresText[i].ToString();
            Invoke(nameof(ShowText), i + 0.25f);
        }
        Invoke(nameof(ShowContinueButton), texts.Length + 0.25f);
        index = 0;
    }

    void ShowContinueButton()
    {
        if(scoresText[0] == 4000 && scoresText[0] <= scoresText[1])
        {
            ContinueExitButton.SetActive(true);
        }
        else
        {
            ContinueButton.SetActive(true);
        }
    }

    void ShowText()
    {
        GameObject.Find("ButtonSound").GetComponent<SoundEffectHandler>().PlaySound(3);
        texts[index++].SetActive(true);
    }
}
