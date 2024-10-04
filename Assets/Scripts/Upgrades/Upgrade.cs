using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    [SerializeField] private Sprite icon; 

    //public abstract string getName();
    public abstract string getDescription();
    public Sprite getIcon()
    {
        return icon;
    }

    public void equip()
    {
        if (this is not Sacrifice)
        {
            DataStorage.instance.addUpgrade(this);
        }
        else
        {
            equipEffect(Dungeon.instance.getPlayer());
        }
    }

    public abstract void equipEffect(Player player);

    public abstract upgradeTypes getType();

    protected BombAttributes getBomb(Player player)
    {
        return getBomb(player, getType());
    }
    protected BombAttributes getBomb(Player player, upgradeTypes type)
    {
        if (type == upgradeTypes.sacrifice || type == upgradeTypes.neutral)
        {
            return null;
        }
        else if (type == upgradeTypes.red)
        {
            return player.bombSquare;
        }
        else if (type == upgradeTypes.blue)
        {
            return player.bombPlus;
        }
        else
        {
            return player.bombX;
        }
    }

    protected upgradeTypes specificUpgradeType;
    public upgradeTypes setUpSpecificBombTypeUtilities()
    {
        specificUpgradeType = (upgradeTypes)Random.Range(0, 3);
        return specificUpgradeType;
    }
}

class bombSpecificUtilities
{
    public upgradeTypes type;
}

public enum upgradeTypes
{
    red,
    green,
    blue,
    random,
    neutral,
    sacrifice
}