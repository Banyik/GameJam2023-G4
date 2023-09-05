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
        GameObject PauseMenu;

        private void Start()
        {
            mapGeneration = gameObject.GetComponent<MapGeneration>();
            grid = GameObject.Find("Grid").GetComponent<Grid>();
            PauseMenu = GameObject.Find("Canvas").gameObject.transform.Find("Pause").gameObject;
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

        public bool IsPaused()
        {
            GetGameHandler();
            return gameHandler.IsPaused;
        }

        public void Pause()
        {
            GetGameHandler();
            gameHandler.IsPaused = true;
            PauseMenu.SetActive(true);
        }

        public Towel GetTowel(Vector2 position)
        {
            GetTileSpawner();
            return tileSpawner.SearchTowel((Vector2Int)grid.WorldToCell(position));
        }

        public void SpawnTowels(int map, int mapType)
        {
            GetTileSpawner();
            tileSpawner.SpawnTowels(map, mapType);
        }
    }
}

