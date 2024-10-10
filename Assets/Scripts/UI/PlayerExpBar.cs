using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerExpBar : MonoBehaviour
{
    [SerializeField] private Image expBar, flash;
    [SerializeField] private TMP_Text expBarText;
    private Player player;

    void Start()
    {
        Dungeon.instance.playerSpawned += setUpPlayer;
    }
    private void setUpPlayer(object sender, System.EventArgs e)
    {
        this.player = (Player)sender;
        player.onExpChange += onExpChange;
        onExpChange(player,  EventArgs.Empty);
    }

    private void updateExpBarText()
    {
        expBarText.text = $"{player.getExpCount()}/{player.getExpThreshhold()}";
    }

    private void onExpChange(object sender, EventArgs e)
    {
        int maxHp = Mathf.Max(player.getExpThreshhold(), player.getExpCount());
        StopAllCoroutines();
        StartCoroutine(expTransition(flash.fillAmount, (float)player.getExpCount() / maxHp));
        updateExpBarText();
    }

    private IEnumerator expTransition(float startingExp, float endingExp)
    {
        setFill(expBar, endingExp);
        setFill(flash, startingExp);
        float timeElapsed = 0f, duration = Mathf.Abs(startingExp - endingExp);
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            setFill(flash, Mathf.Lerp(startingExp, endingExp, timeElapsed / duration));
            yield return null;
        }
        setFill(flash, endingExp);
    }

    private void setFill(Image bar, float fillAmount)
    {
        bar.fillAmount = fillAmount;
    }
}
