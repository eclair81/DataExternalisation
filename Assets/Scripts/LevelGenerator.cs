using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    //private Elem[] colorMapping;
    private int threshold;
    private Dictionary<int[], List<Sprite[]>> dicoMapping;
    

    void Start()
    {
        //colorMapping = JsonReader.Instance.stages.colorMapping;
        threshold = JsonReader.Instance.stages.colorFidelity;
        dicoMapping = JsonReader.Instance.dicoMapping;
        

        //Test, print all the pixel of stage1
        //GenerateLevel(JsonReader.Instance.maps[0]);
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
