using Maps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCs
{
    public class Grandma : NPC
    {
        float waitTimeScale = 5f;
        float waitTime = 0f;
        SoundEffectHandler soundEffect;
        bool isInMainMenu;
        public Grandma(int speed, State state, WalkableGrid walkableGrid, bool isMoving, bool coolDown, SoundEffectHandler soundEffect, bool isInMainMenu) : base(speed, state, walkableGrid, isMoving, coolDown)
        {
            this.soundEffect = soundEffect;
            this.isInMainMenu = isInMainMenu;
        }

        public override void CalculateCoolDown()
        {
            base.CalculateCoolDown();
        }

        public override void GetNewState(Animator animator)
        {
            if(waitTime < waitTimeScale)
            {
                waitTime += Time.deltaTime;
            }
            else if(!isInMainMenu)
            {
                animator.SetBool("Look", true);
                soundEffect.PlaySound(2);
                ChangeState(State.See);
                waitTime = 0f;
                waitTimeScale = Random.Range(5, 9);
            }
        }

        public override void IsTargetReached(Vector2 position, Animator animator)
        {
            base.IsTargetReached(position, animator);
        }

        public override void See(Animator animator)
        {
        }

        public override void StartChase(Animator animator)
        {
            base.StartChase(animator);
        }

        public override void Stun(Animator animator)
        {
            
        }
    }
}

