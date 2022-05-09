using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private List<Sprite[]> sheets;
    private SpriteRenderer spriteRenderer;

    private bool flagChecked = false;
    private float timeBeforeNextFrame;
    private float timerAnim = 0f;
    
    private int currentFrame = 0;
    private int currentSheet = 0;

    private AudioSource audioSource;

    public void Go(List<Sprite[]> list, float[] scale, float delay, AudioClip sound, float volume)
    {
        timeBeforeNextFrame = delay;
        sheets = list;
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = sheets[currentSheet][currentFrame];
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
        timerAnim += Time.deltaTime;
        if(timerAnim >= timeBeforeNextFrame)
        {
            NextFrame();
            timerAnim = 0f;
        }
    }

    private void NextFrame()
    {
        currentFrame = (currentFrame + 1) % sheets[currentSheet].Length;
        spriteRenderer.sprite = sheets[currentSheet][currentFrame];
    }

    private void changeLocalScale(float[] scale)
    {
        transform.localScale = new Vector3(scale[0], scale[1], 1f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Only useable once
        if(!flagChecked)
        {
            if(other.tag == "Player")
            {
                GameManager.SavePos(transform.position);
                audioSource.Play();

                // Start anim torch
                flagChecked = true;
                currentSheet = 1;
            }
        }
    }
}
