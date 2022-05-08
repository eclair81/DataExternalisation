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
                        torchScript.Go(entry.Value.sheets, entry.Value.scale, entry.Value.animDelay, entry.Value.sound);

                        return;
                    
                    case "endPoint":
                        GameObject end = Instantiate(endPointPrefab, new Vector2(x, y), Quaternion.identity, parent);
                        EndPoint endScript = end.GetComponent<EndPoint>();
                        endScript.Go(entry.Value.sheets, entry.Value.scale, entry.Value.animDelay);

                        return;

                    case "bloc":
                        GameObject bloc = Instantiate(blocPrefab, new Vector2(x, y), Quaternion.identity, parent);
                        Bloc blocScript = bloc.GetComponent<Bloc>();
                        blocScript.Go(entry.Value.sheets, 0, 0);
                        
                        return;

                    case "collectible":
                        GameObject coin = Instantiate(collectiblePrefab, new Vector2(x, y), Quaternion.identity, parent);
                        Collectible coinScript = coin.GetComponent<Collectible>();
                        coinScript.Go(entry.Value.sheets, entry.Value.scale, entry.Value.animDelay, entry.Value.sound);

                        return;

                    case "foe1":
                        //things to do
                        return;

                    case "foe2":
                        //things to do
                        return;

                    case "foe3":
                        //things to do
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

}
