using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar, flash;
    [SerializeField] private TMP_Text healthBarText;
    private Player player;

    void Start()
    {
        Dungeon.instance.playerSpawned += setUpPlayer;
    }
    private void setUpPlayer(object sender, System.EventArgs e)
    {
        this.player = (Player)sender;
        player.onHpChange += onHpChange;
        onHpChange(player, new DamageArgs(0, damageTags.None));
    }

    private void updateHealthBarText()
    {
        healthBarText.text = $"{player.getHp()}/{player.getMaxHp()}";
    }

    private void onHpChange(object sender, DamageArgs e)
    {
        int maxHp = Mathf.Max(player.getMaxHp(), player.getHp());
        StopAllCoroutines();
        StartCoroutine(hpTransition(flash.fillAmount, (float)player.getHp() / maxHp));
        updateHealthBarText();
    }

    private IEnumerator hpTransition(float startingHp, float EndingHp)
    {
        setFill(healthBar, EndingHp);
        setFill(flash, startingHp);
        float timeElapsed = 0f, duration = Mathf.Abs(startingHp - EndingHp);
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            setFill(flash, Mathf.Lerp(startingHp, EndingHp, timeElapsed / duration));
            yield return null;
        }
        setFill(flash, EndingHp);
    }

    private void setFill(Image bar, float fillAmount)
    {
        bar.fillAmount = fillAmount;
    }
}
