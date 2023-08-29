using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Item
    {
        float amount;
        ItemType type;
        public Item(float amount, ItemType type)
        {
            this.amount = amount;
            this.type = type;
        }

        public float Amount { get => amount; set => amount = value; }
        public ItemType Type { get => type; set => type = value; }
    }
}

