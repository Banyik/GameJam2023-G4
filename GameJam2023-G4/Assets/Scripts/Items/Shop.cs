using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Maps;

namespace Items
{
    public class Shop : MonoBehaviour
    {
        public Player.Behaviour playerBehaviour;
        public GameHandler handler;
        public WalkableGrid walkableGrid;

        public Button buyCorn;
        public Button buyIceCream;
        public Button buyLangos;
        public Button buyStrawberrySyrup;
        public Button buyButton;

        public Text myMoney;
        ItemType type;
        float price;

        private void Start()
        {
            buyCorn.onClick.AddListener(delegate { SetItem("2"); });
            buyCorn.onClick.AddListener(delegate { SetPrice("30"); });

            buyIceCream.onClick.AddListener(delegate { SetItem("3"); });
            buyIceCream.onClick.AddListener(delegate { SetPrice("30"); });

            buyLangos.onClick.AddListener(delegate { SetItem("4"); });
            buyLangos.onClick.AddListener(delegate { SetPrice("30"); });

            buyStrawberrySyrup.onClick.AddListener(delegate { SetItem("5"); });
            buyStrawberrySyrup.onClick.AddListener(delegate { SetPrice("30"); });
        }

        public void Continue()
        {
            playerBehaviour.player.Money = 0;
            walkableGrid.ResetMap();
        }

        public void SetItem(string type)
        {
            this.type = (ItemType)System.Convert.ToInt32(type);
        }
        public void SetPrice(string price)
        {
            this.price = System.Convert.ToInt32(price);
        }

        public void CheckIfTransactionIsPossible()
        {
            if(GetFreeSlot(type) > -1 && HasEnoughMoney(price))
            {
                buyButton.interactable = true;
            }
            else
            {
                buyButton.interactable = false;
            }
        }

        public void BuyItem()
        {
            int index = GetFreeSlot(type);
            if(index > -1 && type != ItemType.StrawberrySyrup)
            {
                if (HasEnoughMoney(price))
                {
                    if (playerBehaviour.player.Items[index] == null)
                    {
                        playerBehaviour.player.Items[index] = new Item(1, type);
                    }
                    else
                    {
                        playerBehaviour.player.Items[index].Amount++;
                    }
                    ExecuteTransaction();
                }
            }
            else if (type == ItemType.StrawberrySyrup && HasEnoughMoney(price))
            {
                ExecuteTransaction();
                playerBehaviour.player.MaxThirst += playerBehaviour.player.MaxThirst * .1f;
                playerBehaviour.thirstBarBehaviour.SetAnimationSpeed(playerBehaviour.player.MaxThirst);
            }
        }

        void ExecuteTransaction()
        {
            handler.buyScore -= (int)price;
            RefreshMyMoney();
        }

        public void RefreshMyMoney()
        {
            myMoney.text = handler.buyScore.ToString();
        }

        public bool HasEnoughMoney(float price)
        {
            if(handler.buyScore >= price)
            {
                return true;
            }
            return false;
        }

        public int GetFreeSlot(ItemType type)
        {
            int index = -1;
            for (int i = 0; i < playerBehaviour.player.Items.Length; i++)
            {
                if(playerBehaviour.player.Items[i] != null && playerBehaviour.player.Items[i].Type == type)
                {
                    return i;
                }
                else if(playerBehaviour.player.Items[i] == null)
                {
                    if(index == -1)
                    {
                        index = i;
                    }
                }
            }
            return index;
        }
    }
}
