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
        public Tile[] lootedHorizontalTowels;
        public Tile[] verticalTowels;
        public Tile[] lootedVerticalTowels;
        public int towelCount = 3;
        WalkableGrid walkableGrid;

        public List<Towel> towels = new List<Towel>();

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

            if (isHorizontal && walkableGrid.IsFlaggable(cell) && cell.y + up.y < -3 && cell.x > -7)
            {
                if (walkableGrid.IsFlaggable(cell += up))
                {
                    return PlaceTowel(cell, left, towel);
                }
                else if(walkableGrid.IsFlaggable(cell += down))
                {
                    return PlaceTowel(cell, left, towel);
                }
            }
            else if(!isHorizontal && walkableGrid.IsFlaggable(cell) && cell.x + right.x < 7 && cell.x > -7)
            {
                if (walkableGrid.IsFlaggable(cell += right))
                {
                    return PlaceTowel(cell, left, towel);
                }
                else if (walkableGrid.IsFlaggable(cell += left))
                {
                    
                    return PlaceTowel(cell, left, towel);
                }
            }
            return false;
        }

        bool PlaceTowel(Vector2Int cell, Vector2Int direction, Tile towel)
        {
            if(FlagCellWithNeighbour(cell, direction))
            {
                SpawnTowelWithNeighbour(cell, direction, towel);
                return true;
            }
            return false;
        }

        bool FlagCellWithNeighbour(Vector2Int cell, Vector2Int direction)
        {
            if(walkableGrid.FlagCell(cell) && walkableGrid.FlagCell(cell + direction)){
                return true;   
            }
            walkableGrid.RevertFlag(cell);
            walkableGrid.RevertFlag(cell + direction);
            return false;
        }

        void SpawnTowelWithNeighbour(Vector2Int cell, Vector2Int direction, Tile towel)
        {
            towels.Add(new Towel(cell, cell + direction, towel, towel, false, tilemap));
            tilemap.SetTile(new Vector3Int(cell.x, cell.y, 0), towel);
            tilemap.SetTile(new Vector3Int(cell.x + direction.x, cell.y + direction.y, 0), towel);
        }

        public Towel SearchTowel(Vector2Int pos)
        {
            Towel neededTowel = null;
            foreach (var towel in towels)
            {
                if (towel.HasPosition(pos) && !towel.IsLooted)
                {
                    neededTowel = towel;
                }
            }
            return neededTowel;
        }
    }
}
