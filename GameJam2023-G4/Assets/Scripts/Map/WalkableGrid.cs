using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Maps
{
    public class WalkableGrid : MonoBehaviour
    {
        List<Coordinates> coordinates = new List<Coordinates>();
        List<Coordinates> walkableCoordinates = new List<Coordinates>();
        bool generated = false;
        public void GetCoordinates()
        {
            if(!generated)
            {
                for (int i = -8; i <= 7; i++)
                {
                    for (int j = -4; j <= -2; j++)
                    {
                        coordinates.Add(new Coordinates(new Vector2Int(i, j)));
                    }
                }
                walkableCoordinates.AddRange(coordinates);
                generated = true;
            }
        }

        public void FlagCell(Vector2Int pos)
        {
            var coord = coordinates.SingleOrDefault(c => c.VectorCoordinates == pos);
            if (coord != null)
            {
                coord.IsWalkable = false;
                walkableCoordinates.Remove(coord);
            }
        }

        public Vector2Int GetRandomCoordinate()
        {
            return walkableCoordinates[Random.Range(0, walkableCoordinates.Count)].VectorCoordinates;
        }

        public bool IsWalkable(Vector2Int pos)
        {
            var coord = coordinates.SingleOrDefault(c => c.VectorCoordinates == pos);
            if(coord != null && coord.IsWalkable)
            {
                return true;
            }
            return false;
        }
    }
}

