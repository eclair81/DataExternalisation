using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    public static JsonReader Instance;

    [Header("JsonData")]
    public TextAsset playerJson;
    public TextAsset stagesJson;
    //public TextAsset enemisJson;

    [Header("Things for other Scripts")]
    public Player player;
    public Stages stages;
    public Texture2D[] maps;

    void Awake()
    {
        Instance = this;

        //Read Json
        player = JsonUtility.FromJson<Player>(playerJson.text);
        stages = JsonUtility.FromJson<Stages>(stagesJson.text);

        //Process Json
        //maps
        maps = new Texture2D[stages.stageList.Length];
        for(int i = 0; i < maps.Length; i++)
        {
            Texture2D map = duplicateTexture(Resources.Load<Texture2D>(stages.stageList[i]));
            map.filterMode = FilterMode.Point;
            maps[i] = map;
        }

        
    }


    // Create a copy of the source texture
    // Necessary because by default an imported Texture isn't readable (need to change that in the inspector)
    // however, a newly created texture is readable.
    // Since I want every png put in Resources/Stages to be readable, I need to duplicate them all
    private Texture2D duplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);
    
        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}
