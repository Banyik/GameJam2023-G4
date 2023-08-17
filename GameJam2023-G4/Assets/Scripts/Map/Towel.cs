using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Items;

namespace Maps
{
    public class Towel
    {
        Loot loot;
        Vector2Int firstPosition;
        Vector2Int secondPosition;
        Tile skin;
        Tile lootedSkin;
        Tilemap towelTiles;
        bool isLooted;
        int mapType;

        public bool IsLooted { get => isLooted; set => isLooted = value; }
        public Loot Loot { get => loot; set => loot = value; }

        public Towel(Vector2Int firstPosition, Vector2Int secondPosition, Tile skin, Tile lootedSkin, bool isLooted, Tilemap towelTiles, int mapType)
        {
            this.loot = GenerateLoot(Random.Range(0, 100) < 50f);
            this.firstPosition = firstPosition;
            this.secondPosition = secondPosition;
            this.skin = skin;
            this.lootedSkin = lootedSkin;
            this.isLooted = isLooted;
            this.towelTiles = towelTiles;
            this.mapType = mapType;
            CreateTowel();
        }
        void CreateTowel()
        {
            towelTiles.SetTile(new Vector3Int(firstPosition.x, firstPosition.y, 0), skin);
        }

        public void RemoveTowel()
        {
            towelTiles.SetTile(new Vector3Int(firstPosition.x, firstPosition.y, 0), null);
        }

        public void LootTowel()
        {
            isLooted = true;
            towelTiles.SetTile(new Vector3Int(firstPosition.x, firstPosition.y, 0), lootedSkin);
            //SetLootedSkin
        }

        public bool HasPosition(Vector2Int pos)
        {
            return pos == firstPosition || pos == secondPosition;
        }

        Loot GenerateLoot(bool withMoneyAndWater)
        {
            Loot loot;
            if (withMoneyAndWater)
            {
                loot = new Loot(new Item[2] { GenerateWaterItem(), GenerateMoneyItem() });
            }
            else
            {
                loot = new Loot(new Item[1] { GenerateWaterItem() });
            }
            return loot;
        }

        Item GenerateWaterItem()
        {
            return new Item(Random.Range(0.1f, 0.3f * (mapType + 1)), ItemType.Water);
        }
        Item GenerateMoneyItem()
        {
            return new Item(Mathf.RoundToInt(Random.Range(1, 50*(mapType+1))), ItemType.Money);
        }
    }
}

