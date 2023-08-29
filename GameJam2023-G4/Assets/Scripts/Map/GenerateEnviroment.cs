using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateEnviroment : MonoBehaviour
{
    public Tile[] balatonElements;
    public Tile[] festivalElements;
    public Tile[] mud;

    public Tilemap balatonTiles;
    public Tilemap festivalTiles;

    int startX = -7;
    int amount = 3;

    public void GenerateElements(int mapType)
    {
        ClearElements(mapType);
        Tilemap tiles = GetTilemap(mapType);
        Tile[] tileSet = GetTileSet(mapType);
        for (int i = 0; i <= amount; i++)
        {
            tiles.SetTile(new Vector3Int(startX + i * 4, 0, 0), tileSet[Random.Range(0, tileSet.Length)]);
        }
    }

    void GenerateMud()
    {
        for (int i = 0; i < 20; i++)
        {
            festivalTiles.SetTile(new Vector3Int(Random.Range(-7, 8), Random.Range(-1, -5), 0), mud[Random.Range(0, mud.Length)]);
        }
    }

    void ClearElements(int mapType)
    {
        Tilemap tiles = GetTilemap(mapType);
        tiles.ClearAllTiles();
    }

    Tilemap GetTilemap(int mapType)
    {
        switch (mapType)
        {
            case 0:
                return balatonTiles;
            case 1:
                return festivalTiles;
            default:
                return balatonTiles;
        }
    }

    Tile[] GetTileSet(int mapType)
    {
        switch (mapType)
        {
            case 0:
                startX = -7;
                amount = 3;
                return balatonElements;
            case 1:
                startX = -4;
                amount = 2;
                GenerateMud();
                return festivalElements;
            default:
                startX = -7;
                amount = 3;
                return balatonElements;
        }
    }
}
