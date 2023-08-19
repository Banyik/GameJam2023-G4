using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerNPCHandler : MonoBehaviour
    {
        Behaviour playerBehaviour;
        private void Start()
        {
            playerBehaviour = GameObject.Find("Player").GetComponent<Behaviour>();
        }

        public bool IsPlayerStealing()
        {
            return playerBehaviour.IsPlayerStealing();
        }

        public bool IsGameOver()
        {
            return playerBehaviour.IsGameOver();
        }

        public void CatchPlayer()
        {
            playerBehaviour.ChangeState(State.Caught);
        }

        public void StunPlayer()
        {
            playerBehaviour.ChangeState(State.StunnedStart);
        }

        public bool IsPlayerAvoidingStun(bool disable)
        {
            bool value = playerBehaviour.avoidStun;
            if (disable)
            {
                playerBehaviour.avoidStun = false;
            }
            return value;
        }

        public Vector3 GetPlayerPosition()
        {
            return playerBehaviour.gameObject.transform.position;
        }
        public bool IsPlayerOnRightSide(GameObject npc)
        {
            return playerBehaviour.gameObject.transform.position.x > npc.transform.position.x;
        }

        public bool IsPlayerOnLeftSide(GameObject npc)
        {
            return playerBehaviour.gameObject.transform.position.x < npc.transform.position.x;
        }
    }
}

