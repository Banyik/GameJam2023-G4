using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace NPCs
{
    public class TriggerView : MonoBehaviour
    {
        Behaviour npcBehaviour;
        bool playerIsVisible = false;
        public Player.Behaviour playerBehaviour;
        void Start()
        {
            npcBehaviour = gameObject.GetComponentInParent<Behaviour>();
        }

        private void Update()
        {
            transform.up = (Vector3Int)npcBehaviour.npc.TargetPosition - transform.position;
            if (playerIsVisible && (playerBehaviour.player.IsState(Player.State.StealingStart) || playerBehaviour.player.IsState(Player.State.Stealing)))
            {
                npcBehaviour.npc.ChangeState(State.Chase);
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Player")
            {
                playerIsVisible = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.tag == "Player")
            {
                playerIsVisible = false;
            }
        }
    }
}

