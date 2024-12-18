using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Enemy : Character
{ 
    protected float damageScaling;
    private int tickNumber = 0, updateEveryNTicks;

    protected override void Start()
    {
        base.Start();
        metronome.onBeatEnemy += onTick;
        int floor = DataStorage.instance.floor;
        increaseMaxHp((int)Math.Round((float)getBaseMaxHp() * (floor * floor - 1) * 1f), true);
        damageScaling = 1f + ((floor - 1) * 0.5f);
        updateEveryNTicks = getUpdateEveryNTicks();
    }

    protected int getScaledDamage(int unscaledDamage)
    {
        return (int)(unscaledDamage * damageScaling);
    }

    protected abstract int getUpdateEveryNTicks();

    protected override spiritType setWeakness()
    {
        spiritType enemyWeakness;
        var roll = UnityEngine.Random.Range(0, 3);
        if (roll == 0)
        {
            enemyWeakness = spiritType.bear;
        }
        else if (roll == 1)
        {
            enemyWeakness = spiritType.wolf;
        }
        else
        {
            enemyWeakness = spiritType.owl;
        }
        return enemyWeakness;
    }

    public override void die()
    {
        onDeath();
        metronome.onBeatEnemy -= onTick;
        if (this is not Boss)
        {
            Currencies.instance.increaseGold(goldRewardOnDeath(), gameObject);
        }
        else
        {
            Currencies.instance.increaseGold(20, gameObject);
        }
        base.die();
    }

    private int goldRewardOnDeath()
    {
        int floor = dungeon.getFloor();
        return UnityEngine.Random.Range(floor, 2 * floor + 1);
    }

    private void onTick(object sender, EventArgs e)
    {
        tickNumber++;
        if (tickNumber == updateEveryNTicks)
        {
            tickNumber = 0;
            onTick();
        }
    }
    protected abstract void onTick();

    protected void onDeath()
    {
        dungeon.onEnemyKilled();
    }

    protected Movement? detectPlayer()
    {
        var player = dungeon.getPlayer();
        int distance = getDistance(player.getX(), player.getY(), getX(), getY());

        if (distance > getPlayerDetectionRadius())
        {
            return null;
        }

        // Player and enemy coordinates
        int px = player.getX();
        int py = player.getY();
        int ex = getX();
        int ey = getY();

        // Direction vector from enemy to player
        int dx = px - ex;
        int dy = py - ey;

        // Possible movement directions towards the player
        List<Movement> preferredMovements = new List<Movement>();

        if (Math.Abs(dx) >= Math.Abs(dy))
        {
            // Prioritize horizontal movement
            if (dx > 0)
                preferredMovements.Add(Movement.Right);
            else if (dx < 0)
                preferredMovements.Add(Movement.Left);

            if (dy > 0)
                preferredMovements.Add(Movement.Up);
            else if (dy < 0)
                preferredMovements.Add(Movement.Down);
        }
        else
        {
            // Prioritize vertical movement
            if (dy > 0)
                preferredMovements.Add(Movement.Up);
            else if (dy < 0)
                preferredMovements.Add(Movement.Down);

            if (dx > 0)
                preferredMovements.Add(Movement.Right);
            else if (dx < 0)
                preferredMovements.Add(Movement.Left);
        }

        // Check for obstacles and handle player tile
        foreach (var move in preferredMovements)
        {
            int newX = ex;
            int newY = ey;

            switch (move)
            {
                case Movement.Up:
                    newY += 1;
                    break;
                case Movement.Down:
                    newY -= 1;
                    break;
                case Movement.Left:
                    newX -= 1;
                    break;
                case Movement.Right:
                    newX += 1;
                    break;
            }

            Vector3 targetPosition = new Vector3(newX, newY);

            if (!Dungeon.instance.isTileOccupied(targetPosition) || (player.getX() == newX && player.getY() == newY))
            {
                // The tile is free or enemy; move the enemy
                return move;
            }
        }
        return null;
    }

    private int getDistance(int a_x, int a_y, int b_x, int b_y)
    {
        return Mathf.FloorToInt(Mathf.Sqrt(Mathf.Pow(a_x - b_x, 2) + Mathf.Pow(a_y - b_y, 2)));
    }

    protected virtual int getPlayerDetectionRadius()
    {
        return 0;
    }
}