using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private Elem[] elems;

    void Start()
    {
        elems = JsonReader.Instance.stages.colorMapping;
        //Test, print all the pixel of stage1
        GenerateLevel(JsonReader.Instance.maps[0]);
    }

    public void GenerateLevel(Texture2D map)
    {
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
        Debug.Log(pixel);
    }
}
