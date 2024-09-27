using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : TileContent
{
    [SerializeField] private Explosion explosion;
    private List<Explosion> explosionList;
    private int currentBeatsUntilImpact, beatsUntilImpact, damage;
    public void setUp (int beatsUntilImpact, int damage, int horizontalLength, int verticalLength, int diagonalLength, int areaSize)
    {
        Metronome.instance.onBombUpdate += reduceBeatsUntilImpact;
        this.beatsUntilImpact = beatsUntilImpact;
        currentBeatsUntilImpact = beatsUntilImpact;
        this.damage = damage;

        explosionList = new List<Explosion>();
        
        // an explosion at offset <0; 0> is created in addAreaExplosions, even at area == 0
        //addExplosion(beatsUntilImpact, damage, 0, 0);
        
        addAreaExplosions(areaSize);
        addHorizontalExplosions(horizontalLength, areaSize);
        addVerticalExplosions(verticalLength, areaSize);
        addDiagonalExplosions(diagonalLength, areaSize);
    }

    private void addExplosion(int beatsUntilImpact, int damage, int xOffset, int yOffset)
    {
        var offset = new Vector3(xOffset, yOffset);
        var exp = Instantiate(explosion, transform.position + offset, Quaternion.identity);
        exp.setUp(CharacterType.NPC, damage);
        explosionList.Add(exp);
        exp.transform.SetParent(transform);
    }

    private void addAreaExplosions(int areaSize)
    {
        for (int xOffset = -1 * areaSize; xOffset <= areaSize; xOffset++)
        {
            for (int yOffset = -1 * areaSize; yOffset <= areaSize; yOffset++)
            {
                addExplosion(beatsUntilImpact, damage, xOffset, yOffset);
            }
        }
    }

    private void addHorizontalExplosions(int horizontalLength, int areaSize)
    {
        for(int xOffset = -1 * horizontalLength; xOffset <= horizontalLength; xOffset++)
        {
            if (Mathf.Abs(xOffset) > areaSize)
            {
                addExplosion(beatsUntilImpact, damage, xOffset, 0);
            }
        }
    }

    private void addVerticalExplosions(int verticalLength, int areaSize)
    {
        for (int yOffset = -1 * verticalLength; yOffset <= verticalLength; yOffset++)
        {
            if (Mathf.Abs(yOffset) > areaSize)
            {
                addExplosion(beatsUntilImpact, damage, 0, yOffset);
            }
        }
    }
    private void addDiagonalExplosions(int diagonalLength, int areaSize)
    {
        for (int xyOffset = -1 * diagonalLength; xyOffset <= diagonalLength; xyOffset++)
        {
            if (Mathf.Abs(xyOffset) > areaSize)
            {
                addExplosion(beatsUntilImpact, damage, xyOffset, -xyOffset);
                addExplosion(beatsUntilImpact, damage, xyOffset, xyOffset);
            }
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
