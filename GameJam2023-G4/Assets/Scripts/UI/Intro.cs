using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    void Start()
    {
        SetVideo();
        StartCoroutine(WaitForIntroEnd());
    }

    void SetVideo()
    {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Intro.mp4");
        videoPlayer.Play();
    }

    public IEnumerator WaitForIntroEnd()
    {
        while (videoPlayer.isPlaying)
        {
            yield return new WaitForEndOfFrame();

        }
        StartGame();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
