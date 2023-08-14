using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;

namespace NPCs
{
    public class NPC
    {
        int speed;
        State state;
        Vector2Int targetPosition;
        Vector2Int currentPosition;
        WalkableGrid walkableGrid;
        public NPC(int speed, State state, Vector2Int currentPosition, WalkableGrid walkableGrid)
        {
            this.speed = speed;
            this.state = state;
            this.currentPosition = currentPosition;
            this.walkableGrid = walkableGrid;
        }

        public virtual void GetNewState() { }
        public virtual void IsTargetReached(Vector2 position) { }

        public int Speed { get => speed; set => speed = value; }
        public State State { get => state; set => state = value; }
        public Vector2Int TargetPosition { get => targetPosition; set => targetPosition = value; }
        public Vector2Int CurrentPosition { get => currentPosition; set => currentPosition = value; }
        public WalkableGrid WalkableGrid { get => walkableGrid; set => walkableGrid = value; }
    }

}
