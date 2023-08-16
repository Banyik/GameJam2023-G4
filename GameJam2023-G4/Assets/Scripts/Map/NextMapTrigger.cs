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
                map.GetNextMap();
                walkableGrid.ResetMap();
                collision.transform.position = new Vector3(-7, -2.5f, 0);
            }
        }
    }
}

