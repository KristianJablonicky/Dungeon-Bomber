using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : TileContent
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private int damage, currentDistance, range;
    private CharacterType target;
    private spiritType type;
    private damageTags damageTag;

    public void setUp(CharacterType target, int baseDamage, int currentDistance, int range, spiritType type, damageTags damageTag)
    {
        this.target = target;
        this.damage = baseDamage * (1 + range - currentDistance);
        this.currentDistance = currentDistance;
        this.type = type;
        this.range = range;
        this.damageTag = damageTag;
        setColor(type);
    }

    private void setColor(spiritType type)
    {
        if (type == spiritType.bear)
        {
            spriteRenderer.color = Color.red;
        }
        else if (type == spiritType.wolf)
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            spriteRenderer.color = Color.blue;
        }
    }

    public void updateAlpha(float newAlpha)
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b,
                newAlpha);
    }

    public void setFrame(Sprite frame)
    {
        spriteRenderer.sprite = frame;
    }
    public void dealDamge()
    {
        var hitObject = Dungeon.instance.isTileOccupied(transform.position);
        if ((target == CharacterType.Player && hitObject is Player)
            || (target == CharacterType.NPC && (hitObject is Enemy)))
        {
            ((Character)hitObject).takeDamage(damage, damageTag, type);
        }
        else if (hitObject is Destructable)
        {
            ((Destructable)hitObject).destroy();
        }
        else if (hitObject is BossHitbox)
        {
            ((BossHitbox)hitObject).takeDamage(damage, damageTag, type);
        }

        spriteRenderer.color = new Color(1f, 1f, 1f, (range - (currentDistance - 1)) / (float)range);
    }

    public void fade(float duration)
    {
        StartCoroutine(flash(duration));
    }

    private IEnumerator flash(float duration)
    {
        float timeElapsed = 0f;
        while(timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            var color = spriteRenderer.color;
            color.a = Mathf.Pow(1f - timeElapsed / duration, 4f);
            spriteRenderer.color = color;

            yield return null;
        }
        Destroy(gameObject);
    }
}
