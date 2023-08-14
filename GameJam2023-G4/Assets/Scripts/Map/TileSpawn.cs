using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSpawn : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile tile;

    void Start()
    {
        tilemap.SetTile(new Vector3Int(0, 0, 0), tile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
