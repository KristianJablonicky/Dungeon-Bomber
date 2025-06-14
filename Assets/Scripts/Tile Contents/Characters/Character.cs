using System;
using System.Collections;
using UnityEngine;

public abstract class Character : TileContent
{
    protected int hp, maxHp;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] protected Animator animator;

    protected spiritType weakness;

    public event EventHandler<DamageArgs> hpChanged;
    public event EventHandler defeated;

    private Coroutine runningHurtAnimation, runningSlideAnimation;

    private HitSplat hitSplatInstance = null;

    protected SoundPlayer soundPlayer;

    protected bool turnedRight = true;

    protected override void Start()
    {
        base.Start();
        hp = getBaseMaxHp();
        maxHp = hp;
        weakness = setWeakness();
        if (this is not Player && this is not Boss)
        {
            var healthBar = Instantiate(Prefabs.i.healthBar);
            healthBar.setCharacter(this);
        }

        soundPlayer = SoundPlayer.getInstance();
    }

    public int getMaxHp()
    {
        return maxHp;
    }

    public void increaseMaxHp(int increase, bool restoreHealth = false)
    {
        if (this is not Boss)
        {
            maxHp += increase;
        }
        if (restoreHealth)
        {
            heal(increase, damageTags.MaxHpIncreaseHeal);
        }
    }

    public abstract int getBaseMaxHp();
    public int getHp()
    {
        return hp;
    }

    public virtual void takeDamage(int damage, damageTags tag = damageTags.Damage, spiritType? type = null)
    {
        if (type == weakness)
        {
            tag = damageTags.CriticalDamage;
            damage *= 2;
            damage = Math.Max(damage, getMaxHp() / 2);
        }
        else if (weakness != spiritType.none)
        {
            damage = (int)Mathf.Ceil(damage / 2f);
        }

        damage = Math.Min(damage, hp);
        hp -= damage;
        hpChanged?.Invoke(this, new DamageArgs(damage, tag));
        if (hp <= 0)
        {
            die();
        }
        else if (runningHurtAnimation == null)
        {
            runningHurtAnimation = StartCoroutine(hurtAnimation());
        }
    }

    protected abstract spiritType setWeakness();
    public spiritType getWeakness()
    {
        return weakness;
    }
    public void heal(int healAmount, damageTags tag = damageTags.Heal)
    {
        healAmount = Math.Min(healAmount, maxHp - hp);
        hp += healAmount;
        hpChanged?.Invoke(this, new DamageArgs(healAmount, tag));
    }
    private IEnumerator hurtAnimation()
    {
        float speed = 4f;
        while (spriteRenderer.color.g > 0f)
        {
            float diff = speed * Time.deltaTime;
            spriteRenderer.color = new Color(1f, spriteRenderer.color.g - diff, spriteRenderer.color.b - diff);
            yield return null;
        }
        while (spriteRenderer.color.g < 1f)
        {
            float diff = speed * Time.deltaTime;
            spriteRenderer.color = new Color(1f, spriteRenderer.color.g + diff, spriteRenderer.color.b + diff);
            yield return null;
        }
        spriteRenderer.color = Color.white;

        runningHurtAnimation = null;
    }

    public virtual void die()
    {
        if (this != null)
        {
            dungeon.removeFromTile(this);
            defeated?.Invoke(this, EventArgs.Empty);
            if (animator != null)
            {
                animator.SetTrigger("die");
            }
            else if (this is not Player && this is not Boss)
            {
                StartCoroutine(deathAnimation());
            }
        }
    }

    private IEnumerator deathAnimation(float duration = 0.25f)
    {
        float speed = 1f / duration;

        while (Mathf.Abs(transform.localScale.x) < 1.2f)
        {
            if (transform.localScale.x < 0f)
            {
                transform.localScale = new Vector3(-1f * transform.localScale.x, transform.localScale.y);
                transform.localScale += Vector3.one * (Time.deltaTime * speed);
                transform.localScale = new Vector3(-1f * transform.localScale.x, transform.localScale.y);
            }
            else
            {
                transform.localScale += Vector3.one * (Time.deltaTime * speed);
            }
            yield return null;
        }
        float timeElapsed = 0f;
        Vector3 startingSize = transform.localScale;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transform.localScale = startingSize * (1f - timeElapsed / duration);
            yield return null;
        }
        destroyGameObjectForGood();
    }

    public void destroyGameObjectForGood()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    // move the character if successful, if not, return object that stood in the way
    public TileContent move(Movement movement, int moveTiles = 1)
    {
        Vector3 direction = Directions.vectors[(int)movement];
        TileContent targetTile = dungeon.isTileOccupied(new Vector3(x, y) + direction);

        for (int moveNumber = 0; moveNumber < moveTiles; moveNumber++)
        {
            targetTile = dungeon.isTileOccupied(new Vector3(x, y) + direction);

            // collision
            if (targetTile != null)
            {
                if ((targetTile.bounceOnCollision() || this is not Player))
                {
                    if (moveNumber == 0)
                    {
                        int destX = x + (int)direction.x, destY = y + (int)direction.y;
                        StartCoroutine(bounce(destX, destY));
                    }
                    else
                    {
                        moveAnimation();
                    }
                
                    if (targetTile is Character)
                    {
                        // Collide with the player (Enemy on emeny violence is not tolerated)
                        if (!(this is Enemy && targetTile is Enemy))
                        {
                            collideWithCharacter((Character)targetTile);
                        }
                    }
                    return targetTile;
                }
                else if (targetTile is Ladder && this is Player)
                {
                    return targetTile;
                }
            }
            else
            {
                dungeon.removeFromTile(x, y);
                x += (int)direction.x;
                y += (int)direction.y;
                dungeon.moveToTile(this, x, y);
            }
        }
        moveAnimation();


        if (movement == Movement.Right)
        {
            transform.localScale = Vector3.one;
            turnedRight = true;
        }
        if (movement == Movement.Left)
        {
            transform.localScale = new Vector3(-1, 1);
            turnedRight = false;
        }
        return targetTile;
    }

    public abstract void collideWithCharacter(Character character);

    private float animationDuration = 0.2f;

    private void moveAnimation()
    {
        if (runningSlideAnimation != null)
        {
            StopCoroutine(runningSlideAnimation);
            runningSlideAnimation = null;
            transform.position = new Vector3(x, y);
        }
        runningSlideAnimation = StartCoroutine(slide());
    }

    protected IEnumerator slide()
    {
        Vector3 start = transform.position;
        float xDir = x - (int)transform.position.x, yDir = y - (int)transform.position.y;

        float duration = animationDuration * metronome.getBeatLength(), elapsedTime = 0f;


        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = new Vector3(start.x + Mathf.Sqrt(elapsedTime / duration) * xDir, start.y + Mathf.Sqrt(elapsedTime / duration) * yDir);
            movementMethod();
            yield return null;
        }
        transform.position = new Vector3(x, y);
        runningSlideAnimation = null;
    }

    protected virtual void movementMethod() { }

    protected IEnumerator bounce(int x, int y)
    {
        Vector3 start = transform.position;
        float xDir = x - (int)transform.position.x, yDir = y - (int)transform.position.y;

        float duration = animationDuration * metronome.getBeatLength(), elapsedTime = 0f;


        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = new Vector3(start.x + 0.25f * Mathf.Pow(Mathf.Sin(Mathf.PI * elapsedTime / duration), 2) * xDir, start.y + 0.25f * Mathf.Pow(Mathf.Sin(Mathf.PI * elapsedTime / duration), 2) * yDir);
            yield return null;
        }
        transform.position = new Vector3(this.x, this.y);
    }

    public void setHitSplatInstance(HitSplat hitSplat)
    {
        hitSplatInstance = hitSplat;
    }

    public HitSplat getHitSplatInstance()
    {
        return hitSplatInstance;
    }
}
public enum spiritType
{
    bear,
    wolf,
    owl,
    none
}