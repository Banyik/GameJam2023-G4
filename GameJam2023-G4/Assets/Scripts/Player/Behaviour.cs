using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;

namespace Player
{
    public class Behaviour : MonoBehaviour
    {
        public PlayerBase player;
        public TileSpawn tileSpawner;
        public Grid grid;
        public float speed = 1;
        public Animator animator;
        public float stealTime = 5;
        public float stunTime = 5;
        public float maxThirst = 10;
        public float money = 0;
        float stealTimeCount = 0;
        float stunTimeCount = 0;
        public Rigidbody2D rb;
        Towel closestTowel = null;
        public ParticleSystem lootEffect;
        public LootBarBehaviour lootBarBehaviour;
        void Start()
        {
            player = new PlayerBase(speed, maxThirst, maxThirst, money, State.Idle);
            StartCoroutine(IncreaseThirst());
        }

        IEnumerator IncreaseThirst()
        {
            while (true)
            {
                player.Thirst -= 0.05f;
                //Debug.Log($"Thirst: {player.Thirst}");
                yield return new WaitForSeconds(1);
            }
        }
        void FixedUpdate()
        {
            CheckState();
            CheckForTowel();
            if(!player.IsState(State.Stealing) && !player.IsState(State.StealingStart) &&
                !player.IsState(State.StunnedStart) && !player.IsState(State.Stunned) &&
                !player.IsState(State.StunnedEnd))
            {
                CheckMovement();
            }
        }
        private void Update()
        {
            CheckAction();
            CheckAnimationSwitch();
        }

        void CheckAnimationSwitch()
        {
            if (player.IsState(State.StealingStart))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Stealing") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("StealingTop"))
                {
                    lootBarBehaviour.SetAnimationSpeed(stealTime);
                    ChangeState(State.Stealing);
                    lootEffect.Play();
                }
            }
            if (player.IsState(State.StunnedStart))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Stunned"))
                {
                    ChangeState(State.Stunned);
                }
            }
        }
        void CheckAction()
        {
            if (Input.GetButtonDown("Use") && player.IsState(State.Idle) && (closestTowel != null && !closestTowel.IsLooted))
            {
                ChangeState(State.StealingStart);
            }
            if (Input.GetButtonUp("Use") && (player.IsState(State.StealingStart) || player.IsState(State.Stealing)))
            {
                lootBarBehaviour.StopAnimation();
                ChangeState(State.Idle);
                animator.SetBool("IsStealing", false);
                stealTimeCount = 0;
            }
        }

        void StopMovement()
        {
            rb.velocity = Vector2.zero;
        }

        void CheckMovement()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector2 movement = new Vector2(horizontal, vertical);
            rb.velocity = Vector2.Lerp(rb.velocity, movement, player.Speed);
            if(rb.velocity != new Vector2(0,0))
            {
                if(horizontal != 0)
                {
                    GetComponent<SpriteRenderer>().flipX = horizontal == -1;
                }
                ChangeState(State.Run);
            }
            else
            {
                lootEffect.Stop();
                ChangeState(State.Idle);
            }
        }

        public void ChangeState(State state)
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
                    break;
                case State.Stealing:
                    StealingCountDown();
                    break;
                case State.Stunned:
                    StunnedCountDown();
                    break;
                case State.StunnedStart:
                    lootBarBehaviour.StopAnimation();
                    StopMovement();
                    stealTimeCount = 0;
                    lootEffect.Stop();
                    animator.SetBool("IsMoving", false);
                    animator.SetBool("IsStealing", false);
                    animator.SetBool("IsStunned", true);
                    break;
                case State.StunnedEnd:
                    ChangeState(State.Idle);
                    break;
                case State.Caught:
                    break;
                default:
                    break;
            }
        }

        void StunnedCountDown()
        {
            if (stunTimeCount <= stunTime)
            {
                stunTimeCount += Time.deltaTime;
            }
            else
            {
                animator.SetBool("IsStunned", false);
                stunTimeCount = 0;
                player.State = State.StunnedEnd;
            }
        }


        void StealingCountDown()
        {
            if (stealTimeCount <= stealTime)
            {
                stealTimeCount += Time.deltaTime;
                lootBarBehaviour.Animate(stealTimeCount);
            }
            else
            {
                lootBarBehaviour.StopAnimation();
                lootEffect.Stop();
                closestTowel.LootTowel();
                stealTimeCount = 0;
                player.State = State.Idle;
                animator.SetBool("IsStealing", false);
                GetLoot();
            }
        }
        void GetLoot()
        {
            foreach (var item in closestTowel.Loot.Items)
            {
                switch (item.Type)
                {
                    case Items.ItemType.Water:
                        player.Thirst += item.Amount;
                        break;
                    case Items.ItemType.Money:
                        player.Money += item.Amount;
                        break;
                    default:
                        break;
                }
            }
            Debug.Log($"Thirst: {player.Thirst}\nMoney: {player.Money}");
        }

        void CheckForTowel()
        {
            closestTowel = null;
            RaycastHit2D[] hits = new RaycastHit2D[4];
            Vector2 rayStart = new Vector2(rb.transform.position.x, rb.transform.position.y + 0.15f);
            for (int i = 0; i < 4; i++)
            {
                float rayLength = 0.75f;
                if (i % 2 == 0)
                {
                    rayLength = 0.15f;
                }
                Vector2 rayDirection = Quaternion.AngleAxis(i * 90f, Vector3.forward) * Vector2.up;
                hits[i] = Physics2D.Raycast(rayStart, rayDirection, rayLength, LayerMask.GetMask("Towels"));
                Debug.DrawRay(rayStart, rayDirection * rayLength, Color.red);
                if(hits[i].collider != null)
                {
                    if(i%2 == 0)
                    {
                        if (i == 2)
                        {
                            closestTowel = tileSpawner.SearchTowel((Vector2Int)grid.WorldToCell((Vector2)transform.position - hits[i].normal) + new Vector2Int(0, 1));
                            animator.SetBool("OnTop", true);
                        }
                        if (i == 0)
                        {
                            closestTowel = tileSpawner.SearchTowel((Vector2Int)grid.WorldToCell((Vector2)transform.position - hits[i].normal));
                            animator.SetBool("OnTop", false);
                        }
                    }
                    else
                    {
                        closestTowel = tileSpawner.SearchTowel((Vector2Int)grid.WorldToCell((Vector2)transform.position - hits[i].normal));
                    }
                }
            }

        }
    }
}

