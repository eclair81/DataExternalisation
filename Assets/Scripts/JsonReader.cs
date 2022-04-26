using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    [Header("JsonData")]
    public TextAsset playerJson;
    public TextAsset stagesJson;
    //public TextAsset enemisJson;

    [Header("Things for other Scripts")]
    public Player player;
    public Stages stages;

    // Start is called before the first frame update
    void Start()
    {
        player = JsonUtility.FromJson<Player>(playerJson.text);
        stages = JsonUtility.FromJson<Stages>(stagesJson.text);
    }
}
