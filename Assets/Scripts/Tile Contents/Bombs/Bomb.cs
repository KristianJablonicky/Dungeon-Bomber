using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Bomb : TileContent
{
    [SerializeField] private Explosion explosion;
    private List<Explosion> explosionList;
    private int currentBeatsUntilImpact, beatsUntilImpact, damage;
    private bombTypes type;
    public void setUp (int beatsUntilImpact, int damage, int horizontalLength, int verticalLength, int diagonalLength, bombTypes type)
    {
        Start();
        Metronome.instance.onBombUpdate += reduceBeatsUntilImpact;
        this.type = type;
        this.beatsUntilImpact = beatsUntilImpact;
        currentBeatsUntilImpact = beatsUntilImpact;
        this.damage = damage;
        explosionList = new List<Explosion>();
        
        
        addExplosion(1, 0, 0);
        addHorizontalExplosions(horizontalLength);
        addVerticalExplosions(verticalLength);
        addDiagonalExplosions(diagonalLength);
        
        updateAlphas();
    }

    private void addExplosion(int distance, int xOffset, int yOffset)
    {
        var offset = new Vector3(xOffset, yOffset);
        var exp = Instantiate(explosion, transform.position + offset, Quaternion.identity);
        exp.setUp(CharacterType.NPC, damage, distance, type);
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
        /*
        for(int xOffset = -1 * length; xOffset <= length; xOffset++)
        {
            if (Mathf.Abs(xOffset) > areaSize)
            {
                addExplosion(beatsUntilImpact, damage, xOffset, 0);
            }
        }
        */
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
        if (content == null || content is Character)
        {
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
    private void updateAlphas()
    {
        foreach(var exp in explosionList)
        {
            exp.updateAlpha((float)((beatsUntilImpact + 1f) - currentBeatsUntilImpact) / beatsUntilImpact);
        }
    }
    private void explode()
    {
        Metronome.instance.onBombUpdate -= reduceBeatsUntilImpact;
        foreach (var exp in explosionList)
        {
            exp.explode();
        }
        StartCoroutine(waitForExplosionFlash());
    }
    private IEnumerator waitForExplosionFlash()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
