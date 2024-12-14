using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup youDiedGraphic, deathScreen, fade;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TMP_Text summary;

    [SerializeField] private List<Button> deathScreenButtons;

    void Start()
    {
        Dungeon.instance.getPlayer().defeated += playerDead;
        foreach (var button in deathScreenButtons)
        {
            button.enabled = false;
        }
    }

    private void playerDead(object sender, System.EventArgs e)
    {
        var records = storeDataOnDeath();
        string floorString = "";

        if (records.highestFloor)
        {
            floorString = " (new record!)";
        }

        summary.text = $"Floor reached: {Dungeon.instance.getFloor()}{floorString}\n" +
            $"Beats survived: {DataStorage.instance.currentBeats}\n" +
            $"Fastest run: {DataStorage.instance.highScore}\n" +
            $"Gold gained: {Currencies.instance.getGold()}\n" +
            $"Level reached: {Dungeon.instance.getPlayer().getPlayerLevel()}";

        foreach(var button in deathScreenButtons)
        {
            button.enabled = true;
        }

        StartCoroutine(youDiedFade());
    }
    private class NewRecords
    {
        public bool highestFloor = false, bestTime = false;
    }
    private NewRecords storeDataOnDeath()
    {
        var record = new NewRecords();
        var highestFloor = PlayerPrefs.GetInt("HighestFloorReached");
        if (Dungeon.instance.getFloor() > highestFloor)
        {
            PlayerPrefs.SetInt("HighestFloorReached", Dungeon.instance.getFloor());
            record.highestFloor = true;
        }

        return record;
    }

    private IEnumerator youDiedFade()
    {
        audioSource.volume = PlayerPrefs.GetFloat("musicVolume");
        audioSource.Play();
        float timeElapsed = 0f, fadeInTime = 3f, holdTime = 1f, fadeOutTime = 1f, fadeInScreenTime = 0.5f;

        // You died fade in
        while (timeElapsed < fadeInTime)
        {
            timeElapsed += Time.deltaTime;
            youDiedGraphic.alpha = timeElapsed / fadeInTime;
            fade.alpha = timeElapsed / fadeInTime * 0.5f;
            yield return null;
        }
        
        
        // You died full alpha
        youDiedGraphic.alpha = 1f;
        fade.alpha = 0.5f;
        yield return new WaitForSeconds(holdTime);

        // You died fade out
        timeElapsed = 0f;
        while (timeElapsed < fadeOutTime)
        {
            timeElapsed += Time.deltaTime;
            youDiedGraphic.alpha = 1f - timeElapsed / fadeOutTime;
            yield return null;
        }
        youDiedGraphic.alpha = 0f;

        // Run recap screen
        timeElapsed = 0f;
        while(timeElapsed < fadeInScreenTime)
        {
            timeElapsed += Time.deltaTime;
            deathScreen.alpha = timeElapsed / fadeInScreenTime;
            yield return null;
        }
        deathScreen.alpha = 1f;
    }

    public void reset()
    {
        Dungeon.instance.reset();
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
