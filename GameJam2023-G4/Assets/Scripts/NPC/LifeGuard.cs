using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;
using Player;

namespace NPCs
{
    public class LifeGuard : NPC
    {
        bool onLeft = false;
        bool saw = false;
        float avoidCoolDownScale = 2f;
        float avoidCoolDown = 0f;
        PlayerNPCHandler handler;
        SoundEffectHandler soundEffect;
        public LifeGuard(int speed, State state, WalkableGrid walkableGrid, bool isMoving, bool coolDown, PlayerNPCHandler handler, SoundEffectHandler soundEffect) : base(speed, state, walkableGrid, isMoving, coolDown)
        {
            this.handler = handler;
            this.soundEffect = soundEffect;
        }

        public override void CalculateCoolDown()
        {
            if(avoidCoolDown < avoidCoolDownScale)
            {
                avoidCoolDown += Time.deltaTime;
            }
            else
            {
                CoolDown = false;
                avoidCoolDown = 0;
            }
        }

        public override void GetNewState(Animator animator)
        {
            ChangeState(State.Move);
            animator.SetBool("IsWalking", true);
            IsMoving = true;
            if (onLeft)
            {
                TargetPosition = new Vector2Int(15, -1);
            }
            else
            {
                TargetPosition = new Vector2Int(-15, -1);
            }
        }

        public override void StartChase(Animator animator)
        {
            IsMoving = true;
            Speed = 6;
            TargetPosition = Vector2Int.RoundToInt(GameObject.Find("Player").transform.position);
        }

        public override void See(Animator animator)
        {
            IsMoving = false;
            soundEffect.PlaySound(0);
            if (!saw)
            {
                animator.SetBool("IsSeeing", true);
                animator.SetBool("IsWalking", false);
                saw = true;
            }
        }

        public override void IsTargetReached(Vector2 position, Animator animator)
        {
            if (Vector3.Distance((Vector2)TargetPosition, position) <= .1f && !IsState(State.Chase))
            {
                onLeft = !onLeft;
                ChangeState(State.Calm);
                GetNewState(animator);
            }
            else if (Vector3.Distance((Vector2)TargetPosition, position) <= 1f && IsState(State.Chase))
            {
                IsMoving = false;
                ChangeState(State.Stun);
                animator.SetBool("IsRunning", false);
                soundEffect.PlaySound(3);
                if (!handler.IsPlayerAvoidingStun(true))
                {
                    handler.StunPlayer();
                }
                else
                {
                    CoolDown = true;
                    ChangeState(State.Calm);
                }
                Speed = 1;
                saw = false;
            }
        }
    }
}

