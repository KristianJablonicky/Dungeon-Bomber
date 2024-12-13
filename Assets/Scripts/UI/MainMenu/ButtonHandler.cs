using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject popUpContainer, bestiary;
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
        createPopUp(bestiary);
    }

    public void mainMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }

    private void createPopUp(GameObject go)
    {
        var instance = Instantiate(go, popUpContainer.transform);
    }
}
