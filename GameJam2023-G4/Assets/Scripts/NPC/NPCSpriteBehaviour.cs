using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCs
{
    public class NPCSpriteBehaviour : MonoBehaviour
    {
        Sprite sprite;
        GameObject NPC;
        Behaviour behaviour;
        SpriteRenderer spriteRenderer;

        private void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            behaviour = NPC.GetComponent<Behaviour>();
        }
        public void SetSprite(Sprite sprite, GameObject NPC)
        {
            this.NPC = NPC;
            this.sprite = sprite;
        }

        private void Update()
        {
            if (NPC.transform.position.x > 8.5f && NPC.transform.position.x < 10f && behaviour.npc.TargetPosition.x < 8.5f)
            {
                spriteRenderer.sprite = sprite;
                spriteRenderer.flipX = false;
                gameObject.transform.position = new Vector3(7.5f, NPC.transform.position.y, 0);
            }
            else if (NPC.transform.position.x < -8.5f && NPC.transform.position.x > -10f && behaviour.npc.TargetPosition.x > -8.5f)
            {
                spriteRenderer.sprite = sprite;
                spriteRenderer.flipX = transform;
                gameObject.transform.position = new Vector3(-6.5f, NPC.transform.position.y, 0);
            }
            else
            {
                spriteRenderer.sprite = null;
            }
        }
    }
}

