using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : TileContent
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite destroyedSprite;
    public void destroy()
    {
        Dungeon.instance.removeFromTile(this);
        spriteRenderer.sprite = destroyedSprite;
        onDestruction();
        Destroy(this);
    }

    protected virtual void onDestruction() { }
}
