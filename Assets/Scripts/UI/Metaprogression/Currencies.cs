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

    public void increaseGold(int gainAmount)
    {
        gold += gainAmount;
        goldChanged?.Invoke(this, new(gainAmount));
        PlayerPrefs.SetInt("Gold", gold);
    }

    public void increaseGold(int gainAmount, GameObject spawn)
    {
        increaseGold(gainAmount);
        GoldTosser.instance.tossGold(spawn, gainAmount);
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
