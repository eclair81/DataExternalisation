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
    public Dictionary<int[], ElemDico> dicoMapping;

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

        // Creating dictionary
        dicoMapping = new Dictionary<int[], ElemDico>();
        foreach (Elem elem in stages.colorMapping)
        {
            //Debug.Log(elem.nom);

            if(elem.src != null)
            {
                //Texture2D[] textures = new Texture2D[elem.src.Length];
                List<Sprite[]> allSprites = new List<Sprite[]>();
                for(int i = 0; i < elem.src.Length; i++)
                {

                    Texture2D texture;
                    try {
                        texture = duplicateTexture(Resources.Load<Texture2D>(elem.src[i]));
                        texture.name = elem.src[i];
                    } catch {
                        //Debug.Log("There is no texture for " + elem.src[i]);
                        //Add empty spritesheet
                        allSprites.Add(new Sprite[0]);
                        //skip to next
                        continue;
                    }

                    Sprite[] sheet;
                    /*
                    string res = "[";
                    foreach (int nb in elem.info)
                    {
                        res += nb.ToString() + ", ";
                    }
                    res += "]";
                    Debug.Log(elem.nom + " a pour info: " + res);
                    */
                    if(elem.info != null)
                    {
                        //Debug.Log("Starting Extraction");
                        sheet = SpriteSheetCreator.ExtractSpriteSheet(texture, elem.dim[0], elem.dim[1], elem.info[i*3], elem.info[i*3 + 1], elem.info[i*3 + 2]);
                        //Debug.Log("Ending Extraction, number of sprites in sheet: " + sheet.Length);
                    }
                    else
                    {
                        //Debug.Log("Trying to transform a full texture (" + elem.src[i] +") in sprite[] of " + elem.dim[0] + " * " + elem.dim[1] + " pixels...");
                        sheet = SpriteSheetCreator.CreateSpriteSheet(texture, elem.dim[0], elem.dim[1]);
                    }

                    allSprites.Add(sheet);
                }

                ElemDico elemDico = new ElemDico(elem.type, allSprites);
                dicoMapping.Add(elem.rvb, elemDico);
                //Debug.Log("Added into dictionary: " + elem.nom + " [" + elem.rvb[0] + ", " + elem.rvb[1] + ", " + elem.rvb[2] + "] \n It contains " + elemDico.sheets.Count + "spriteSheets \n the first spriteSheet has " + elemDico.sheets[0].Length + " sprites in it");
            }
            else
            {
                // if there is no sprites for that element (spawnpoint), we still add it into the dictionary, but with an empty list of Sprite[]
                ElemDico elemDico = new ElemDico(elem.type, new List<Sprite[]>());
                dicoMapping.Add(elem.rvb, elemDico);
            }
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
