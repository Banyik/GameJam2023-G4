using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SpriteRenderer sprite;
    public Sprite hoverSprite;
    public void OnPointerEnter(PointerEventData eventData)
    {
        sprite.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ClearSprite();
    }

    public void ClearSprite()
    {
        sprite.sprite = null;
    }
}
