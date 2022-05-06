using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloc : MonoBehaviour
{
    private List<Sprite[]> sheets;
    private SpriteRenderer spriteRenderer;

    
    public void Go(List<Sprite[]> list, int[] d)
    {
        sheets = list;
        spriteRenderer = GetComponent<SpriteRenderer>();
        //Test with just the first sprite for now
        Sprite sprite = sheets[0][0];
        spriteRenderer.sprite = sprite;

        //Add boxCollider
        BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();

        //Set Ground Layer
        gameObject.layer = LayerMask.NameToLayer("Ground");

        changeLocalScale(d);
    }

    private void changeLocalScale(int[] d)
    {
        transform.localScale = new Vector3(100f / d[0], 100f / d[1], 1f);
    }
    
}
