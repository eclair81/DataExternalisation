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
    public int[][] info;
    public int[] dim;
    public string type;
}


public class ElemDico
{
    public string type;
    public List<Sprite[]> sheets;

    public ElemDico(string t, List<Sprite[]> l)
    {
        type = t;
        sheets = l;
    }
}