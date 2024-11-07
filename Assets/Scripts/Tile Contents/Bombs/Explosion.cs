using System.Collections;
using UnityEngine;

public class Explosion : TileContent
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private int baseDamage, distance;
    CharacterType target;
    spiritType type;

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

    public void explode()
    {
        var hitObject = Dungeon.instance.isTileOccupied(transform.position);
        if ((target == CharacterType.Player && hitObject is Player)
            || (target == CharacterType.NPC && (hitObject is Enemy)))
        {
            ((Character)hitObject).takeDamage(baseDamage * distance, damageTags.Damage, type);
        }
        else if (hitObject is Destructable)
        {
            ((Destructable)hitObject).destroy();
        }
        else if (hitObject is BossHitbox)
        {
            ((BossHitbox)hitObject).takeDamage(baseDamage * distance, damageTags.Damage, type);
        }
        StartCoroutine(flash());
    }

    private IEnumerator flash()
    {
        spriteRenderer.color = Color.white;
        animator.SetTrigger("explode");
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
