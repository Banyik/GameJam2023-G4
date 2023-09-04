using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;
using Player;

namespace NPCs
{
    public class Kid : NPC
    {
        float targetSwitchTimeScale = 3f;
        float targetSwtichTimer = 0f;

        float avoidCoolDownScale = 2f;
        float avoidCoolDown = 0f;
        
        bool saw;
        PlayerNPCHandler handler;

        bool isInMainMenu;

        public Kid(int speed, State state, WalkableGrid walkableGrid, bool isMoving, bool coolDown, PlayerNPCHandler handler, bool isInMainMenu) : base(speed, state, walkableGrid, isMoving, coolDown)
        {
            this.handler = handler;
            this.isInMainMenu = isInMainMenu;
        }

        public override void CalculateCoolDown()
        {
            if (avoidCoolDown < avoidCoolDownScale)
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
            IsMoving = true;
            animator.SetBool("IsMoving", true);
            if(Random.Range(0, 100) < 10)
            {
                TargetPosition = new Vector2Int(30, 2);
                targetSwitchTimeScale = 15;
            }
            else
            {
                if (!isInMainMenu)
                {
                    TargetPosition = WalkableGrid.GetRandomCoordinate();
                    TargetPosition += new Vector2Int(0, 2);
                }
                else
                {
                    TargetPosition = new Vector2Int(Random.Range(-10, 10), Random.Range(-3, -5));
                }
                targetSwitchTimeScale = 3;
            }
            
        }

        public override void IsTargetReached(Vector2 position, Animator animator)
        {
            if (Vector3.Distance((Vector2)TargetPosition, position) <= 0.5f)
            {
                targetSwtichTimer = 0;
                GetNewState(animator);
            }
            else if (targetSwtichTimer < targetSwitchTimeScale)
            {
                targetSwtichTimer += Time.deltaTime;
            }
            else
            {
                targetSwtichTimer = 0;
                GetNewState(animator);
            }
        }

        public override void See(Animator animator)
        {
            if (!saw)
            {
                saw = true;
                IsMoving = false;
                animator.SetBool("Saw", true);
                animator.SetBool("IsMoving", false);
                if (handler.IsPlayerAvoidingStun(false))
                {
                    CoolDown = true;
                }
            }
            
        }

        public override void Stun(Animator animator)
        {
            saw = false;
            GetNewState(animator);
        }
    }
}

