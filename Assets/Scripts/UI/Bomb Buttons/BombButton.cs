using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BombButton : MonoBehaviour
{
    [SerializeField] private TMP_Text hotKey;
    [SerializeField] private Image bombImage, highlight;

    private Player player;
    private bombTypes type;
    public void setUp(bombTypes type)
    {
        this.type = type;
        highlight.enabled = false;
        if (type == bombTypes.square)
        {
            setSpecificBomb(Color.red, "Z");
            highlight.enabled = true;
        }
        else if (type == bombTypes.plus)
        {
            setSpecificBomb(Color.green, "X");
        }
        else
        {
            setSpecificBomb(Color.blue, "C");
        }
    }

    private void setSpecificBomb(Color color, string hotKey)
    {
        bombImage.color = color;
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
        player.bombChanged += highlightBomb;
    }

    private void highlightBomb(object sender, EventArgs e)
    {
        if (type == player.currentBombType)
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
        player.changeBomb(type);
    }
}
