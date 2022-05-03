using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloc : MonoBehaviour
{
    public List<Sprite[]> sheets;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
        //sheets = new List<Sprite[]>();
    }

    public void Go()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //Test with just the first sprite for now
        Debug.Log("setting the sprite...");
        //Debug.Log("sheets value: " + sheets);
        //Debug.Log("First sprite: " + sheets[0][0]);
        //Debug.Log("sprite size: " + sheets[0][0].rect);
        Sprite sprite = sheets[0][0];
        Debug.Log("sprite:" + sprite);
        if(spriteRenderer == null)
        {
            Debug.Log("fuck");
        }
        else
        {
            Debug.Log("yay");
        }
        spriteRenderer.sprite = sprite;
        Debug.Log("sprite set!");

        //Add boxCollider
        BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
