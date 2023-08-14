using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;

namespace NPCs
{
    public class Kid : NPC
    {
        public Kid(int speed, State state, Vector2Int currentPosition, WalkableGrid walkableGrid) : base(speed, state, currentPosition, walkableGrid)
        {
        }

        public override void GetNewState()
        {
            this.State = State.Move;
            TargetPosition = WalkableGrid.GetRandomCoordinate();
        }

        public override void IsTargetReached(Vector2 position)
        {
            if (Vector3.Distance(new Vector3(TargetPosition.x, TargetPosition.y, 0), position) <= .09f)
            {
                CurrentPosition = TargetPosition;
                GetNewState();
            }
        }
    }
}

