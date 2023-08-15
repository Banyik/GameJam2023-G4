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
        public Tile[] horizontalTowels;
        public Tile[] verticalTowels;
        public int towelCount = 3;
        WalkableGrid walkableGrid;

        void Start()
        {
            walkableGrid = gameObject.GetComponent<WalkableGrid>();
            walkableGrid.GetCoordinates();
            SpawnTowels();
        }

        void SpawnTowels()
        {
            for (int i = 0; i < towelCount; i++)
            {
                Vector2Int cell = walkableGrid.GetRandomCoordinate();
                if (!TryPlaceTowel(Random.Range(0, 100) < 50f, cell, tile))
                {
                    i--;
                }
            }
        }

        public bool TryPlaceTowel(bool isHorizontal, Vector2Int cell, Tile towel)
        {
            var up = new Vector2Int(0, 1);
            var down = new Vector2Int(0, -1);
            var right = new Vector2Int(1, 0);
            var left = new Vector2Int(-1, 0);
            if (isHorizontal && walkableGrid.IsWalkable(cell))
            {
                if (walkableGrid.IsWalkable(cell += up))
                {
                    PlaceTowel(cell, up, towel);
                    return true;
                }
                else if(walkableGrid.IsWalkable(cell += down))
                {
                    PlaceTowel(cell, down, towel);
                    return true;
                }
            }
            else if(!isHorizontal && walkableGrid.IsWalkable(cell))
            {
                if (walkableGrid.IsWalkable(cell += right))
                {
                    PlaceTowel(cell, right, towel);
                    return true;
                }
                else if (walkableGrid.IsWalkable(cell += left))
                {
                    PlaceTowel(cell, left, towel);
                    return true;
                }
            }
            return false;
        }

        void PlaceTowel(Vector2Int cell, Vector2Int direction, Tile towel)
        {
            FlagCellWithNeighbour(cell, direction);
            SpawnTowelWithNeighbour(cell, direction, towel);
        }

        void FlagCellWithNeighbour(Vector2Int cell, Vector2Int direction)
        {
            walkableGrid.FlagCell(cell);
            walkableGrid.FlagCell(cell + direction);
        }

        void SpawnTowelWithNeighbour(Vector2Int cell, Vector2Int direction, Tile towel)
        {
            tilemap.SetTile(new Vector3Int(cell.x, cell.y, 0), towel);
            tilemap.SetTile(new Vector3Int(cell.x + direction.x, cell.y + direction.y, 0), towel);
        }
    }
}
