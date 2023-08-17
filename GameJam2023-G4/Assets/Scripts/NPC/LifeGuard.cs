using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;

namespace NPCs
{
    public class LifeGuard : NPC
    {
        bool onLeft = false;
        bool saw = false;
        public LifeGuard(int speed, State state, Vector2Int currentPosition, WalkableGrid walkableGrid, bool isMoving) : base(speed, state, currentPosition, walkableGrid, isMoving)
        {
        }

        public override void GetNewState(Animator animator)
        {
            ChangeState(State.Move);
            animator.SetBool("IsWalking", true);
            IsMoving = true;
            if (onLeft)
            {
                TargetPosition = new Vector2Int(9, 0);
            }
            else
            {
                TargetPosition = new Vector2Int(-9, 0);
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
                CurrentPosition = TargetPosition;
                onLeft = !onLeft;
                ChangeState(State.Calm);
                GetNewState(animator);
            }
            else if (Vector3.Distance((Vector2)TargetPosition, position) <= 1f && IsState(State.Chase))
            {
                IsMoving = false;
                animator.SetBool("IsRunning", false);
                GameObject.Find("Player").GetComponent<Player.Behaviour>().ChangeState(Player.State.StunnedStart);
                Speed = 1;
                ChangeState(State.Stun);
                saw = false;
            }
        }
    }
}

