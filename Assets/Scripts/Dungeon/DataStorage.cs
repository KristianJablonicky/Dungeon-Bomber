using System.Collections.Generic;
using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public static DataStorage instance = null;
    public int floor = 0, playerHp, playerMaxHp, playerLevel, playerExp,
        highScore, currentBeats, startingGold;

    private List<Upgrade> upgrades;

    private void Awake()
    {
        // first awakening
        if (instance == null)
        {
            highScore = PlayerPrefs.GetInt("HighScore", -1);
            floor = 1;
            instance = this;
            DontDestroyOnLoad(this);
            upgrades = new List<Upgrade>();

            startingGold = Currencies.instance.getGold();

            Application.targetFrameRate = 144;
            /*
            if (Application.isMobilePlatform)
            {
                Application.targetFrameRate = 60;
            }
            else
            {
                Application.targetFrameRate = 144;
            }
            */
        }
        Metronome.instance.onBeat += onBeat;
    }

    private void onBeat(object sender, System.EventArgs e)
    {
        instance.currentBeats++;
    }

    public void addUpgrade(Upgrade upgrade)
    {
        upgrades.Add(upgrade);
    }

    public void equipUpgrades(Player player)
    {
        foreach (var upgrade in upgrades)
        {
            upgrade.equipEffect(player);
        }
    }

    public void reset()
    {
        floor = 0;
        upgrades = new List<Upgrade>();
        playerHp = 1;
        playerLevel = 0;
        playerExp = 0;
        currentBeats = 0;
        startingGold = Currencies.instance.getGold();
    }

    public void resetHighScore()
    {
        highScore = -1;
        PlayerPrefs.SetInt("HighScore", highScore);
    }

}
