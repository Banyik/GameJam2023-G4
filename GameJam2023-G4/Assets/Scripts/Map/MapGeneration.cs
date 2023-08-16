using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maps
{
    public class MapGeneration : MonoBehaviour
    {
        int xMax = 10;
        int yMax = 10;
        float scale = 15;
        public float xOffset = 10;
        public float yOffset = 10;
        float maxOffset = 100000f;
        public int currentMapType = 0;
        int index = -1;
        List<int> maps = new List<int>();

        public GameObject[] mapObjects;
        private void Start()
        {
            CalculateOffsets();
            for (int i = 1; i <= xMax; i++)
            {
                for (int j = 1; j <= yMax; j++)
                {
                    maps.Add((int)(Mathf.Round((CalculateMapPerlinNoise(i, j) + 1) * 2) / 2 * 2) - 2);
                }
            }
            GetNextMap();
        }
        public void GetNextMap()
        {
            mapObjects[currentMapType].SetActive(false);
            currentMapType = maps[++index];
            mapObjects[currentMapType].SetActive(true);
        }
        void CalculateOffsets()
        {
            xOffset = Random.Range(0.0f, maxOffset);
            yOffset = Random.Range(0.0f, maxOffset);
        }
        float CalculateMapPerlinNoise(float x, float y)
        {
            float xNoise = x / (float)xMax * scale + xOffset;
            float yNoise = y / (float)yMax * scale + yOffset;
            return Mathf.PerlinNoise(xNoise, yNoise);
        }
    }
}

