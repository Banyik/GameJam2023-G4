using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Maps
{
    public class WalkableGrid : MonoBehaviour
    {
        List<Coordinates> coordinates = new List<Coordinates>();
        bool generated = false;
        public TileSpawn tileSpawn;
        public void GetCoordinates()
        {
            if(!generated)
            {
                for (int i = -8; i < 8; i++)
                {
                    for (int j = -5; j < -2; j++)
                    {
                        coordinates.Add(new Coordinates(new Vector2Int(i, j)));
                    }
                }
                generated = true;
            }
        }

        public void ResetMap()
        {
            foreach (var coord in coordinates)
            {
                coord.IsWalkable = true;
            }
            tileSpawn.ResetMap();
            tileSpawn.SpawnTowels();
        }

        Coordinates FindCoordinate(Vector2Int pos)
        {
            Coordinates coord = null;
            foreach (var c in coordinates)
            {
                if (c.VectorCoordinates == pos)
                {
                    coord = c;
                }
            }
            return coord;
        }

        public bool FlagCell(Vector2Int pos)
        {
            Coordinates coord = FindCoordinate(pos);
            if(coord != null)
            {
                coord.IsWalkable = false;
                return true;
            }
            return false;
        }

        public void RevertFlag(Vector2Int pos)
        {
            Coordinates coord = FindCoordinate(pos);
            if (coord != null)
            {
                coord.IsWalkable = true;
            }
        }

        public bool IsFlaggable(Vector2Int pos)
        {
            Coordinates coord = FindCoordinate(pos);
            if (coord != null && coord.IsWalkable)
            {
                return true;
            }
            return false;
        }

        public Vector2Int GetRandomCoordinate()
        {
            Coordinates coord = null;
            do
            {
                coord = coordinates[Random.Range(0, coordinates.Count)];
            } while (coord == null || !coord.IsWalkable);
            return coord.VectorCoordinates;
        }

        public bool IsWalkable(Vector2Int pos)
        {
            Coordinates coord = FindCoordinate(pos);
            if (coord != null)
            {
                return true;
            }
            return false;
        }
    }
}

