using System.Collections.Generic;
using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public static DataStorage instance = null;
    public int floor = 0, playerHp;

    private List<Upgrade> upgrades;

    private void Awake()
    {
        // first awakening
        if (instance == null)
        {
            floor = 1;
            instance = this;
            DontDestroyOnLoad(this);
            upgrades = new List<Upgrade>();
        }
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
        playerHp = 3;
    }

}
