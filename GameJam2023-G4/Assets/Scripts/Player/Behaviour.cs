using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;
using Items;

namespace Player
{
    public class Behaviour : MonoBehaviour
    {
        public ThirstBarBehaviour thirstBarBehaviour;
        public PlayerBase player;
        PlayerMapHandler handler;
        public float speed = 10;
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
        public bool avoidStun = false;
        bool fasterLoot = false;
        bool reset = false;
        void Start()
        {
            player = new PlayerBase(speed, maxThirst, maxThirst, money, new Item[3], State.Idle);
            handler = GameObject.Find("ScriptHandler").GetComponent<PlayerMapHandler>();
            StartCoroutine(IncreaseThirst());
        }

        void SetDifficulty(bool isBigTowel)
        {
            int lootPowerUp = 1;
            if (fasterLoot)
            {
                lootPowerUp = 2;
            }
            if(isBigTowel)
            {
                stealTime = 6 / lootPowerUp;
            }
            else
            {
                stealTime = 3 / lootPowerUp;
            }
        }

        IEnumerator IncreaseThirst()
        {
            thirstBarBehaviour.SetAnimationSpeed(player.MaxThirst);
            while (true)
            {
                player.Thirst -= 0.05f;
                thirstBarBehaviour.Animate(player.Thirst);
                if(player.Thirst <= 0)
                {
                    ChangeState(State.Idle);
                    StopMovement();
                    handler.TimesUp(player.Money);
                }
                yield return new WaitForSeconds(1);
            }
        }
        void FixedUpdate()
        {
            CheckState();
            CheckForTowel();
            if(IsPlayerAbleToMove())
            {
                CheckMovement();
            }
        }
        private void Update()
        {
            if(!player.IsState(State.Caught) && player.Thirst > 0)
            {
                CheckAction();
                CheckAnimationSwitch();
            }
            
        }

        bool IsPlayerAbleToMove()
        {
            return (player.IsState(State.Idle) || player.IsState(State.Run)) && player.Thirst > 0;
        }

        bool IsStealingAnimationPlaying()
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("Stealing") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("StealingTop");
        }

        void CheckAnimationSwitch()
        {
            if (player.IsState(State.StealingStart))
            {
                if (IsStealingAnimationPlaying())
                {
                    lootBarBehaviour.SetAnimationSpeed(stealTime);
                    ChangeState(State.Stealing);
                    lootEffect.gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0.2f, 0);
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
            if (Input.GetButtonDown("Slot_1"))
            {
                UseItem(0);
            }
            if (Input.GetButtonDown("Slot_2"))
            {
                UseItem(1);
            }
            if (Input.GetButtonDown("Slot_3"))
            {
                UseItem(2);
            }
        }

        void UseItem(int index)
        {
            if(player.Items[index] != null)
            {
                switch (player.Items[index].Type)
                {
                    case ItemType.Corn:
                        fasterLoot = true;
                        break;
                    case ItemType.IceCream:
                        player.Speed = 4;
                        break;
                    case ItemType.Langos:
                        avoidStun = true;
                        Debug.Log(avoidStun);
                        break;
                    default:
                        break;
                }
                if (--player.Items[index].Amount == 0)
                {
                    player.Items[index] = null;
                }
                
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
            rb.velocity = Vector2.Lerp(rb.velocity, movement, player.Speed) * player.Speed;
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
            if(!player.IsState(state) && (!player.IsState(State.Caught) || reset))
            {
                player.State = state;
                CheckState();
            }
        }

        public void ResetState()
        {
            reset = true;
            ChangeState(State.Idle);
            reset = false;
            animator.SetBool("Default", true);
            animator.SetBool("Caught", false);
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsStealing", false);
            player.Thirst = player.MaxThirst;
            thirstBarBehaviour.SetAnimationSpeed(player.MaxThirst);
            thirstBarBehaviour.Animate(player.Thirst);
            fasterLoot = false;
            player.Speed = 2;
            stealTimeCount = 0;
            stunTimeCount = 0;
            StopMovement();
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
                    lootEffect.Stop();
                    break;
                case State.StealingStart:
                    SetDifficulty(closestTowel.IsBigTowel);
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
                    player.Speed = 2;
                    break;
                case State.StunnedEnd:
                    ChangeState(State.Idle);
                    break;
                case State.Caught:
                    animator.SetBool("Default", false);
                    lootBarBehaviour.StopAnimation();
                    StopMovement();
                    lootEffect.Stop();
                    animator.SetBool("Caught", true);
                    handler.GameOver(player.Money);
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
            if (stealTimeCount <= stealTime && player.Thirst > 0)
            {
                stealTimeCount += Time.deltaTime;
                lootBarBehaviour.Animate(stealTimeCount);
            }
            else if(player.Thirst > 0)
            {
                if (fasterLoot)
                {
                    fasterLoot = false;
                }
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
                    case ItemType.Water:
                        if(player.Thirst + item.Amount < player.MaxThirst)
                        {
                            player.Thirst += item.Amount;
                        }
                        else
                        {
                            player.Thirst = player.MaxThirst;
                        }
                        thirstBarBehaviour.Animate(player.Thirst);
                        break;
                    case ItemType.Money:
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
                    Vector2 position = Vector2.zero;
                    position = (Vector2)transform.position - hits[i].normal;
                    if (i % 2 == 0)
                    {
                        if (i == 2)
                        {
                            position += new Vector2Int(0, 1);
                            animator.SetBool("OnTop", true);
                        }
                        if (i == 0)
                        {
                            animator.SetBool("OnTop", false);
                        }
                    }
                    closestTowel = handler.GetTowel(position);
                }
            }

        }
    }
}

