using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;

namespace Player
{
    public class Behaviour : MonoBehaviour
    {
        PlayerBase player;
        public TileSpawn tileSpawner;
        public float speed = 1;
        public Animator animator;
        public float stealTime = 5;
        public float maxThirst = 10;
        float count = 0;
        public Rigidbody2D rb;
        Towel closestTowel = null;

        void Start()
        {
            player = new PlayerBase(speed, maxThirst, maxThirst, State.Idle);
            StartCoroutine(IncreaseThirst());
        }

        IEnumerator IncreaseThirst()
        {
            while (true)
            {
                player.Thirst -= 0.25f;
                Debug.Log($"Thirst: {player.Thirst}");
                yield return new WaitForSeconds(1);
            }
        }
        void FixedUpdate()
        {
            CheckState();
            CheckForTowel();
            if(!player.IsState(State.Stealing) && !player.IsState(State.StealingStart))
            {
                CheckMovement();
            }
        }
        private void Update()
        {
            CheckAction();
        }
        void CheckAction()
        {
            if (Input.GetButtonDown("Use") && player.IsState(State.Idle))
            {
                ChangeState(State.StealingStart);
            }
            if (Input.GetButtonUp("Use") && (player.IsState(State.StealingStart) || player.IsState(State.Stealing)))
            {
                ChangeState(State.Idle);
                animator.SetBool("IsStealing", false);
                count = 0;
            }
        }

        void CheckMovement()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector2 movement = new Vector2(horizontal, vertical);
            rb.velocity = Vector2.Lerp(rb.velocity, movement, player.Speed);
            if(rb.velocity != new Vector2(0,0))
            {
                animator.SetFloat("Direction", horizontal);
                ChangeState(State.Run);
            }
            else
            {
                ChangeState(State.Idle);
            }
        }

        void ChangeState(State state)
        {
            if(!player.IsState(state))
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
                    animator.SetBool("IsMoving", false);
                    animator.SetBool("IsStealing", true);
                    if (player.IsState(State.StealingStart) && (animator.GetCurrentAnimatorStateInfo(0).IsName("StealingLeft") ||
                                                          animator.GetCurrentAnimatorStateInfo(0).IsName("StealingRight")))
                    {

                        ChangeState(State.Stealing);
                    }
                    break;
                case State.Stealing:
                    if(count <= stealTime)
                    {
                        Debug.Log(count);
                        count += Time.deltaTime;
                    }
                    else
                    {
                        count = 0;
                        player.State = State.Idle;
                        animator.SetBool("IsStealing", false);
                    }
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

        void CheckForTowel()
        {
            closestTowel = null;
            RaycastHit2D[] hits = new RaycastHit2D[4];
            Vector2 rayStart = new Vector2(rb.transform.position.x, rb.transform.position.y);
            for (int i = 0; i < 4; i++)
            {
                Vector2 rayDirection = Quaternion.AngleAxis(i * 90f, Vector3.forward) * Vector2.up;
                hits[i] = Physics2D.Raycast(rayStart, rayDirection, 0.5f);
                Debug.DrawRay(rayStart, rayDirection * 0.5f, Color.red);
                if(hits[i].collider != null)
                {
                    //Debug.Log(hits[i].normal);
                    Debug.Log(Vector2Int.RoundToInt((Vector2)transform.position.normalized + hits[i].normal));
                    closestTowel = tileSpawner.SearchTowel(Vector2Int.RoundToInt(rb.position.normalized + hits[i].normal));
                    Debug.Log(closestTowel == null);
                }
            }

        }
    }
}

