using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;

namespace NPCs
{
    public class Kid : NPC
    {
        float targetSwitchTimeScale = 3f;
        float targetSwtichTimer = 0f;
        public Kid(int speed, State state, WalkableGrid walkableGrid, bool isMoving, bool coolDown) : base(speed, state, walkableGrid, isMoving, coolDown)
        {
        }

        public override void GetNewState(Animator animator)
        {
            ChangeState(State.Move);
            IsMoving = true;
            TargetPosition = WalkableGrid.GetRandomCoordinate();
        }

        public override void IsTargetReached(Vector2 position, Animator animator)
        {
            if (Vector3.Distance((Vector2)TargetPosition, position) <= 1.5f)
            {
                GetNewState(animator);
            }
            else if (targetSwtichTimer < targetSwitchTimeScale)
            {
                targetSwtichTimer += Time.deltaTime;
                Debug.Log(targetSwtichTimer);
            }
            else
            {
                targetSwtichTimer = 0;
                GetNewState(animator);
            }
        }
    }
}

