using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class LootBarBehaviour : MonoBehaviour
    {
        float speed;
        int index = 0;
        public Sprite[] frames;
        private void Start()
        {
            StopAnimation();
        }
        public void Animate(float currentTime)
        {
            GetComponent<SpriteRenderer>().sprite = frames[index];
            index = (int)(((currentTime / speed) / frames.Length)*80);
        }

        public void SetAnimationSpeed(float lootTime)
        {
            speed = lootTime;
        }

        public void StopAnimation()
        {
            index = 0;
            GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}

