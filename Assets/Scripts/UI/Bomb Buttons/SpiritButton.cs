using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpiritButton : MonoBehaviour
{
    [SerializeField] private TMP_Text hotKey;
    [SerializeField] private Image spiritImage, highlight;
    [SerializeField] private Sprite spiritBear, spiritWolf, spiritOwl;

    private Player player;
    private spiritType type;
    public void setUp(spiritType type)
    {
        this.type = type;
        highlight.enabled = false;
        if (type == spiritType.bear)
        {
            setSpecificSpirit(spiritBear, "Z");
            highlight.enabled = true;
        }
        else if (type == spiritType.wolf)
        {
            setSpecificSpirit(spiritWolf, "X");
        }
        else
        {
            setSpecificSpirit(spiritOwl, "C");
        }
    }

    private void setSpecificSpirit(Sprite sprite, string hotKey)
    {
        spiritImage.sprite = sprite;
        if (Application.isMobilePlatform)
        {
            this.hotKey.text = "";
        }
        else
        {
            this.hotKey.text = hotKey;
        }
    }

    private void Start()
    {
        Dungeon.instance.playerSpawned += setPlayer;
    }

    private void setPlayer(object sender, System.EventArgs e)
    {
        player = (Player)sender;
        player.spiritChanged += highlightSpirit;
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
