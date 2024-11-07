using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image healthBar, flash;
    [SerializeField] private TMP_Text healthBarText;

    private Boss boss;
    float timeElapsed = 0f, duration = 2f;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        enabled = false;
    }

    public void setUp(Boss boss)
    {
        this.boss = boss;
        boss.hpChanged += onHpChange;
        enabled = true;
    }

    private void Update()
    {
        if (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Pow(timeElapsed / duration, 2f);
        }
        else
        {
            canvasGroup.alpha = 1f;
            enabled = false;
        }

    }

    private void onHpChange(object sender, DamageArgs e)
    {
        int maxHp = Mathf.Max(boss.getMaxHp(), boss.getHp());
        StopAllCoroutines();
        StartCoroutine(hpTransition(flash.fillAmount, (float)boss.getHp() / maxHp));
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
    private void updateHealthBarText()
    {
        healthBarText.text = $"{boss.getHp()}/{boss.getMaxHp()}";
    }
}
