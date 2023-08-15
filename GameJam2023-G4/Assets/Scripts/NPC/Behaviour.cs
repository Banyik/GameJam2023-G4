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
        public void Start()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            walkableGrid = scriptHandler.GetComponent<WalkableGrid>();
            walkableGrid.GetCoordinates();
            switch (type)
            {
                case Type.Kid:
                    npc = new Kid(5, State.Calm, new Vector2Int(4, -2), walkableGrid, true);
                    break;
                case Type.Grandma:
                    break;
                case Type.LifeGuard:
                    npc = new LifeGuard(1, State.Calm, new Vector2Int(4, -2), walkableGrid, true);
                    transform.position = new Vector3(9, 0, 0);
                    ignoreLayer = LayerMask.GetMask("LifeGuardPath");
                    break;
                default:
                    break;
            }
        }

        void FixedUpdate()
        {
            switch (npc.State)
            {
                case State.Calm:
                    npc.GetNewState();
                    break;
                case State.Move:
                    npc.IsTargetReached(rb.transform.position);
                    break;
                case State.Chase:
                    npc.StartChase();
                    npc.IsTargetReached(rb.transform.position);
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
            transform.position += new Vector3(delta.x, delta.y, 0) * npc.Speed * Time.deltaTime;
        }
    }
}

