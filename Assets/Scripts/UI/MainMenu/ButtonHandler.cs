using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject popUpContainer, bestiary, lore, runeForge;
    [SerializeField] private GameObject fade;
    private static ButtonHandler instance;

    private void Awake()
    {
        instance = this;
    }

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
        createPopUp(lore);
    }

    public void runeForgeButton()
    {
        createPopUp(runeForge);
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
        fade.SetActive(true);
    }
    public static void hideFade()
    {
        instance.fade.SetActive(false);
    }
}
