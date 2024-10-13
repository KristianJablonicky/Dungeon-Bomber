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
        var player = Dungeon.instance.getPlayer();
        DataStorage.instance.addUpgrade(this);
        oneTimeEffect(player);
    }

    public abstract void equipEffect(Player player);

    // add code to this method if you wish to obain a certain buff immediately (such as heals)
    protected virtual void oneTimeEffect(Player player) { }

    public abstract upgradeTypes getType();

    protected BombAttributes getBomb(Player player)
    {
        return getBomb(player, getType());
    }
    protected BombAttributes getBomb(Player player, upgradeTypes type)
    {
        if (type == upgradeTypes.Sacrifice || type == upgradeTypes.Neutral)
        {
            return null;
        }
        else if (type == upgradeTypes.Red)
        {
            return player.bombSquare;
        }
        else if (type == upgradeTypes.Blue)
        {
            return player.bombX;
        }
        else
        {
            return player.bombPlus;
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
    Red,
    Green,
    Blue,
    Random,
    Neutral,
    Sacrifice
}