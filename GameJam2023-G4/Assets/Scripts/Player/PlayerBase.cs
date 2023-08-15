using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerBase
    {
        float speed;
        float thirst;
        float maxThirst;
        float money;
        State state;
        public PlayerBase(float speed, float thirst, float maxThirst, float money, State state)
        {
            this.speed = speed;
            this.thirst = thirst;
            this.maxThirst = maxThirst;
            this.money = money;
            this.state = state;
        }

        public bool IsState(State state)
        {
            return this.state == state;
        }
        public float Speed { get => speed; set => speed = value; }
        public State State { get => state; set => state = value; }
        public float Thirst { get => thirst; set => thirst = value; }
        public float MaxThirst { get => maxThirst; set => maxThirst = value; }
        public float Money { get => money; set => money = value; }
    }
}

