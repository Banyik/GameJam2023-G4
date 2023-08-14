using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maps
{
    public class Coordinates
    {
        Vector2Int vectorCoordinates;
        bool isWalkable;

        public Coordinates(Vector2Int vectorCoordinates)
        {
            this.vectorCoordinates = vectorCoordinates;
            this.isWalkable = true;
        }
        public Vector2Int VectorCoordinates { get => vectorCoordinates; set => vectorCoordinates = value; }
        public bool IsWalkable { get => isWalkable; set => isWalkable = value; }
    }
}

