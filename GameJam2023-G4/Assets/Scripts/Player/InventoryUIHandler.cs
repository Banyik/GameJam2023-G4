using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIHandler : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject[] slots;
    public Text[] slotTexts;

    public void SetItem(int slot, Items.ItemType type, int amount)
    {
        if(amount > 0)
        {
            Sprite sprite = GetSprite(type);
            slotTexts[slot].text = amount.ToString();
            slots[slot].GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else
        {
            slotTexts[slot].text = "";
            slots[slot].GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    public void HideInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slotTexts[i].text = "";
            slots[i].GetComponent<SpriteRenderer>().sprite = null;
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
