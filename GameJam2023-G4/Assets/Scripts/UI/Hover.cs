using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator anim;
    public SpriteRenderer sprite;
    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.ResetTrigger("EndHover");
        anim.SetTrigger("Hover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.ResetTrigger("Hover");
        anim.SetTrigger("EndHover");
        sprite.sprite = null;
    }
}
