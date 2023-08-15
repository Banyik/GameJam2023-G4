using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Behaviour : MonoBehaviour
    {
        PlayerBase player;
        public float speed = 1;

        //public AnimationClip idle;
        //public AnimationClip run;
        //public AnimationClip stealingStart;
        //public AnimationClip stealing;
        //public AnimationClip stunned;
        //public AnimationClip stunnedStart;
        //public AnimationClip caught;

        public Animator animator;

        public Rigidbody2D rb;
        void Start()
        {
            player = new PlayerBase(speed, State.Idle);
        }
        void FixedUpdate()
        {
            CheckState();
            CheckMovement();
        }

        void CheckMovement()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector2 movement = new Vector2(horizontal, vertical);
            rb.velocity = Vector2.Lerp(rb.velocity, movement, player.Speed);
            if(rb.velocity != new Vector2(0,0))
            {
                ChangeState(State.Run);
            }
            else
            {
                ChangeState(State.Idle);
            }
        }

        void ChangeState(State state)
        {
            if(player.State != state)
            {
                player.State = state;
                CheckState();
            }
        }

        void CheckState()
        {
            switch (player.State)
            {
                case State.Idle:
                    animator.SetBool("IsMoving", false);
                    break;
                case State.Run:
                    animator.SetBool("IsMoving", true);
                    break;
                case State.StealingStart:
                    break;
                case State.Stealing:
                    break;
                case State.Stunned:
                    break;
                case State.StunnedStart:
                    break;
                case State.StunnedEnd:
                    break;
                case State.Caught:
                    break;
                default:
                    break;
            }
        }
    }
}

