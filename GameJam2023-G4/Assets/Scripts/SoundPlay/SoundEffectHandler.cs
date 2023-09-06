using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectHandler : MonoBehaviour
{
    public AudioSource audioSrc;

    public AudioClip[] clips;

    public bool isInMainMenu;
    public void PlaySound(int index)
    {
        if (!isInMainMenu)
        {
            audioSrc.clip = clips[index];
            audioSrc.Play();
        }
    }

    public void PlaySoundWithDelay(string data)
    {
        if (!isInMainMenu)
        { 
            int index = System.Convert.ToInt32(data.Split(';')[0]);
            float delay = float.Parse(data.Split(';')[1]);
            audioSrc.clip = clips[index];
            audioSrc.PlayDelayed(delay);
        }
    }
}
