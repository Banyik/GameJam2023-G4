using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using Maps;

namespace Player
{
    public class PlayerMapHandler : MonoBehaviour
    {
        TileSpawn tileSpawner;
        GameHandler gameHandler;
        MapGeneration mapGeneration;
        Grid grid;
        Behaviour playerBehaviour;

        private void Start()
        {
            mapGeneration = gameObject.GetComponent<MapGeneration>();
            grid = GameObject.Find("Grid").GetComponent<Grid>();
        }

        void GetTileSpawner()
        {
            if (tileSpawner == null)
            {
                tileSpawner = gameObject.GetComponent<TileSpawn>();
            }
        }

        void GetPlayerBehaviour()
        {
            if (playerBehaviour == null)
            {
                playerBehaviour = GameObject.Find("Player").GetComponent<Behaviour>();
            }
        }
        void GetGameHandler()
        {
            if (gameHandler == null)
            {
                gameHandler = gameObject.GetComponent<GameHandler>();
            }
        }

        public int GetTowelCount()
        {
            GetTileSpawner();
            return tileSpawner.towelCount;
        }

        public void TimesUp(float money)
        {
            GetGameHandler();
            gameHandler.TimesUp(money);
        }

        public void GameOver(float money)
        {
            GetGameHandler();
            gameHandler.GameOver(money);
        }

        public Towel GetTowel(Vector2 position)
        {
            GetTileSpawner();
            return tileSpawner.SearchTowel((Vector2Int)grid.WorldToCell(position));
        }

        public void SpawnTowels(int map)
        {
            GetTileSpawner();
            tileSpawner.SpawnTowels(map);
        }

        public void SetDifficulty()
        {
            GetPlayerBehaviour();
            playerBehaviour.SetDifficulty();
        }
    }
}

