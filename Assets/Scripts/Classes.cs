using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int life;
    public int coinForLife;
    public float acceleration;
    public float maxSpeed;
    public float deceleration;
    public float jumpHeight;
    public float coyoteTime;
    public string[] src; // Need 5 sheets: Idle, Run, Jump, Fall, Death (in that order)
    public int[] dim;
    public int[] info; // only required if there are multiple sheets to extract from a single set
    public float[] scale;
    public float animDelay;
}



public class Stages
{
    public string[] stageList;
    public int colorFidelity;
    public Elem[] colorMapping;
}

[System.Serializable]
public class Elem
{
    public string nom;
    public int[] rvb;
    public string[] src;
    public int[] info;
    public int[] dim;
    public float[] scale;
    public string type;
    public float animDelay;
    public string sound;
}


public class ElemDico
{
    public string type;
    public List<Sprite[]> sheets;
    public int[] dim;
    public float[] scale;
    public float animDelay;
    public AudioClip sound;

    public ElemDico(string t, List<Sprite[]> l, int[] d, float[] s, float ad, AudioClip ac)
    {
        type = t;
        sheets = l;
        dim = d;
        scale = s;
        animDelay = ad;
        sound = ac;
    }
}