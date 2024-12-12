using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Spirit spirit;
    public SpiritAttributes bear, wolf, owl;

    private int level = 1;
    private int expCount = 0;
    private int currentExpThreshhold = 2;

    [SerializeField] private MovementParticles movementParticles;

    private int currentFloorUpgrades = 0;
    public event EventHandler onExpChange, spiritSummoned;

    Dictionary<int, int> expThreshholds = new Dictionary<int, int>
    {
        { 1, 2 },
        { 2, 4 },
        { 3, 6 },
        { 4, 10 },
        { 5, 12 },
        { 6, 16 },
        { 7, 20 },
        { 8, 24 },
        { 9, 30 },
        { 10, 36 }
    };

    private int spiritCoolDown = 3, currentCoolDown = 0;
    public bool movementEnabled = false, summonedSpiritAlready = false;

    public spiritType currentSpiritType = spiritType.bear;

    public EventHandler spiritChanged, positionUpdated;

    public override int getBaseMaxHp()
    {
        return 5;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(fallDown());
        metronome.onPlayerInputStart += enableInput;
        dungeon.enemyKilled += onEnemyKill;

        metronome.onBeat += summonSpirit;

        setUpAttributes();
        if (DataStorage.instance.floor != 1)
        {
            hp = DataStorage.instance.playerHp;
            maxHp = DataStorage.instance.playerMaxHp;
            level = DataStorage.instance.playerLevel;
            expCount = DataStorage.instance.playerExp;
            currentExpThreshhold = expThreshholds[level];
        }
        DataStorage.instance.equipUpgrades(this);
        heal(0, damageTags.None);

        Metronome.instance.onBeat += startGrooving;
    }

    protected override spiritType setWeakness()
    {
        return spiritType.none;
    }

    private void startGrooving(object sender, EventArgs e)
    {
        Metronome.instance.onBeat -= startGrooving;
        playerAnimator.SetTrigger("Groove");
        playerAnimator.speed = 1f / Metronome.instance.getBeatLength();
    }

    private void setUpAttributes()
    {
        bear = new()
        {
            horizontalLength = 1,
            verticalLength = 1,
            diagonalLength = 1
        };

        wolf = new()
        {
            horizontalLength = 2,
            verticalLength = 2
        };

        owl = new()
        {
            diagonalLength = 2
        };
    }

    private void summonSpirit(object sender, System.EventArgs e)
    {
        if (currentCoolDown < 0 && !summonedSpiritAlready)
        {
            summonSpirit();
        }
    }

    private IEnumerator fallDown()
    {
        float duration = metronome.getBeatLength() * 0.5f, timeElapsed = 0;
        Vector3 start = transform.position + new Vector3(0, 1), end = transform.position;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transform.position = start - new Vector3(0, Mathf.Sqrt(timeElapsed / duration));
            
            yield return null;
        }
        transform.position = end;
    }

    public override void die()
    {
        metronome.onPlayerInputStart -= enableInput;
        metronome.onBeat -= summonSpirit;
        dungeon.enemyKilled -= onEnemyKill;
        base.die();

        //DataStorage.instance.updateHighScore();
    }

    private void enableInput(object sender, System.EventArgs e)
    {
        currentCoolDown--;

        summonedSpiritAlready = false;
        movementEnabled = true;
    }
    private void disableInput(object sender, System.EventArgs e)
    {
        movementEnabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            changeSummon(spiritType.bear);
        }
        else if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeSummon(spiritType.wolf);
        }
        else if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            changeSummon(spiritType.owl);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            onEnemyKill(this, EventArgs.Empty);
        }
        if (movementEnabled)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                jump(Movement.Right);
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                jump(Movement.Up);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                jump(Movement.Down);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                jump(Movement.Left);
            }
        }
    }

    public void changeSummon(spiritType type)
    {
        currentSpiritType = type;
        spiritChanged?.Invoke(this, EventArgs.Empty);
    }

    private Movement delayedMovementDirection;
    public void jump(Movement movement)
    {
        if (!movementEnabled)
        {
            return;
        }

        if (metronome.isInPlayerWindowInputStart())
        {
            metronome.prematureBeat();
            /*
            metronome.onBeatLowerPriority += delayMove;
            delayedMovementDirection = movement;
            disableInput(this, System.EventArgs.Empty);
            return;
            */
        }

        createMovementParticles();
        
        var collider = move(movement);
        if (collider is Ladder)
        {
            ((Ladder)collider).onPlayerContact();
        }

        if (currentCoolDown < 0)
        {
            summonSpirit();
            summonedSpiritAlready = true;
        }


        disableInput(this, EventArgs.Empty);
    }

    private void delayMove(object sender, EventArgs e)
    {
        jump(delayedMovementDirection);
        metronome.onBeatLowerPriority -= delayMove;
    }

    protected override void movementMethod()
    {
        positionUpdated?.Invoke(this, EventArgs.Empty);
    }

    public void summonSpirit()
    {
        currentCoolDown = spiritCoolDown;
        var newSpirit = Instantiate(spirit, new Vector3(x, y), Quaternion.identity);
        int[] shape = { 0, 0, 0 };
        shape[(int)currentSpiritType] = 1;

        SpiritAttributes attributes = bear;
        if (currentSpiritType == spiritType.bear)
        {
            attributes = bear;
        }
        else if (currentSpiritType == spiritType.wolf)
        {
            attributes = wolf;
        }
        else if (currentSpiritType == spiritType.owl)
        {
            attributes = owl;
        }

        newSpirit.setUp(attributes.ticksUntilExplosion, attributes.damage, spiritLengthUtility.getLength(attributes),
            attributes.horizontalLength, attributes.verticalLength, attributes.diagonalLength, currentSpiritType);
        spiritSummoned?.Invoke(attributes, EventArgs.Empty);
    }

    public override void collideWithCharacter(Character character)
    {
        if (character is Enemy)
        {
            ((Enemy)character).collideWithCharacter(this);
        }
    }

    private void onEnemyKill(object sender, System.EventArgs e)
    {
        if (level == 10) return;
        expCount++;
        if (reachedNewPlayerLevel())
        {
            level++;
            currentFloorUpgrades++;
            expCount = 0;
            currentExpThreshhold = expThreshholds[level];
            increaseMaxHp(1, true);
            //StartCoroutine(levelUp());
            playerAnimator.SetTrigger("LevelUp");
        }
        onExpChange?.Invoke(this, EventArgs.Empty);
    }

    private void createMovementParticles()
    {
        Instantiate(movementParticles, transform.position, Quaternion.identity);
    }

    private bool reachedNewPlayerLevel()
    {
        /**
         * TODO: adjust leveling logic as needed
         */
        /*
        if (expCount >= 2)
        {
            return true;
        }
        return false;
        */
        return expCount >= currentExpThreshhold;
    }

    public int getExp()
    {
        return expCount;
    }

    public int getPlayerLevel()
    {
        return level;
    }

    public int getCurrentFloorUpgrades()
    {
        return currentFloorUpgrades;
    }

    public int getExpCount()
    {
        return expCount;
    }

    public int getExpThreshhold()
    {
        return currentExpThreshhold;
    }

    private IEnumerator levelUp()
    {
        float duration = 0.5f, timeElapsed = 0;
        Vector3 formerScale = transform.localScale;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transform.localScale = formerScale * (1 + Mathf.Sin(timeElapsed / duration * Mathf.PI) * 0.25f);
            yield return null;
        }
        transform.localScale = formerScale;
    }
}
