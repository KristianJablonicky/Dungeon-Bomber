using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    [SerializeField] private Sprite icon;
    protected upgradeTypes specificUpgradeType;

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

    protected SpiritAttributes getSpirit(Player player)
    {
        return getSpirit(player, getType());
    }
    protected SpiritAttributes getSpirit(Player player, upgradeTypes type)
    {

        if (type == upgradeTypes.Random)
        {
            type = specificUpgradeType;
        }

        if (type == upgradeTypes.Sacrifice || type == upgradeTypes.Neutral)
        {
            return null;
        }
        else if (type == upgradeTypes.Bear)
        {
            return player.bear;
        }
        else if (type == upgradeTypes.Owl)
        {
            return player.owl;
        }
        else if (type == upgradeTypes.Wolf)
        {
            return player.wolf;
        }
        Debug.Log("Unchecked upgradeType");
        return null;
    }
    public upgradeTypes setUpSpecificSpiritTypeUtilities()
    {
        specificUpgradeType = (upgradeTypes)Random.Range(0, 3);
        return specificUpgradeType;
    }
    protected string getTypeString()
    {
        return $"<sprite name=\"{specificUpgradeType}\">";
    }

}

public enum upgradeTypes
{
    Bear,
    Wolf,
    Owl,
    Random,
    Neutral,
    Sacrifice
}

public static class Icons
{
    public static string damage = "<sprite name=\"Damage\">",
        range = "<sprite name=\"Range\">",
        heal = "<sprite name=\"Heal\">",
        vitality = "<sprite name=\"Vitality\">",
        dash = "<sprite name=\"Dash\">",
        delay = "<sprite name=\"Delay\">";
}