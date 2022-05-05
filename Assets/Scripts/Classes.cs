using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int life;
    public float speed;
    public float jumpHeight;
    public string spriteSheet;
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
    public string type;
    public float animDelay;
}


public class ElemDico
{
    public string type;
    public List<Sprite[]> sheets;
    public int[] dim;
    public float animDelay;

    public ElemDico(string t, List<Sprite[]> l, int[] d, float a)
    {
        type = t;
        sheets = l;
        dim = d;
        animDelay = a;
    }
}