using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    private int threshold;
    private Dictionary<int[], ElemDico> dicoMapping;
    
    [Header("Prefabs")]
    public GameObject blocPrefab;
    public GameObject collectiblePrefab;
    public GameObject checkPointPrefab;
    public GameObject endPointPrefab;

    [Header("Player")]
    public Transform player;

    [Header("Stage")]
    public Transform parent;

    void Start()
    {
        Instance = this;

        threshold = JsonReader.Instance.stages.colorFidelity;
        dicoMapping = JsonReader.Instance.dicoMapping;
        

        //Test, print all the pixel of stage1
        GenerateLevel(JsonReader.Instance.maps[0]);
    }

    public void GenerateLevel(Texture2D map)
    {
        ClearLevel();
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateTile(x, y, map);
            }
        }
    }

    public void GenerateTile(int x, int y, Texture2D map)
    {
        Color pixel = map.GetPixel(x, y);

        if(pixel.a == 0)
        {   
            // ignore transparent pixels
            return;
        }
        //Debug.Log(pixel);

        foreach (KeyValuePair<int[], ElemDico> entry in dicoMapping)
        {
            if(ColorsAreClose(entry.Key, pixel))
            {

                //Debug.Log(pixel + " has been recognized as [" + entry.Key[0] + ", " + entry.Key[1] + ", " + entry.Key[2] + "]");

                switch (entry.Value.type)
                {
                    case "spawnPoint":
                        Vector2 start = new Vector2(x, y);
                        GameManager.SavePos(start);
                        player.position = start;
                        
                        return;

                    case "checkPoint":
                        GameObject torch = Instantiate(checkPointPrefab, new Vector2(x, y), Quaternion.identity, parent);
                        CheckPoint torchScript = torch.GetComponent<CheckPoint>();
                        torchScript.Go(entry.Value.sheets, entry.Value.scale, entry.Value.animDelay, entry.Value.sound, entry.Value.volume);

                        return;
                    
                    case "endPoint":
                        GameObject end = Instantiate(endPointPrefab, new Vector2(x, y), Quaternion.identity, parent);
                        EndPoint endScript = end.GetComponent<EndPoint>();
                        endScript.Go(entry.Value.sheets, entry.Value.scale, entry.Value.animDelay);

                        return;

                    case "bloc":
                        Debug.Log("spawn bloc at " + x + " " + y);
                        GameObject bloc = Instantiate(blocPrefab, new Vector2(x, y), Quaternion.identity, parent);
                        Bloc blocScript = bloc.GetComponent<Bloc>();

                        //look at this bloc's up/down/left/right neighbor to determine which sprite from which sheet display
                        bool up, down, left, right;
                        
                        if(y == map.height-1)
                        {
                            up = false;
                        }
                        else
                        {
                            try{
                                Color neighbor = map.GetPixel(x, y+1);
                                if(neighbor.a != 0)
                                {
                                    up = ColorsAreClose(pixel, neighbor);
                                }
                                else
                                {
                                    up = false;
                                }
                            }catch{
                                up = false;
                            }
                        }
                        
                        if(y == 0)
                        {
                            down = false;
                        }
                        else{
                            try{
                                Color neighbor = map.GetPixel(x, y-1);
                                if(neighbor.a != 0)
                                {
                                    down = ColorsAreClose(pixel, neighbor);
                                }
                                else
                                {
                                    down = false;
                                }
                            }catch{
                                down = false;
                            }
                        }

                        if(x == 0)
                        {
                            left = false;
                        }
                        else{
                            try{
                                Color neighbor = map.GetPixel(x-1, y);
                                if(neighbor.a != 0)
                                {
                                    left = ColorsAreClose(pixel, neighbor);
                                    //Debug.Log("left neighbor of "+ x +" "+ y + " is " + (x-1)+" "+ y+"\n It's color is " + neighbor + "\n same color? : " + left.ToString());
                                    
                                }
                                else
                                {
                                    left = false;
                                }
                            }catch{
                                left = false;
                            }
                        }

                        if(x == map.width-1)
                        {
                            right = false;
                        }
                        else
                        {
                            try{
                                Color neighbor = map.GetPixel(x+1, y);
                                if(neighbor.a != 0)
                                {
                                    right = ColorsAreClose(pixel, neighbor);
                                }
                                else
                                {
                                    right = false;
                            }
                            }catch{
                                right = false;
                            }
                        }
                        



                        int[] res = WhichSprite(up, down, left, right);
                        Debug.Log(x + ", " + y + "has sprite: " + res[0] + ", " + res[1] + "\n neighbors: up: " + up.ToString() + " down: " + down.ToString() + "  left: " + left.ToString() + "  right: " + right.ToString());

                        blocScript.Go(entry.Value.sheets, res[0], res[1]);
                        
                        return;

                    case "collectible":
                        GameObject coin = Instantiate(collectiblePrefab, new Vector2(x, y), Quaternion.identity, parent);
                        Collectible coinScript = coin.GetComponent<Collectible>();
                        coinScript.Go(entry.Value.sheets, entry.Value.scale, entry.Value.animDelay, entry.Value.sound, entry.Value.volume);

                        return;

                    default:
                        Debug.Log("Incorrect type (" + entry.Value.type +") for [" + entry.Key[0] + ", " + entry.Key[1] + ", " + entry.Key[2] + "]");
                        return;
                }
            }
        }
        //Debug.Log(pixel + " at x: " + x + ", and y: "+ y +" hasn't been recognized...");
    }

    private void ClearLevel()
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }


    // takes two colors: one int[3](0-255) -> rgb and one Unity type of color (rgba)(0f-1f)
    // return true if the two colors are close enought to each other
    // how close is determined by a threshold
    private bool ColorsAreClose(int[] rvb, Color c/*, int threshold = 10*/)
    {
        float  r = rvb[0] - (c.r * 255),
            g = rvb[1] - (c.g * 255),
            b = rvb[2] - (c.b * 255);
        return (r*r + g*g + b*b) <= threshold*threshold;
    }

    private bool ColorsAreClose(Color c1, Color c2/*, int threshold = 10*/)
    {
        float  r = (c1.r * 255) - (c2.r * 255),
            g = (c1.g * 255) - (c2.g * 255),
            b = (c1.b * 255) - (c2.b * 255);
        return (r*r + g*g + b*b) <= threshold*threshold;
    }

    private int[] WhichSprite(bool up, bool down, bool left, bool right)
    {
        switch(up, down, left, right)
        {
            case (false, false, false, false):
                return new int[2] {0, 1};
            case (false, false, false, true):
                return new int[2] {0, 0};
            case (false, false, true, false):
                return new int[2] {0, 2};
            case (false, false, true, true):
                return new int[2] {0, 1};
            case (false, true, false, false):
                return new int[2] {0, 1};
            case (false, true, false, true):
                return new int[2] {0, 0};
            case (false, true, true, false):
                return new int[2] {0, 2};
            case (false, true, true, true):
                return new int[2] {0, 1};
            case (true, false, false, false):
                return new int[2] {2, 1};
            case (true, false, false, true):
                return new int[2] {2, 0};
            case (true, false, true, false):
                return new int[2] {2, 2};
            case (true, false, true, true):
                return new int[2] {2, 1};
            case (true, true, false, false):
                return new int[2] {1, 1};
            case (true, true, false, true):
                return new int[2] {1, 0};
            case (true, true, true, false):
                return new int[2] {1, 2};
            case (true, true, true, true):
                return new int[2] {1, 1};
            default:
                return new int[2] {0, 0};
        }
    }

}
