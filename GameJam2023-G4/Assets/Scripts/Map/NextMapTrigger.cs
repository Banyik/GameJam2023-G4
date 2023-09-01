using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Maps
{
    public class NextMapTrigger : MonoBehaviour
    {
        public WalkableGrid walkableGrid;
        public MapGeneration map;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                GameObject.Find("IntroLine").SetActive(false);
                walkableGrid.ResetMap();
            }
        }
    }
}

