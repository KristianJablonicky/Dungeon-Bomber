using System.Collections.Generic;
using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public static DataStorage instance = null;
    public int floor = 0, playerHp, playerMaxHp, playerLevel, playerExp,
        highScore, currentBeats;

    private List<Upgrade> upgrades;

    private void Awake()
    {
        // first awakening
        if (instance == null)
        {
            highScore = PlayerPrefs.GetInt("HighScore");
            floor = 1;
            instance = this;
            DontDestroyOnLoad(this);
            upgrades = new List<Upgrade>();

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
    }

    public void updateHighScore()
    {
        if (currentBeats < highScore || highScore == 0)
        {
            highScore = currentBeats;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

}
