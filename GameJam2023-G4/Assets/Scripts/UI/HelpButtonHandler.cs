using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Video;

public class HelpButtonHandler : MonoBehaviour
{
    public Button[] buttons;
    int buttonIndex = 0;
    public Sprite selectedSprite;
    public string[] videoClips;
    public VideoPlayer videoPlayer;
    private void Start()
    {
        SetVideo();
    }
    private void Update()
    {
        SetSprite(buttonIndex, selectedSprite);
    }
    void SetVideo()
    {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoClips[buttonIndex]);
        videoPlayer.Play();
    }
    void SetSprite(int index, Sprite sprite)
    {
        buttons[index].gameObject.transform.Find("HoverSprite").GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void ChangeIndex(int index)
    {
        SetSprite(buttonIndex, null);
        buttonIndex = index;
        SetVideo();
    }

    public void NextButton()
    {
        SetSprite(buttonIndex, null);
        if(buttonIndex == buttons.Length-1)
        {
            buttonIndex = 0;
        }
        else
        {
            buttonIndex++;
        }
        SetVideo();
    }

    public void PreviousButton()
    {
        SetSprite(buttonIndex, null);
        if (buttonIndex == 0)
        {
            buttonIndex = buttons.Length-1;
        }
        else
        {
            buttonIndex--;
        }
        SetVideo();
    }
}
