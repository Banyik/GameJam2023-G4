using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirstBarBehaviour : MonoBehaviour
{
    float speed;
    int index = 0;
    public Sprite[] frames;
    private void Start()
    {

    }
    public void Animate(float currentTime)
    {
        index = (int)(frames.Length * (currentTime / speed));
        if(index >= 16)
        {
            index--;
        }
        GetComponent<SpriteRenderer>().sprite = frames[index];
    }

    public void SetAnimationSpeed(float maxThirst)
    {
        index = frames.Length;
        speed = maxThirst;
        gameObject.transform.localScale = new Vector3(1, maxThirst / 10, 0);
    }

    public void StopAnimation()
    {
        index = frames.Length;
        GetComponent<SpriteRenderer>().sprite = null;
    }
}
