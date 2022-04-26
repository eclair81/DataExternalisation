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
}



public class ColorToElem
{
    public Elem[] colorToElem;
}

public class Elem
{
    public int r;
    public int v;
    public int b;
    public string src;
    public string type;
}