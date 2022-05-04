using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloc : MonoBehaviour
{
    public List<Sprite[]> sheets;
    private SpriteRenderer spriteRenderer;

    
    public void Go()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //Test with just the first sprite for now
        Sprite sprite = sheets[0][0];
        spriteRenderer.sprite = sprite;

        //Add boxCollider
        BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
