using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : TileContent
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> commonSprites;
    [SerializeField] private List<Sprite> rareSprites;

    private void Awake()
    {
        if (Random.Range(0, 4) == 0)
        {
            spriteRenderer.sprite = rareSprites[Random.Range(0, rareSprites.Count)];
        }
        else
        {
            spriteRenderer.sprite = commonSprites[Random.Range(0, commonSprites.Count)];
        }
    }

}
