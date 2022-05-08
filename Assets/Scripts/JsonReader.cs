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
    public List<Sprite[]> pSheets;
    public AudioClip deathSound;
    public AudioClip lifeSound;


    void Awake()
    {
        Instance = this;

        //Read Json
        player = JsonUtility.FromJson<Player>(playerJson.text);
        stages = JsonUtility.FromJson<Stages>(stagesJson.text);

        //Process Json
        //player
        GameManager.SetInfos();
        pSheets = new List<Sprite[]>();

        // only 1 src -> use info to extract all the sheets
        if(player.src.Length == 1)
        {
            Texture2D texture;
            try {
                texture = duplicateTexture(Resources.Load<Texture2D>(player.src[0]));
                texture.name = player.src[0];

                for(int i = 0; i < 5; i++)
                {
                    Sprite[] sheet = SpriteSheetCreator.ExtractSpriteSheet(texture, player.dim[0], player.dim[1], player.info[i*3], player.info[i*3 + 1], player.info[i*3 + 2]);
                    pSheets.Add(sheet);
                } 

            } catch {
                //Debug.Log("There is no texture for " + player.src[0]);
                //Add empty spritesheets
                for(int i = 0; i < 5; i++)
                {
                    pSheets.Add(new Sprite[0]);
                }
            }
        }
        // one sheet for each animation
        if(player.src.Length == 5)
        {
            foreach (string str in player.src)
            {
                Texture2D texture;
                try {
                    texture = duplicateTexture(Resources.Load<Texture2D>(str));
                    texture.name = str;
                } catch {
                    //Debug.Log("There is no texture for " + str);
                    //Add empty spritesheet
                    pSheets.Add(new Sprite[0]);
                    //skip to next
                    continue;
                }

                Sprite[] sheet = SpriteSheetCreator.CreateSpriteSheet(texture, player.dim[0], player.dim[1]);
                pSheets.Add(sheet);
            }
        }

        lifeSound = null;
        if(player.extraLifeSound != null)
        {
            lifeSound = Resources.Load<AudioClip>(player.extraLifeSound);
        }

        deathSound = null;
        if(player.deathSound != null)
        {
            deathSound = Resources.Load<AudioClip>(player.deathSound);
        }


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
                foreach(string str in elem.src)
                {

                    Texture2D texture;
                    try {
                        texture = duplicateTexture(Resources.Load<Texture2D>(str));
                        texture.name = str;

                        Sprite[] sheet;
                        if(elem.info != null)
                        {
                            for(int i = 0; i < (elem.info.Length / 3); i++)
                            {
                                //Debug.Log("Starting Extraction");
                                sheet = SpriteSheetCreator.ExtractSpriteSheet(texture, elem.dim[0], elem.dim[1], elem.info[i*3], elem.info[i*3 + 1], elem.info[i*3 + 2]);
                                //Debug.Log("Ending Extraction, number of sprites in sheet: " + sheet.Length);
                                allSprites.Add(sheet);
                            }
                        }
                        else
                        {
                            //Debug.Log("Trying to transform a full texture (" + elem.src[i] +") in sprite[] of " + elem.dim[0] + " * " + elem.dim[1] + " pixels...");
                            sheet = SpriteSheetCreator.CreateSpriteSheet(texture, elem.dim[0], elem.dim[1]);
                            allSprites.Add(sheet);
                        }
                    } catch {
                        //Debug.Log("There is no texture for " + elem.src[i]);
                        //Add empty spritesheet
                        allSprites.Add(new Sprite[0]);
                        //skip to next
                        continue;
                    }
                }

                // Audio
                AudioClip audio = null;
                if(elem.sound != null)
                {
                    audio = Resources.Load<AudioClip>(elem.sound);
                }
                float volume = 1f;
                if(elem.volume != null)
                {
                    volume = elem.volume[0];
                }

                ElemDico elemDico = new ElemDico(elem.type, allSprites, elem.dim, elem.scale, elem.animDelay, audio, volume);
                dicoMapping.Add(elem.rvb, elemDico);
            }
            else
            {
                AudioClip audio = null;
                if(elem.sound != null)
                {
                    audio = Resources.Load<AudioClip>(elem.sound);
                }
                float volume = 1f;
                if(elem.volume != null)
                {
                    volume = elem.volume[0];
                }

                // if there is no sprites for that element (spawnpoint), we still add it into the dictionary, but with an empty list of Sprite[]
                ElemDico elemDico = new ElemDico(elem.type, new List<Sprite[]>(), new int[2]{0, 0}, new float[2]{0, 0}, 0, audio, volume);
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
