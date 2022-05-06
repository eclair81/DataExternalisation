using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private List<Sprite[]> sheets;
    private SpriteRenderer spriteRenderer;

    private bool go = false;
    private float timeBeforeNextFrame;
    private float timerAnim = 0f;
    
    private int currentFrame = 0;

    public void Go(List<Sprite[]> list, float[] scale, float delay)
    {
        timeBeforeNextFrame = delay;
        sheets = list;
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = sheets[0][currentFrame];
        spriteRenderer.sprite = sprite;

        //Add boxCollider
        BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;

        //Scalling
        changeLocalScale(scale);

        go = true;
    }

    void Update()
    {
        if(!go)
        {
            return;
        }

        timerAnim += Time.deltaTime;
        if(timerAnim >= timeBeforeNextFrame)
        {
            NextFrame();
            timerAnim = 0f;
        }
    }

    private void NextFrame()
    {
        currentFrame = (currentFrame + 1) % sheets[0].Length;
        spriteRenderer.sprite = sheets[0][currentFrame];
    }

    private void changeLocalScale(float[] scale)
    {
        transform.localScale = new Vector3(scale[0], scale[1], 1f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            GameManager.NextLevel();
        }
    }
}
