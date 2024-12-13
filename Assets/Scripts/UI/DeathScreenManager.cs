using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup youDiedGraphic, deathScreen, fade;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TMP_Text summary;

    void Start()
    {
        Dungeon.instance.getPlayer().defeated += playerDead;
    }

    private void playerDead(object sender, System.EventArgs e)
    {
        summary.text = $"Floor reached: {Dungeon.instance.getFloor()}\n" +
            $"Beats survived: {DataStorage.instance.currentBeats}\n" +
            $"Fastest run: {DataStorage.instance.highScore}\n" +
            $"Gold gained: {Currencies.instance.getGold()}\n" +
            $"Level reached: {Dungeon.instance.getPlayer().getPlayerLevel()}";

        StartCoroutine(youDiedFade());
    }

    private IEnumerator youDiedFade()
    {
        audioSource.volume = PlayerPrefs.GetFloat("musicVolume");
        audioSource.Play();
        float timeElapsed = 0f, fadeInTime = 3f, holdTime = 1f, fadeOutTime = 1f, fadeInScreenTime = 0.5f;
        while (timeElapsed < fadeInTime)
        {
            timeElapsed += Time.deltaTime;
            youDiedGraphic.alpha = timeElapsed / fadeInTime;
            fade.alpha = timeElapsed / fadeInTime * 0.5f;
            yield return null;
        }
        youDiedGraphic.alpha = 1f;
        fade.alpha = 0.5f;
        yield return new WaitForSeconds(holdTime);
        timeElapsed = 0f;
        while (timeElapsed < fadeOutTime)
        {
            timeElapsed += Time.deltaTime;
            youDiedGraphic.alpha = 1f - timeElapsed / fadeOutTime;
            yield return null;
        }
        youDiedGraphic.alpha = 0f;
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
