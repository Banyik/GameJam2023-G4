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
        GenerateEnviroment generateEnviroment;
        ScoreboardHandler scoreboardHandler;
        public GameObject[] mapObjects;
        bool nextMapType = false;
        int minScore = 30;
        private void Start()
        {
            playerMapHandler = gameObject.GetComponent<PlayerMapHandler>();
            generateEnviroment = gameObject.GetComponent<GenerateEnviroment>();
            scoreboardHandler = gameObject.GetComponent<ScoreboardHandler>();
            difficultyScore = new int[3] { 1700, 2500, 4000 };
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

        public bool HasEnoughScore(int score)
        {
            return score >= minScore;
        }

        public int CheckScore(int score)
        {
            ResetMapTypeIndex();
            minScore = difficultyScore[currentMapType];
            if (score >= minScore)
            {
                nextMapType = true;
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

        public void ResetMapTypeIndex()
        {
            index = -1;
        }

        public void ShowScoreboardUI(int score)
        {
            scoreboardHandler.ShowScoreboard(new int[3] { minScore, score, score - minScore });
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
            generateEnviroment.GenerateElements(currentMapType);
            playerMapHandler.SpawnTowels(index + 1);
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

