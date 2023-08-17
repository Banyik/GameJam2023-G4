using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCs
{
    public class SpawnNPC : MonoBehaviour
    {
        public GameObject NPC;
        public void Spawn(Type type, bool flipSprite, Vector2 pos)
        {
            var npc = Instantiate(NPC, pos + new Vector2(1, 0.5f), new Quaternion(0, 0, 0, 0), null);
            npc.GetComponent<Behaviour>().type = type;
            npc.GetComponent<Behaviour>().StartNPC();
            npc.SetActive(true);
            npc.GetComponent<SpriteRenderer>().flipX = flipSprite;
        }
    }
}

