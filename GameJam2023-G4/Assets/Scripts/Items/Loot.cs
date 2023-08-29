using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Loot
    {
        Item[] items;

        public Loot(Item[] items)
        {
            this.items = items;
        }

        public Item[] Items { get => items; set => items = value; }
    }
}

