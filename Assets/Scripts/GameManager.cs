using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using UnityEngine.SceneManagement;
using TMPro;

public static class GameManager : object
{
    public static int livesLeft;
    public static int coinNumber = 0;
    public static int coinForExtraLife;

    private static Vector2 playerSavedPos;
    private static int currentStage = 0;

    public static PlayerController player;

    private static TextMeshProUGUI lives;
    private static TextMeshProUGUI coins;

    public static void SetInfos()
    {
        livesLeft = JsonReader.Instance.player.life;
        coinForExtraLife = JsonReader.Instance.player.coinForLife;
        currentStage = 0;

        lives = GameObject.FindWithTag("LivesText").GetComponent<TextMeshProUGUI>();
        coins = GameObject.FindWithTag("CoinsText").GetComponent<TextMeshProUGUI>();

        UpdateHUD();
    }

    public static void GainCoin()
    {
        coinNumber++;
        if(coinNumber == coinForExtraLife)
        { 
            coinNumber = 0;
            livesLeft++;
            player.playLifeSound();
            //Debug.Log("Yay, an extra life! I have now " + livesLeft + " lifes");
        }
        //Debug.Log("Picked up a coin! Current number of coins: " + coinNumber);
        
        UpdateHUD();
    }

    public static void SavePos(Vector2 pos)
    {
        //Debug.Log("Checkpoint!");
        playerSavedPos = pos;
    }

    public static Vector2 GetSavedPos()
    {
        livesLeft--; // lose a live since this function is only called when the player dies.
        UpdateHUD();
        if(livesLeft == 0)
        {
            //Debug.Log("dead");
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
        return playerSavedPos;
    }

    public static void NextLevel()
    {
        currentStage++;
        if(currentStage == JsonReader.Instance.maps.Length)
        {
            //Debug.Log("gégé, c gagné!!");
            SceneManager.LoadScene("Fin", LoadSceneMode.Single);
            return;
        }

        //Debug.Log("Loading next level...");
        LevelGenerator.Instance.GenerateLevel(JsonReader.Instance.maps[currentStage]);
    }

    public static bool IsOutOfStage(Transform t)
    {
        return t.position.y < -2;
    }

    private static void UpdateHUD()
    {
        lives.text = livesLeft.ToString();
        coins.text = coinNumber.ToString();
    }
}
