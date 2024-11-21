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
        // TODO: Add lore scene
        // SceneManager.LoadScene("Lore");
    }
}
