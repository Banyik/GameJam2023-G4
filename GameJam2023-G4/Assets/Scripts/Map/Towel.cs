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
        bool isBigTowel;

        public bool IsLooted { get => isLooted; set => isLooted = value; }
        public Loot Loot { get => loot; set => loot = value; }
        public bool IsBigTowel { get => isBigTowel; set => isBigTowel = value; }

        public Towel(Vector2Int firstPosition, Vector2Int secondPosition, Tile skin, Tile lootedSkin, bool isLooted, Tilemap towelTiles, int mapType, bool isBigTowel)
        {
            this.isBigTowel = isBigTowel;
            this.loot = GenerateLoot(Random.Range(0, 100) < 10f);
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
                loot = new Loot(new Item[1] { GenerateMoneyItem() });
            }
            return loot;
        }

        Item GenerateWaterItem()
        {

            if (isBigTowel)
            {
                return new Item(Random.Range(0.7f * (mapType + 1) * 2, 1.5f * (mapType + 1)) * 2, ItemType.Water);
            }
            return new Item(Random.Range(0.7f * (mapType + 1), 1.5f * (mapType + 1)), ItemType.Water);
        }
        Item GenerateMoneyItem()
        {
            if (isBigTowel)
            {
                return new Item(10 * (mapType + 1) * 2, ItemType.Money);
            }
            return new Item(10 * (mapType + 1), ItemType.Money);
        }
    }
}

