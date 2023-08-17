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

        public void ResetMap()
        {
            foreach (var towel in towels)
            {
                towel.RemoveTowel();
            }
            towels.Clear();
        }

        public void SpawnTowels()
        {
            for (int i = 0; i < towelCount; i++)
            {
                Vector2Int cell = walkableGrid.GetRandomCoordinate();
                if (!TryPlaceTowel(Random.Range(0, 100) < 50f, cell))
                {
                    i--;
                }
            }
        }

        public bool TryPlaceTowel(bool isHorizontal, Vector2Int cell)
        {
            var up = new Vector2Int(0, 1);
            var down = new Vector2Int(0, -1);
            var right = new Vector2Int(1, 0);
            var left = new Vector2Int(-1, 0);

            if (!isHorizontal && walkableGrid.IsFlaggable(cell) && cell.y + up.y < -2 && cell.y > -5 && cell.x > -7)
            {
                int index = Random.Range(0, verticalTowels.Length);
                Tile baseTowel = verticalTowels[index];
                Tile lootedTowel = lootedVerticalTowels[index];
                if (walkableGrid.IsFlaggable(cell + up))
                {
                    return PlaceTowel(cell, up, baseTowel, lootedTowel);
                }
                else if(walkableGrid.IsFlaggable(cell + down))
                {
                    return PlaceTowel(cell + down, up, baseTowel, lootedTowel);
                }
            }
            else if(isHorizontal && walkableGrid.IsFlaggable(cell) && cell.x + right.x < 7 && cell.y > -5 && cell.x > -7)
            {
                int index = Random.Range(0, horizontalTowels.Length);
                Tile baseTowel = horizontalTowels[index];
                Tile lootedTowel = lootedHorizontalTowels[index];
                if (walkableGrid.IsFlaggable(cell + right))
                {
                    return PlaceTowel(cell, right, baseTowel, lootedTowel);
                }
                else if (walkableGrid.IsFlaggable(cell + left))
                {
                    
                    return PlaceTowel(cell + left, right, baseTowel, lootedTowel);
                }
            }
            return false;
        }

        bool PlaceTowel(Vector2Int cell, Vector2Int direction, Tile towel, Tile lootedTowel)
        {
            if(FlagCellWithNeighbour(cell, direction))
            {
                SpawnTowelWithNeighbour(cell, direction, towel, lootedTowel);
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

        void SpawnTowelWithNeighbour(Vector2Int cell, Vector2Int direction, Tile towel, Tile lootedTowel)
        {
            towels.Add(new Towel(cell, cell + direction, towel, lootedTowel, false, tilemap));
            tilemap.SetTile(new Vector3Int(cell.x, cell.y, 0), towel);
            Debug.Log($"cell: {cell}; direction: {direction}");
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
