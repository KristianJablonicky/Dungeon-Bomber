using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpiritButton : MonoBehaviour
{
    [SerializeField] private TMP_Text hotKey, attributes;
    [SerializeField] private Image spiritImage, highlight;
    [SerializeField] private Sprite spiritBear, spiritWolf, spiritOwl;

    private SpiritAttributes spiritAttributes;

    private Player player;
    private spiritType type;
    public void setUp(spiritType type)
    {
        this.type = type;
        highlight.enabled = false;
    }

    private void setUp(Player player)
    {
        if (type == spiritType.bear)
        {
            setSpecificSpirit(spiritBear, "J", player.bear);
            highlight.enabled = true;
        }
        else if (type == spiritType.wolf)
        {
            setSpecificSpirit(spiritWolf, "K", player.wolf);
        }
        else
        {
            setSpecificSpirit(spiritOwl, "L", player.owl);
        }
    }


    private void setSpecificSpirit(Sprite sprite, string hotKey, SpiritAttributes spiritAttributes)
    {
        this.spiritAttributes = spiritAttributes;
        spiritImage.sprite = sprite;
        if (Application.isMobilePlatform)
        {
            this.hotKey.text = "";
        }
        else
        {
            this.hotKey.text = hotKey;
        }

        updateAttributes();
    }

    private void updateAttributes()
    {
        int range = spiritLengthUtility.getLength(spiritAttributes);
        attributes.text = $"{spiritAttributes.damage}<sprite name=\"Damage\">\r\n{range}<sprite name=\"Range\">\r\n{spiritAttributes.ticksUntilExplosion}<sprite name=\"Delay\">";
    }

    private void Start()
    {
        Dungeon.instance.playerSpawned += setPlayer;
        UpgradeCardsGenerator.upgradePicked += updateAttributes;
    }

    private void setPlayer(object sender, EventArgs e)
    {
        player = (Player)sender;
        player.spiritChanged += highlightSpirit;
        setUp(player);
    }

    private void updateAttributes(object sender, EventArgs e)
    {
        updateAttributes();
    }

    private void highlightSpirit(object sender, EventArgs e)
    {
        if (type == player.currentSpiritType)
        {
            highlight.enabled = true;
        }
        else
        {
            highlight.enabled = false;
        }
    }

    public void clickedOn()
    {
        player.changeSummon(type);
    }
}

// TODO: idealne vyhodit do prec a riesit uplne inak ale tak co uz
public static class spiritLengthUtility
{
    public static int getLength(SpiritAttributes spiritAttributes)
    {
        var range = 1000;
        range = isMin(range, spiritAttributes.diagonalLength);
        range = isMin(range, spiritAttributes.horizontalLength);
        range = isMin(range, spiritAttributes.verticalLength);

        return range;
    }
    private static int isMin(int currentMin, int comparedValue)
    {
        if (comparedValue == 0)
        {
            return currentMin;
        }

        return Math.Min(currentMin, comparedValue);
    }
}