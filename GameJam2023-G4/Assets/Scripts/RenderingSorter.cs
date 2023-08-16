using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingSorter : MonoBehaviour
{
    int orderBase = 9000;
    int offset;
    Renderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<Renderer>();
    }
    private void LateUpdate()
    {
        spriteRenderer.sortingOrder = (int)(orderBase - transform.position.y - offset);
    }
}
