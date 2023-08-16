using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;

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
        LayerMask ignoreLayer;
        WalkableGrid walkableGrid;
        public RuntimeAnimatorController lifeGuardAnimator;
        public RuntimeAnimatorController kidAnimator;
        public RuntimeAnimatorController grandmaAnimator;
        public Animator animator;
        public void Start()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            walkableGrid = scriptHandler.GetComponent<WalkableGrid>();
            walkableGrid.GetCoordinates();
            switch (type)
            {
                case Type.Kid:
                    npc = new Kid(5, State.Calm, new Vector2Int(4, -2), walkableGrid, true);
                    animator.runtimeAnimatorController = kidAnimator;
                    break;
                case Type.Grandma:
                    break;
                case Type.LifeGuard:
                    npc = new LifeGuard(1, State.Calm, new Vector2Int(4, -2), walkableGrid, true);
                    animator.runtimeAnimatorController = lifeGuardAnimator;
                    transform.position = new Vector3(9, 0, 0);
                    ignoreLayer = LayerMask.GetMask("LifeGuardPath");
                    break;
                default:
                    break;
            }
        }

        private void Update()
        {
            if (npc.IsState(State.See))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    animator.SetBool("IsRunning", true);
                    animator.SetBool("IsSeeing", false);
                    npc.ChangeState(State.Chase);
                }
            }
        }

        void FixedUpdate()
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
            if (npc.IsMoving)
            {
                Move();
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
                hits[i] = Physics2D.Raycast(rayStart, rayDirection, raySpacing - (Mathf.Abs(i - 8) * 0.1f), ~ignoreLayer);
                Debug.DrawRay(rayStart, rayDirection * (raySpacing - (Mathf.Abs(i - 8) * 0.1f)), Color.red);
                if (hits[i].collider != null)
                {
                    delta -= (1f / 17) * avoidanceForceMultiplier * (rayDirection / 5);
                }
                else
                {
                    delta += (1f / 17) * avoidanceForceMultiplier * rayDirection;
                }
            }
            if(direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 0);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 0);
            }
            transform.position += new Vector3(delta.x, delta.y, 0) * npc.Speed * Time.deltaTime;
        }
    }
}

