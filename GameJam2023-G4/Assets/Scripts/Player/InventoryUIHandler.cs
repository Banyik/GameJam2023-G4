using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIHandler : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject[] slots;
    public Text[] slotTexts;
    public Text moneyText;

    public void SetMoney(float money)
    {
        moneyText.text = money.ToString();
    }

    public void SetItem(int slot, Items.ItemType type, int amount)
    {
        if(amount > 0)
        {
            Sprite sprite = GetSprite(type);
            slotTexts[slot].text = amount.ToString();
            slots[slot].GetComponent<Image>().sprite = sprite;
            slots[slot].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            slotTexts[slot].text = "";
            slots[slot].GetComponent<Image>().sprite = null;
            slots[slot].GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
    }

    public void HideInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slotTexts[i].text = "";
            slots[i].GetComponent<Image>().sprite = null;
            slots[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
    }

    Sprite GetSprite(Items.ItemType type)
    {
        switch (type)
        {
            case Items.ItemType.Corn:
                return sprites[0];
            case Items.ItemType.IceCream:
                return sprites[1];
            case Items.ItemType.Langos:
                return sprites[2];
            default:
                return null;
        }
    }
}
