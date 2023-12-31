using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;
using Player;

namespace NPCs
{
    public class Behaviour : MonoBehaviour
    {
        public Type type;
        public NPC npc;
        Rigidbody2D rb;
        public float avoidanceForceMultiplier = 0.4f;
        public float raySpacing = 3f;
        public GameObject scriptHandler;
        LayerMask ignoreLayers;
        WalkableGrid walkableGrid;
        public RuntimeAnimatorController lifeGuardAnimator;
        public RuntimeAnimatorController kidAnimator;
        public RuntimeAnimatorController grandmaAnimator;
        public Animator animator;
        public GameObject lifeGuardTrigger;
        public GameObject kidTrigger;
        public NPCSpriteBehaviour spriteBehaviour;
        public Sprite lifeGuardHeadSprite;
        public Sprite kidHeadSprite;
        public GameObject waterBullet;
        PlayerNPCHandler playerNPCHandler;
        GameHandler gameHandler;
        SoundEffectHandler soundEffect;
        public bool isInMainMenu;
        public void StartNPC()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            if (!isInMainMenu)
            {
                walkableGrid = scriptHandler.GetComponent<WalkableGrid>();
                walkableGrid.GetCoordinates();
                playerNPCHandler = GameObject.Find("ScriptHandler").GetComponent<PlayerNPCHandler>();
            }
            gameHandler = GameObject.Find("ScriptHandler").GetComponent<GameHandler>();
            soundEffect = gameObject.GetComponent<SoundEffectHandler>();
            switch (type)
            {
                case Type.Kid:
                    npc = new Kid(5, State.Calm, walkableGrid, true, false, playerNPCHandler, isInMainMenu);
                    animator.runtimeAnimatorController = kidAnimator;
                    gameObject.layer = LayerMask.NameToLayer("Kid");
                    ignoreLayers += LayerMask.GetMask("PlayerWall");
                    ignoreLayers += LayerMask.GetMask("Player");
                    raySpacing = 1.4f;
                    avoidanceForceMultiplier = 10;
                    kidTrigger.SetActive(true);
                    spriteBehaviour.SetSprite(kidHeadSprite, gameObject);
                    break;
                case Type.Grandma:
                    npc = new Grandma(5, State.Calm, walkableGrid, false, false, soundEffect, isInMainMenu);
                    animator.runtimeAnimatorController = grandmaAnimator;
                    gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    gameObject.layer = LayerMask.NameToLayer("Grandma");
                    spriteBehaviour.gameObject.SetActive(false);
                    break;
                case Type.LifeGuard:
                    npc = new LifeGuard(1, State.Calm, walkableGrid, true, false, playerNPCHandler, soundEffect);
                    animator.runtimeAnimatorController = lifeGuardAnimator;
                    gameObject.layer = LayerMask.NameToLayer("LifeGuard");
                    ignoreLayers += LayerMask.GetMask("LifeGuardPath");
                    ignoreLayers += LayerMask.GetMask("PlayerWall");
                    ignoreLayers += LayerMask.GetMask("Player");
                    avoidanceForceMultiplier = 10;
                    raySpacing = 1.0f;
                    lifeGuardTrigger.SetActive(true);
                    spriteBehaviour.SetSprite(lifeGuardHeadSprite, gameObject);
                    break;
                default:
                    break;
            }
        }

        private void Update()
        {
            if (!gameHandler.IsPaused && (isInMainMenu || (playerNPCHandler != null && !playerNPCHandler.IsGameOver())))
            {
                CheckAnimationSwitch();
                CheckStates();
            }
        }

        void CheckAnimationSwitch()
        {
            switch (type)
            {
                case Type.Kid:
                    if (npc.IsState(State.See) && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") &&
                        !playerNPCHandler.IsGameOver())
                    {
                        soundEffect.PlaySound(0);
                        if (playerNPCHandler.IsPlayerOnLeftSide(gameObject) && gameObject.GetComponent<SpriteRenderer>().flipX == false)
                        {
                            gameObject.GetComponent<SpriteRenderer>().flipX = true;
                        }
                        else if (playerNPCHandler.IsPlayerOnRightSide(gameObject) && gameObject.GetComponent<SpriteRenderer>().flipX == true)
                        {
                            gameObject.GetComponent<SpriteRenderer>().flipX = false;
                        }
                        var bullet = Instantiate(waterBullet, transform.position, new Quaternion(0, 0, 0, 0), null);
                        bullet.SetActive(true);
                        bullet.GetComponent<WaterBulletBehaviour>().Shoot(transform.position, playerNPCHandler.GetPlayerPosition());
                        soundEffect.PlaySound(1);
                        animator.SetBool("Saw", false);
                        npc.ChangeState(State.Stun);
                    }
                    break;
                case Type.Grandma:
                    if (npc.IsState(State.See) && animator.GetCurrentAnimatorStateInfo(0).IsName("Looking"))
                    {
                        if (playerNPCHandler.IsPlayerStealing())
                        {
                            soundEffect.PlaySound(0);
                            animator.SetBool("Saw", true);
                            if(playerNPCHandler.IsPlayerOnLeftSide(gameObject) && gameObject.GetComponent<SpriteRenderer>().flipX == false)
                            {
                                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                            }
                            else if(playerNPCHandler.IsPlayerOnRightSide(gameObject) && gameObject.GetComponent<SpriteRenderer>().flipX == true)
                            {
                                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                            }
                            playerNPCHandler.CatchPlayer();
                        }
                    }
                    else if(npc.IsState(State.See) && animator.GetCurrentAnimatorStateInfo(0).IsName("LookingEnd"))
                    {
                        animator.SetBool("Look", false);
                        npc.ChangeState(State.Calm);
                    }
                    break;
                case Type.LifeGuard:
                    if (npc.IsState(State.Move))
                    {
                        animator.SetBool("IsWalking", true);
                    }
                    if (npc.IsState(State.See))
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                        {
                            animator.SetBool("IsRunning", true);
                            animator.SetBool("IsSeeing", false);
                            npc.ChangeState(State.Chase);
                        }
                    }
                    if (npc.IsState(State.Stun))
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                        {
                            npc.ChangeState(State.Calm);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        void CheckStates()
        {
            switch (npc.State)
            {
                case State.Calm:
                    npc.GetNewState(animator);
                    break;
                case State.Move:
                    npc.IsTargetReached(rb.transform.position, animator);
                    break;
                case State.Chase:
                    npc.StartChase(animator);
                    npc.IsTargetReached(rb.transform.position, animator);
                    break;
                case State.See:
                    npc.See(animator);
                    break;
                case State.Stun:
                    npc.Stun(animator);
                    break;
                default:
                    break;
            }
        }

        void FixedUpdate()
        {
            if (npc.IsMoving && !gameHandler.IsPaused && (isInMainMenu || (playerNPCHandler != null && !playerNPCHandler.IsGameOver())))
            {
                Move();
            }
            if (npc.CoolDown && !gameHandler.IsPaused)
            {
                npc.CalculateCoolDown();
            }
        }

        void Move()
        {
            Vector2 direction = (new Vector2(npc.TargetPosition.x, npc.TargetPosition.y) - new Vector2(rb.transform.position.x, rb.transform.position.y)).normalized;

            RaycastHit2D[] hits = new RaycastHit2D[17];
            Vector2 rayStart = new Vector2(rb.transform.position.x, rb.transform.position.y) + direction * rb.velocity.magnitude * Time.deltaTime;
            var delta = Vector2.zero;

            for (int i = 0; i < 17; i++)
            {
                Vector2 rayDirection = Quaternion.AngleAxis((i - 8) * 15f, Vector3.forward) * direction;
                hits[i] = Physics2D.Raycast(rayStart, rayDirection, raySpacing - (Mathf.Abs(i - 8) * 0.05f), ~ignoreLayers);
                Debug.DrawRay(rayStart, rayDirection * (raySpacing - (Mathf.Abs(i - 8) * 0.05f)), Color.red);
                if (hits[i].collider != null)
                {
                    delta -= (1f / 17) * avoidanceForceMultiplier * (rayDirection / 10) * (hits[i].normal/5);
                }
                else
                {
                    delta += (1f / 17) * 3 * rayDirection;
                }
            }
            if(direction.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            transform.position += new Vector3(delta.x, delta.y, 0) * npc.Speed * Time.deltaTime;
        }
    }
}

