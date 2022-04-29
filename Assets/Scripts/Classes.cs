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
    public Elem[] colorMapping;
}

[System.Serializable]
public class Elem
{
    public string nom;
    public int[] rvb;
    public string src;
    public int[] info;
    public string type;
}