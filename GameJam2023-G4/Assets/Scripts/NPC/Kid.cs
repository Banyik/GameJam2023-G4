using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCs
{
    public class Kid : NPC
    {
        public Kid(int speed, State state) : base(speed, state)
        {
        }

        public override void GetNewState()
        {
            this.State = State.Move;
            TargetPosition = new Vector2(Random.Range(-7.5f,7), Random.Range(-4.5f,-2));
        }

        public override void IsTargetReached(Vector2 position)
        {
            if (position == TargetPosition)
            {
                GetNewState();
            }
        }
    }
}

