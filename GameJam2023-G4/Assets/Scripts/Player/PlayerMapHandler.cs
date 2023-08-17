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
            tileSpawner = gameObject.GetComponent<TileSpawn>();
            gameHandler = gameObject.GetComponent<GameHandler>();
            mapGeneration = gameObject.GetComponent<MapGeneration>();
            grid = GameObject.Find("Grid").GetComponent<Grid>();
            playerBehaviour = GameObject.Find("Player").GetComponent<Behaviour>();
        }

        public int GetTowelCount()
        {
            return tileSpawner.towelCount;
        }

        public void TimesUp()
        {
            gameHandler.TimesUp();
        }

        public void GameOver(float money)
        {
            gameHandler.GameOver(mapGeneration.CurrentMapIndex(), money);
        }

        public Towel GetTowel(Vector2 position)
        {
            return tileSpawner.SearchTowel((Vector2Int)grid.WorldToCell(position));
        }

        public void SetDifficulty()
        {
            playerBehaviour.SetDifficulty();
        }
    }
}

