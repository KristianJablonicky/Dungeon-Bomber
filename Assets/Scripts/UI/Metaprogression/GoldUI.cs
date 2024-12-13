using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldValue;
    [SerializeField] private Image goldIcon;

    [SerializeField] private Sprite[] goldAnimationSprites;
    private int currentGold;
    private readonly float duration = 0.5f;
    private void Awake()
    {
        var currencies = Currencies.instance;
        currentGold = currencies.getGold();
        goldValue.text = currentGold.ToString();
        currencies.goldChanged += updateText;
    }

    private void updateText(object sender, CurrencyArgs e)
    {
        StopAllCoroutines();
        StartCoroutine(incrementNumber(currentGold, e.gain));
        StartCoroutine(tossCoin());
        currentGold += e.gain;
    }

    private IEnumerator incrementNumber(int start, int gain)
    {
        float frequency = duration / Mathf.Abs(gain);
        for (int i = 1; i <= Math.Abs(gain); i++)
        {
            goldValue.text = (start + i * Math.Sign(gain)).ToString();
            yield return new WaitForSeconds(frequency);
        }
    }

    private IEnumerator tossCoin()
    {
        float frequency = duration / (goldAnimationSprites.Length - 1);
        for (int i = 1; i < goldAnimationSprites.Length; i++)
        {
            goldIcon.sprite = goldAnimationSprites[i];
            yield return new WaitForSeconds(frequency);
        }
        goldIcon.sprite = goldAnimationSprites[0];
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
        Currencies.instance.goldChanged -= updateText;
    }
}

public class CurrencyArgs : EventArgs
{
    public int gain;
    public CurrencyArgs(int gain)
    {
        this.gain = gain;
    }
}
