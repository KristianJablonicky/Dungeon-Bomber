using System;
using UnityEngine;

public class Currencies
{
    private int gold;
    public static Currencies instance = new Currencies();

    public event EventHandler<CurrencyArgs> goldChanged;

    private Currencies()
    {
        gold = PlayerPrefs.GetInt("Gold");
        
    }

    public void adjustGold(int adjustAmount)
    {
        gold += adjustAmount;
        goldChanged?.Invoke(this, new(adjustAmount));
        PlayerPrefs.SetInt("Gold", gold);
    }

    public void increaseGold(int gainAmount, GameObject spawn)
    {
        adjustGold(gainAmount);
        if (gainAmount > 0)
        {
            GoldTosser.instance.tossGold(spawn, gainAmount);
        }
    }

    public int getGold()
    {
        return gold;
    }

    public void storeData()
    {
        PlayerPrefs.SetInt("Gold", gold);
    }
}
