using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;

namespace NPCs
{
    public class Kid : NPC
    {
        public Kid(int speed, State state, Vector2Int currentPosition, WalkableGrid walkableGrid, bool isMoving) : base(speed, state, currentPosition, walkableGrid, isMoving)
        {
        }

        public override void GetNewState(Animator animator)
        {
            ChangeState(State.Move);
            IsMoving = true;
            TargetPosition = WalkableGrid.GetRandomCoordinate();
            TargetPosition += new Vector2Int(1, 1);
        }

        public override void IsTargetReached(Vector2 position, Animator animator)
        {
            if (Vector3.Distance((Vector2)TargetPosition, position) <= 1f)
            {
                CurrentPosition = TargetPosition;
                GetNewState(animator);
            }
        }
    }
}

