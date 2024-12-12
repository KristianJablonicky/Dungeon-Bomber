using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public void playButton()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void exitButton()
    {
        Application.Quit();
    }

    public void loreButton()
    {
        
    }

    public void bestiaryButton()
    {
        // SceneManager.LoadScene("Bestiary");
    }

    public void mainMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
