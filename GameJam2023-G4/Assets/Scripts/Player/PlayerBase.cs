using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerBase
    {
        float speed;
        State state;

        public PlayerBase(float speed, State state)
        {
            this.speed = speed;
            this.state = state;
        }

        public bool IsState(State state)
        {
            return this.state == state;
        }
        public float Speed { get => speed; set => speed = value; }
        public State State { get => state; set => state = value; }
    }
}

