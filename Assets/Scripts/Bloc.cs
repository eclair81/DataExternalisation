using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloc : MonoBehaviour
{
    private List<Sprite[]> sheets;
    private SpriteRenderer spriteRenderer;

    
    public void Go(List<Sprite[]> list, int sheetNumber, int spriteNumber)
    {
        sheets = list;
        spriteRenderer = GetComponent<SpriteRenderer>();
        //Test with just the first sprite for now
        Sprite sprite = sheets[sheetNumber][spriteNumber];
        spriteRenderer.sprite = sprite;

        //Add boxCollider
        BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();

        //Set Ground Layer
        gameObject.layer = LayerMask.NameToLayer("Ground");

    }
}
