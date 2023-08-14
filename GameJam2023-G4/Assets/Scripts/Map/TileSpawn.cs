using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Maps
{
    public class TileSpawn : MonoBehaviour
    {
        public Tilemap tilemap;
        public Tile tile;
        WalkableGrid walkableGrid;

        void Start()
        {
            walkableGrid = gameObject.GetComponent<WalkableGrid>();
            walkableGrid.GetCoordinates();
            for (int i = 0; i < 5; i++)
            {
                Vector2Int cell = walkableGrid.GetRandomCoordinate();
                walkableGrid.FlagCell(cell);
                tilemap.SetTile(new Vector3Int(cell.x, cell.y, 0), tile);
            }
            
        }
    }
}
