using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;

namespace NPCs
{
    public class NPC
    {
        int speed;
        bool coolDown;
        State state;
        Vector2Int targetPosition;
        Vector2Int currentPosition;
        WalkableGrid walkableGrid;
        bool isMoving;
        public NPC(int speed, State state, Vector2Int targetPosition, WalkableGrid walkableGrid, bool isMoving, bool coolDown)
        {
            this.speed = speed;
            this.state = state;
            this.targetPosition = targetPosition;
            this.walkableGrid = walkableGrid;
            this.isMoving = isMoving;
            this.coolDown = coolDown;
        }

        public void ChangeState(State state)
        {
            if(this.state != state)
            {
                this.state = state;
            }
        }
        public bool IsState(State state)
        {
            return this.state == state;
        }

        public virtual void GetNewState(Animator animator) { }
        public virtual void StartChase(Animator animator) { }
        public virtual void IsTargetReached(Vector2 position, Animator animator) { }
        public virtual void See(Animator animator) { }
        public virtual void Stun(Animator animator) { }
        public virtual void CalculateCoolDown() { }



        public int Speed { get => speed; set => speed = value; }
        public State State { get => state; set => state = value; }
        public Vector2Int TargetPosition { get => targetPosition; set => targetPosition = value; }
        public Vector2Int CurrentPosition { get => currentPosition; set => currentPosition = value; }
        public WalkableGrid WalkableGrid { get => walkableGrid; set => walkableGrid = value; }
        public bool IsMoving { get => isMoving; set => isMoving = value; }
        public bool CoolDown { get => coolDown; set => coolDown = value; }
    }

}
