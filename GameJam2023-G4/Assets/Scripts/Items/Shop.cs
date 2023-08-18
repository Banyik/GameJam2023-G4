using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;

namespace Items
{
    public class Shop : MonoBehaviour
    {
        public Player.Behaviour playerBehaviour;
        public GameHandler handler;
        public WalkableGrid walkableGrid;

        public void Continue()
        {
            playerBehaviour.player.Money = 0;
            walkableGrid.ResetMap();
        }
        public void BuyItem(ItemType type, float price)
        {
            int index = HasFreeSlot();
            if(index > -1)
            {
                if (HasEnoughMoney(price))
                {
                    if(type == ItemType.StrawBerrySyrup)
                    {
                        playerBehaviour.player.MaxThirst += playerBehaviour.player.MaxThirst * .1f;
                    }
                    else
                    {
                        playerBehaviour.player.Items[index] = new Item(1, type);
                    }
                }
            }
        }

        public bool HasEnoughMoney(float price)
        {
            if(handler.score >= price)
            {
                handler.score -= (int)price;
                return true;
            }
            return false;
        }

        public int HasFreeSlot()
        {
            int index = -1;
            for (int i = 0; i < playerBehaviour.player.Items.Length; i++)
            {
                if(playerBehaviour.player.Items[i] == null)
                {
                    index = i;
                }
            }
            return index;
        }
    }
}
