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
        void FixedUpdate()
        {
            if (!handler.IsPaused())
            {
                CheckState();
                CheckForTowel();
                if (IsPlayerAbleToMove())
                {
                    CheckMovement();
                }
            }
        }
        private void Update()
        {
            if (!IsGameOver() && !handler.IsPaused())
            {
                CheckAction();
                CheckAnimationSwitch();
            }

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
                if (!handler.IsPaused())
                {
                    player.Thirst -= 0.05f;
                    thirstBarBehaviour.Animate(player.Thirst);
                    if (player.Thirst <= 0)
                    {
                        ChangeState(State.Idle);
                        StopMovement();
                        handler.TimesUp(player.Money);
                    }
                }
                yield return new WaitForSeconds(1);
            }
        }
        

        bool IsPlayerAbleToMove()
        {
            return (player.IsState(State.Idle) || player.IsState(State.Run)) && player.Thirst > 0;
        }

        bool IsStealingAnimationPlaying()
        {
            return IsAnimationPlaying("Stealing") || IsAnimationPlaying("StealingTop");
        }

        public bool IsGameOver()
        {
            return player.IsState(State.Caught) || player.Thirst <= 0;
        }

        public bool IsPlayerStealing()
        {
            return player.IsState(State.StealingStart) || player.IsState(State.Stealing);
        }

        bool HasUnlootedTowelNearby()
        {
            return closestTowel != null && !closestTowel.IsLooted;
        }
        
        void CheckAction()
        {
            if (Input.GetButtonDown("Use") && player.IsState(State.Idle) && HasUnlootedTowelNearby())
            {
                ChangeState(State.StealingStart);
            }
            if (Input.GetButtonUp("Use") && IsPlayerStealing())
            {
                lootBarBehaviour.StopAnimation();
                ChangeState(State.Idle);
                DisableAnimationBool("IsStealing");
                stealTimeCount = 0;
            }
            if (Input.GetButtonDown("Cancel"))
            {
                handler.Pause();
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
        void StopMovement()
        {
            rb.velocity = Vector2.zero;
        }

        public void ChangeState(State state)
        {
            if(!player.IsState(state) && (!player.IsState(State.Caught) || reset))
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
                    StartIdle();
                    break;
                case State.Run:
                    StartRun();
                    break;
                case State.StealingStart:
                    StartStealing();
                    break;
                case State.Stealing:
                    StealingCountDown();
                    break;
                case State.Stunned:
                    StunnedCountDown();
                    break;
                case State.StunnedStart:
                    StartStun();
                    break;
                case State.StunnedEnd:
                    ChangeState(State.Idle);
                    break;
                case State.Caught:
                    Caught();
                    break;
                default:
                    break;
            }
        }
        void StartIdle()
        {
            DisableAnimationBool("IsMoving");
        }
        void StartRun()
        {
            EnableAnimationBool("IsMoving");
            lootEffect.Stop();
        }
        void StartStealing()
        {
            SetDifficulty(closestTowel.IsBigTowel);
            DisableAnimationBool("IsMoving");
            EnableAnimationBool("IsStealing");
        }
        void StartStun()
        {
            lootBarBehaviour.StopAnimation();
            StopMovement();
            stealTimeCount = 0;
            lootEffect.Stop();
            DisableAnimationBool("IsMoving");
            DisableAnimationBool("IsStealing");
            EnableAnimationBool("IsStunned");
            player.Speed = 2;
        }
        void Caught()
        {
            DisableAnimationBool("Default");
            lootBarBehaviour.StopAnimation();
            StopMovement();
            lootEffect.Stop();
            EnableAnimationBool("Caught");
            handler.GameOver(player.Money);
        }
        public void ResetState()
        {
            reset = true;
            ChangeState(State.Idle);
            reset = false;
            EnableAnimationBool("Default");
            DisableAnimationBool("Caught");
            DisableAnimationBool("IsMoving");
            DisableAnimationBool("IsStealing");
            player.Thirst = player.MaxThirst;
            thirstBarBehaviour.SetAnimationSpeed(player.MaxThirst);
            thirstBarBehaviour.Animate(player.Thirst);
            fasterLoot = false;
            player.Speed = 2;
            stealTimeCount = 0;
            stunTimeCount = 0;
            StopMovement();
        }

        void StunnedCountDown()
        {
            if (stunTimeCount <= stunTime)
            {
                stunTimeCount += Time.deltaTime;
            }
            else
            {
                DisableAnimationBool("IsStunned");
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
                DisableAnimationBool("IsStealing");
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

        void UseItem(int index)
        {
            if (player.Items[index] != null)
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
                if (IsAnimationPlaying("Stunned"))
                {
                    ChangeState(State.Stunned);
                }
            }
        }

        bool IsAnimationPlaying(string name)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
        }

        void EnableAnimationBool(string name)
        {
            animator.SetBool(name, true);
        }
        void DisableAnimationBool(string name)
        {
            animator.SetBool(name, false);
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

