using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCs
{
    public class NPC
    {
        int speed;
        State state;
        Vector2 targetPosition;
        public NPC(int speed, State state)
        {
            this.speed = speed;
            this.state = state;
        }

        public virtual void GetNewState() { }
        public virtual void IsTargetReached(Vector2 position) { }

        public int Speed { get => speed; set => speed = value; }
        public State State { get => state; set => state = value; }
        public Vector2 TargetPosition { get => targetPosition; set => targetPosition = value; }
    }

}
