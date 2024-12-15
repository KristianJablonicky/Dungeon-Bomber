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
    [SerializeField] private TMP_Text youDiedText;

    [SerializeField] private List<Button> deathScreenButtons;

    void Start()
    {
        Dungeon.instance.getPlayer().defeated += runEnded;
        
        var boss = Dungeon.instance.getBoss();
        if (boss != null)
        {
            boss.defeated += runEnded;
        }

        foreach (var button in deathScreenButtons)
        {
            button.enabled = false;
        }
    }

    private void runEnded(object sender, System.EventArgs e)
    {
        string floorString = "", runString = "Defeat the final boss to start tracking your fastest time!";
        var records = storeDataOnDeath(sender);

        if (records.highestFloor)
        {
            floorString = " (new record!)";
        }

        if (records.bestTime)
        {
            runString = $"That's your new personal best!";
            if (records.previousBest != -1)
            {
                runString += $" (previous best was {records.previousBest})";
            }
        }
        else if (DataStorage.instance.highScore != -1)
        {
            runString = $"Fastest run: {DataStorage.instance.highScore}";
        }

        summary.text = $"Floor reached: {Dungeon.instance.getFloor()}{floorString}\n" +
            $"Run length: {DataStorage.instance.currentBeats}\n" +
            $"{runString}\n" +
            $"Gold gained: {Currencies.instance.getGold() - DataStorage.instance.startingGold}\n" +
            $"Player level reached: {Dungeon.instance.getPlayer().getPlayerLevel()}";

        foreach(var button in deathScreenButtons)
        {
            button.enabled = true;
        }

        StartCoroutine(slowDownTime());
        StartCoroutine(youDiedFade(sender is Boss));
    }
    private class NewRecords
    {
        public bool highestFloor = false, bestTime = false;
        public int previousBest = -1;
    }
    private NewRecords storeDataOnDeath(object sender)
    {
        var record = new NewRecords();
        var highestFloor = PlayerPrefs.GetInt("HighestFloorReached");
        if (Dungeon.instance.getFloor() > highestFloor)
        {
            PlayerPrefs.SetInt("HighestFloorReached", Dungeon.instance.getFloor());
            record.highestFloor = true;
        }

        if (sender is Boss)
        {
            int currentBeats = DataStorage.instance.currentBeats, highScore = DataStorage.instance.highScore;
            if (currentBeats < highScore ||
                highScore == -1)
            {
                record.bestTime = true;
                record.previousBest = highScore;
                DataStorage.instance.highScore = currentBeats;
                PlayerPrefs.SetInt("HighScore", currentBeats);
            }
        }

        return record;
    }

    private IEnumerator slowDownTime()
    {
        float timeElapsed = 0f, duration = 2f;
        while (timeElapsed < duration)
        {
            Time.timeScale = 1f - timeElapsed / duration;
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 0f;
    }

    private IEnumerator youDiedFade(bool boss)
    {
        audioSource.volume = PlayerPrefs.GetFloat("musicVolume");
        audioSource.Play();
        float timeElapsed = 0f, fadeInTime = 3f, holdTime = 1f, fadeOutTime = 1f, fadeInScreenTime = 0.5f;


        if (boss)
        {
            youDiedText.text = "GREAT ENEMY FELLED";
            youDiedText.color = Color.yellow;
        }


        // You died fade in
        while (timeElapsed < fadeInTime)
        {
            timeElapsed += Time.unscaledDeltaTime;
            youDiedGraphic.alpha = timeElapsed / fadeInTime;
            fade.alpha = timeElapsed / fadeInTime * 0.5f;
            yield return null;
        }
        
        
        // You died full alpha
        youDiedGraphic.alpha = 1f;
        fade.alpha = 0.5f;
        yield return new WaitForSecondsRealtime(holdTime);

        // You died fade out
        timeElapsed = 0f;
        while (timeElapsed < fadeOutTime)
        {
            timeElapsed += Time.unscaledDeltaTime;
            youDiedGraphic.alpha = 1f - timeElapsed / fadeOutTime;
            yield return null;
        }
        youDiedGraphic.alpha = 0f;

        // Run recap screen
        timeElapsed = 0f;
        while(timeElapsed < fadeInScreenTime)
        {
            timeElapsed += Time.unscaledDeltaTime;
            deathScreen.alpha = timeElapsed / fadeInScreenTime;
            yield return null;
        }
        deathScreen.alpha = 1f;
    }

    public void reset()
    {
        Time.timeScale = 1f;
        Dungeon.instance.reset();
    }

    public void goToMainMenu()
    {
        Time.timeScale = 1f;
        Dungeon.instance.reset();
        SceneManager.LoadScene("Menu");
    }

}
