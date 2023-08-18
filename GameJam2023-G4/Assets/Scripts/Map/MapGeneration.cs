using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

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
        int[] difficultyScore;
        private int currentMapType = 0;
        int index = -1;
        List<int> maps = new List<int>();
        PlayerMapHandler playerMapHandler;

        public GameObject[] mapObjects;
        bool nextMapType = false;
        int minScore = 30;
        private void Start()
        {
            playerMapHandler = gameObject.GetComponent<PlayerMapHandler>();
            difficultyScore = new int[3] { 500, 1320, 2500 };
            minScore = difficultyScore[currentMapType];
            SetMapDifficulty();
        }

        private void Generate()
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

        public int CheckScore(int score)
        {
            index = -1;
            if (score >= minScore)
            {
                minScore = difficultyScore[currentMapType];
                nextMapType = true;
                //increase minScore
                return score - minScore;
            }
            return 0;
        }
        public void ClearMap()
        {
            index = 0;
            maps.Clear();
            Generate();
        }
        public int GetMap()
        {
            return currentMapType;
        }

        public void SetMapDifficulty()
        {
            if (nextMapType)
            {
                nextMapType = false;
                index = -1;
                GetNextMap();
            }
            index++;
            playerMapHandler.SpawnTowels(index + 1);
            playerMapHandler.SetDifficulty();
        }
        public void GetNextMap()
        {
            mapObjects[currentMapType].SetActive(false);
            currentMapType++;
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

