using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;

namespace NPCs
{
    public class LifeGuard : NPC
    {
        bool onLeft = false;
        public LifeGuard(int speed, State state, Vector2Int currentPosition, WalkableGrid walkableGrid, bool isMoving) : base(speed, state, currentPosition, walkableGrid, isMoving)
        {
        }

        public override void GetNewState()
        {
            ChangeState(State.Move);
            IsMoving = true;
            if (onLeft)
            {
                TargetPosition = new Vector2Int(9, 0);
            }
            else
            {
                TargetPosition = new Vector2Int(-9, 0);
            }
        }

        public override void StartChase()
        {
            Speed = 6;
            TargetPosition = Vector2Int.RoundToInt(GameObject.Find("Player").transform.position);
        }

        public override void IsTargetReached(Vector2 position)
        {
            if (Vector3.Distance((Vector2)TargetPosition, position) <= .09f && !IsState(State.Chase))
            {
                CurrentPosition = TargetPosition;
                onLeft = !onLeft;
                GetNewState();
            }
            else if (Vector3.Distance((Vector2)TargetPosition, position) <= 0.75f && IsState(State.Chase))
            {
                Speed = 1;
                ChangeState(State.Calm);
                IsMoving = false;
                GetNewState();
            }
        }
    }
}

