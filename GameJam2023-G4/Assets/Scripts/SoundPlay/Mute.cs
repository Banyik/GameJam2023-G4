using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class Mute : MonoBehaviour
{
    public Image imageRenderer;
    public Sprite muted;
    public Sprite unMuted;

    public AudioMixer music;
    public AudioMixer sounds;

    bool isMuted = false;

    void Start()
    {
        LoadData();
    }

    public void MuteButton()
    {
        if(!isMuted)
        {
            isMuted = true;
            imageRenderer.sprite = muted;
            MuteSounds(isMuted);
        }
        else
        {
            isMuted = false;
            imageRenderer.sprite = unMuted;
            MuteSounds(isMuted);
        }
    }

    void MuteSounds(bool mute)
    {
        if (mute)
        {
            music.SetFloat("Volume", -80);
            sounds.SetFloat("Volume", -80);
        }
        else
        {
            music.SetFloat("Volume", 0);
            sounds.SetFloat("Volume", 0);
        }
        SaveData(mute);
    }

    void SaveData(bool mute)
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "Sounds.f"))
        {
            string fullPath = System.IO.Path.Combine(Application.persistentDataPath, "Sounds.f");
            System.IO.File.WriteAllText(fullPath, mute.ToString());
        }
        else
        {
            System.IO.File.Create(Application.persistentDataPath + "Sounds.f");
            string fullPath = System.IO.Path.Combine(Application.persistentDataPath, "Sounds.f");
            System.IO.File.WriteAllText(fullPath, mute.ToString());
        }
    }
    public void LoadData()
    {
        if(System.IO.File.Exists(Application.persistentDataPath + "Sounds.f"))
        {
            string fullPath = System.IO.Path.Combine(Application.persistentDataPath, "Sounds.f");
            isMuted = !System.Convert.ToBoolean(System.IO.File.ReadAllText(fullPath));
            MuteButton();
        }
        else
        {
            isMuted = true;
            MuteButton();
        }
    }
}
