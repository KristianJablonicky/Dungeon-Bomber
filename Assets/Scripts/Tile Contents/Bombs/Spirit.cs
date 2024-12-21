using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Spirit : TileContent
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite spiritBear, spiritWolf, spiritOwl;

    [SerializeField] private Explosion explosion;
    private List<Explosion> explosionList;
    private int currentBeatsUntilImpact, beatsUntilImpact, damage, range;
    private spiritType type;
    private damageTags damageTag;

    private bool layedDamageUnderBoss = false;

    public void setUp (SpiritAttributes attributes, int length, spiritType type)
    {
        Start();
        Metronome.instance.onSpiritUpdate += reduceBeatsUntilImpact;
        this.type = type;
        this.beatsUntilImpact = attributes.ticksUntilExplosion;
        currentBeatsUntilImpact = beatsUntilImpact;
        this.damage = attributes.damage;
        this.range = length;

        if (Random.value <= attributes.criticalHitChance)
        {
            damage = (int)(attributes.damage * attributes.criticalHitMultiplier);
            damageTag = damageTags.CriticalDamage;
        }
        else
        {
            damageTag = damageTags.Damage;
        }

        explosionList = new List<Explosion>();

        setSpiritType();

        addExplosion(1, 0, 0);
        addHorizontalExplosions(attributes.horizontalLength);
        addVerticalExplosions(attributes.verticalLength);
        addDiagonalExplosions(attributes.diagonalLength);

        StartCoroutine(fadeIn());

        updateAlphas();
    }

    private void setSpiritType()
    {
        if (type == spiritType.wolf)
        {
            spriteRenderer.sprite = spiritWolf;
        }
        else if (type == spiritType.owl)
        {
            spriteRenderer.sprite = spiritOwl;
        }
    }

    private void addExplosion(int distance, int xOffset, int yOffset)
    {
        var offset = new Vector3(xOffset, yOffset);
        var exp = Instantiate(explosion, transform.position + offset, Quaternion.identity);
        exp.setUp(CharacterType.NPC, damage, distance, range, type, damageTag);
        explosionList.Add(exp);
        exp.transform.SetParent(transform);
    }

    private void addHorizontalExplosions(int length)
    {
        Vector3 target;
        for (int direction = -1; direction <= 1; direction += 2)
        {
            for (int distance = 1; distance <= length; distance++)
            {
                target = new Vector3(transform.position.x + direction * distance, transform.position.y);
                if (addExplosionToTile(target, distance))
                {
                    break;
                }
            }
        }
    }

    private void addVerticalExplosions(int length)
    {
        Vector3 target;
        for (int direction = -1; direction <= 1; direction += 2)
        {
            for (int distance = 1; distance <= length; distance++)
            {
                target = new Vector3(transform.position.x, transform.position.y + direction * distance);
                if (addExplosionToTile(target, distance))
                {
                    break;
                }
            }
        }
        /*
        for (int yOffset = -1 * verticalLength; yOffset <= verticalLength; yOffset++)
        {
            if (Mathf.Abs(yOffset) > areaSize)
            {
                addExplosion(beatsUntilImpact, damage, 0, yOffset);
            }
        }
        */
    }
    private void addDiagonalExplosions(int length)
    {
        Vector3 target;
        for (int directionX = -1; directionX <= 1; directionX += 2)
        {
            for(int directionY = -1; directionY <= 1; directionY += 2)
            {
                for (int distance = 1; distance <= length; distance++)
                {
                    target = new Vector3(transform.position.x + directionX * distance, transform.position.y + directionY * distance);
                    if (addExplosionToTile(target, distance))
                    {
                        break;
                    }
                }
            }
        }
        /*
        for (int xyOffset = -1 * diagonalLength; xyOffset <= diagonalLength; xyOffset++)
        {
            if (Mathf.Abs(xyOffset) > areaSize)
            {
                addExplosion(beatsUntilImpact, damage, xyOffset, -xyOffset);
                addExplosion(beatsUntilImpact, damage, xyOffset, xyOffset);
            }
        }
        */
    }

    private bool addExplosionToTile(Vector3 targetTile, int distance)
    {
        var content = dungeon.isTileOccupied(targetTile);
        int x = (int)(targetTile.x - transform.position.x), y = (int)(targetTile.y - transform.position.y);
        if (content == null || (content is Character && content is not Boss))
        {
            addExplosion(distance, x, y);
            return false;
        }
        else if (!layedDamageUnderBoss && (content is BossHitbox || content is Boss))
        {
            layedDamageUnderBoss = true;
            addExplosion(distance, x, y);
            return false;
        }
        else if (content is Destructable)
        {
            addExplosion(distance, x, y);
            return true;
        }
        else
        {
            return true;
        }
    }


    private void reduceBeatsUntilImpact(object sender, System.EventArgs e)
    {
        currentBeatsUntilImpact--;
        if (currentBeatsUntilImpact == 0)
        {
            explode();
        }
        else
        {
            updateAlphas();
        }
    }

    private IEnumerator fadeIn()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        float targetAlpha = 0.6f;
        while (spriteRenderer.color.a < targetAlpha)
        {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a + Time.deltaTime * 6f);
            transform.localScale = Vector3.one * spriteRenderer.color.a / targetAlpha;
            yield return null;
        }
        spriteRenderer.color = new Color(1, 1, 1, targetAlpha);
        transform.localScale = Vector3.one;
    }

    private void updateAlphas()
    {
        foreach(var exp in explosionList)
        {
            exp.updateAlpha((float)((beatsUntilImpact + 1f) - currentBeatsUntilImpact) / beatsUntilImpact);
        }
    }
    private void explode()
    {
        Metronome.instance.onSpiritUpdate -= reduceBeatsUntilImpact;
        StartCoroutine(dealDamage());
    }

    private IEnumerator dealDamage()
    {
        var metrnonome = Metronome.instance;
        var explosionSpritesList = ExplosionFrames.instance.getExplosionList(type);
        float tickLength = (metrnonome.getBeatLength() * (1 - metrnonome.getBeatProgress())) / (explosionSpritesList.Count + 1);

        foreach (var exp in explosionList)
        {
            exp.dealDamge();
        }

        for (int frame = 0; frame < explosionSpritesList.Count; frame++)
        {
            foreach (var exp in explosionList)
            {
                exp.setFrame(explosionSpritesList[frame]);
            }
            yield return new WaitForSeconds(2f * tickLength);
        }

        foreach (var exp in explosionList)
        {
            exp.fade(tickLength);
        }

        StartCoroutine(waitForExplosionFlash(tickLength));
    }

    private IEnumerator waitForExplosionFlash(float duration)
    {
        float timeElapsed = 0f, startingAlpha = spriteRenderer.color.a;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            //transform.localScale = Vector3.one * (1f + timeElapsed);
            spriteRenderer.color = new Color(1f, 1f, 1f, (1f - timeElapsed / duration) * startingAlpha);
            yield return null;
        }
        Destroy(gameObject);
    }
}
