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

        public bool HasEnoughScore(int score)
        {
            minScore = difficultyScore[currentMapType];
            return score >= minScore;
        }

        public int CheckScore(int score)
        {
            ResetMapTypeIndex();
            if (score >= minScore)
            {
                nextMapType = true;
                return score - minScore;
            }
            return 0;
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
                ResetMapTypeIndex();
                GetNextMap();
            }
            index++;
            generateEnviroment.GenerateElements(currentMapType);
            playerMapHandler.SpawnTowels(index + 1, currentMapType);
        }
        public void GetNextMap()
        {
            mapObjects[currentMapType].SetActive(false);
            currentMapType++;
            mapObjects[currentMapType].SetActive(true);
        }
    }
}

