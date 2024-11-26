using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : TileContent
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> tendrilFrames;
    //[SerializeField] private Animator animator;
    private int baseDamage, distance;
    CharacterType target;
    spiritType type;
    private int currentTendrilFrame = 0;

    public void setUp(CharacterType target, int baseDamage, int distance, spiritType type)
    {
        this.target = target;
        this.baseDamage = baseDamage;
        this.distance = distance;
        this.type = type;

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

    public void dealDamage(int distance)
    {
        if (this.distance > distance)
        {
            return;
        }

        var hitObject = Dungeon.instance.isTileOccupied(transform.position);
        if ((target == CharacterType.Player && hitObject is Player)
            || (target == CharacterType.NPC && (hitObject is Enemy)))
        {
            ((Character)hitObject).takeDamage(baseDamage, damageTags.Damage, type);
        }
        else if (hitObject is Destructable)
        {
            ((Destructable)hitObject).destroy();
        }
        else if (hitObject is BossHitbox)
        {
            ((BossHitbox)hitObject).takeDamage(baseDamage, damageTags.Damage, type);
        }

        spriteRenderer.sprite = tendrilFrames[currentTendrilFrame];


        currentTendrilFrame++;

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
