using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : TileContent
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private int damage;
    CharacterType target;
    public void setUp(CharacterType target, int damage)
    {
        this.target = target;
        this.damage = damage;

        spriteRenderer.color = Color.red;
    }

    public void updateAlpha(float newAlpha)
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b,
                newAlpha);
    }

    public void explode()
    {
        var hitObject = Dungeon.instance.isTileOccupied(transform.position);
        if ((target == CharacterType.Player && hitObject is Player)
            || (target == CharacterType.NPC && (hitObject is Enemy)))
        {
            ((Character)hitObject).takeDamage(damage);
        }
        else if (hitObject is Destructable)
        {
            ((Destructable)hitObject).destroy();
        }
        StartCoroutine(flash());
    }

    private IEnumerator flash()
    {
        spriteRenderer.color = Color.white;

        float animationTime = 0.2f, timeElapsed = 0f;
        while (timeElapsed < animationTime)
        {
            timeElapsed += Time.deltaTime;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b,
                1f - (timeElapsed / animationTime));
            yield return null;
        }
        Destroy(gameObject);
    }
}
