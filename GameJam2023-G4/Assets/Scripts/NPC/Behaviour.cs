using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCs
{
    public class Behaviour : MonoBehaviour
    {
        public Type type;
        NPC npc;
        void Start()
        {
            switch (type)
            {
                case Type.Kid:
                    npc = new Kid(5, State.Calm);
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
                    gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, npc.TargetPosition, npc.Speed*Time.deltaTime);
                    npc.IsTargetReached(gameObject.transform.position);
                    break;
                case State.Chase:
                    break;
                default:
                    break;
            }
        }
    }
}

