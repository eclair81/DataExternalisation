using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager : object
{
    public static int livesLeft;
    public static int coinNumber = 0;
    public static int coinForExtraLife;

    private static Vector2 playerSavedPos;
    private static int currentStage = 0;

    public static void GainCoin()
    {
        coinNumber++;
        if(coinNumber == coinForExtraLife)
        {
            Debug.Log("Yay, an extra life!");
            coinNumber = 0;
            livesLeft++;
        }
        //Debug.Log("Picked up a coin! Current number of coins: " + coinNumber);
    }

    public static void SavePos(Vector2 pos)
    {
        //Debug.Log("Checkpoint!");
        playerSavedPos = pos;
    }

    public static Vector2 GetPos()
    {
        return playerSavedPos;
    }

    public static void NextLevel()
    {
        currentStage++;
        if(currentStage == JsonReader.Instance.maps.Length)
        {
            Debug.Log("gégé, c gagné!!");
            return;
        }

        Debug.Log("Loading next level...");
        LevelGenerator.Instance.GenerateLevel(JsonReader.Instance.maps[currentStage]);
    }
}
