using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectHandler : MonoBehaviour
{
    public AudioSource audioSrc;

    public AudioClip[] clips;
    public void PlaySound(int index)
    {
        audioSrc.clip = clips[index];
        audioSrc.Play();
    }

    public void PlaySoundWithDelay(string data)
    {
        int index = System.Convert.ToInt32(data.Split(';')[0]);
        float delay = float.Parse(data.Split(';')[1]);
        audioSrc.clip = clips[index];
        audioSrc.PlayDelayed(delay);
    }
}
