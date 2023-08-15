using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;

namespace NPCs
{
    public class Behaviour : MonoBehaviour
    {
        public Type type;
        NPC npc;
        Rigidbody2D rb;
        public float avoidanceForceMultiplier = 0.4f;
        public float raySpacing = 3f;
        public GameObject scriptHandler;
        WalkableGrid walkableGrid;
        public void Start()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            walkableGrid = scriptHandler.GetComponent<WalkableGrid>();
            walkableGrid.GetCoordinates();
            switch (type)
            {
                case Type.Kid:
                    npc = new Kid(5, State.Calm, new Vector2Int(4, -2), walkableGrid);
                    break;
                case Type.Grandma:
                    break;
                case Type.LifeGuard:
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
                    Move();
                    npc.IsTargetReached(rb.transform.position);
                    break;
                case State.Chase:
                    break;
                default:
                    break;
            }
        }

        void Move()
        {
            Vector2 direction = npc.TargetPosition - (Vector2)rb.transform.position.normalized;

            RaycastHit2D[] hits = new RaycastHit2D[17];
            Vector2 rayStart = (Vector2)rb.transform.position + direction * rb.velocity.magnitude * Time.deltaTime;
            var delta = Vector2.zero;

            for (int i = 0; i < 17; i++)
            {
                Vector2 rayDirection = Quaternion.AngleAxis((i - 8) * 15f, Vector3.forward) * direction;
                hits[i] = Physics2D.Raycast(rayStart, rayDirection, raySpacing - (Mathf.Abs(i - 8) * 0.1f));
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
            transform.position += (Vector3)delta * npc.Speed * Time.deltaTime;
        }
    }
}

