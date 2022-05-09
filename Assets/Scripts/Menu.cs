using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    
    public void Commencer()
    {
        SceneManager.LoadScene("SceneJeu", LoadSceneMode.Single);
    }

    public void Quitter()
    {
        Application.Quit();
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

}
