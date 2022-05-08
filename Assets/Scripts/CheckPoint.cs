using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private List<Sprite[]> sheets;
    private SpriteRenderer spriteRenderer;

    private bool animStarted = false;
    private float timeBeforeNextFrame;
    private float timerAnim = 0f;
    
    private int currentFrame = 0;

    private AudioSource audioSource;

    public void Go(List<Sprite[]> list, float[] scale, float delay, AudioClip sound, float volume)
    {
        timeBeforeNextFrame = delay;
        sheets = list;
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = sheets[0][currentFrame];
        spriteRenderer.sprite = sprite;

        //Add boxCollider
        BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;

        //Add AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.clip = sound;

        //Scalling
        changeLocalScale(scale);

    }

    void Update()
    {
        if(!animStarted)
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
        currentFrame++;
        if(currentFrame == 13)
        {
            // goes back to 1, because first sprite is unlighted torch
            currentFrame = 1;
        }
        spriteRenderer.sprite = sheets[0][currentFrame];
    }

    private void changeLocalScale(float[] scale)
    {
        transform.localScale = new Vector3(scale[0], scale[1], 1f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Only useable once
        if(!animStarted)
        {
            if(other.tag == "Player")
            {
                GameManager.SavePos(transform.position);
                audioSource.Play();

                // Start anim torch
                animStarted = true;
            }
        }
    }
}
