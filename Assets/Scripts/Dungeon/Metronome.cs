using System;
using System.Collections;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    public static Metronome instance;

    [SerializeField] private int bpm = 60;
    private float beatLength = 1f;
    private readonly float inputWindowLength = 0.5f;

    private int BPM { get => bpm;
        set {
            bpm = value;
            beatLength = 60f / bpm;
            playerInputWindowStart = beatLength - beatLength * inputWindowLength;
            midBeat = 0.5f * beatLength;
        }
    }

    private float playerInputWindowStart = 0.9f, midBeat = 0.5f;


    public event EventHandler countInBeat, onPlayerInputStart, onBeat, onBeatEnemy, onBeatLowerPriority, userInputEnd, onUpdate, onSpiritUpdate;


    private float currentBeatProgress = 0f;
    private MetronomeBeatStates beatState = MetronomeBeatStates.beforeUserInput;
    private Action updateCurrentBeatState;

    private bool calledBeatPrematurely = false;

    private void OnValidate()
    {
        BPM = bpm;
    }

    private void Awake()
    {
        instance = this;
        updateCurrentBeatState = updateBeatBeforeUserInput;
    }

    private void Start()
    {
        Dungeon.instance.ladderReached += pause;
        Dungeon.instance.getPlayer().defeated += pause;
        var boss = Dungeon.instance.getBoss();
        if (boss != null)
        {
            boss.defeated += pause;
        }

        StartCoroutine(countIn());
        enabled = false;
    }

    private void pause(object sender, EventArgs e)
    {
        StopAllCoroutines();
        enabled = false;
    }

    private IEnumerator countIn()
    {
        yield return null;
        BPM = 115 + 5 * (DataStorage.instance.floor - 1);

        float timeElapsed = 0f, measureLength = beatLength * 4f;
        countInBeat?.Invoke(this, EventArgs.Empty);
        for (int beat = 0; beat < 3; beat++)
        {
            while (timeElapsed < beatLength)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            timeElapsed -= beatLength;
            countInBeat?.Invoke(this, EventArgs.Empty);
        }
        enabled = true;
    }
    private void Update()
    {
        currentBeatProgress += Time.deltaTime;
        //Debug.Log($"{currentBeatProgress} -> {getBeatProgress()}");
        updateCurrentBeatState();
    }

    private void updateBeatBeforeEndOfUserInput()
    {
        if (getBeatProgress() >= inputWindowLength)
        {
            updateCurrentBeatState = updateBeatBeforeSpiritExplosion;
            userInputEnd?.Invoke(this, EventArgs.Empty);
            onUpdate?.Invoke(this, EventArgs.Empty);
        }
    }

    private void updateBeatBeforeSpiritExplosion()
    {
        if (currentBeatProgress >= midBeat)
        {
            updateCurrentBeatState = updateBeatBeforeUserInput;
            onSpiritUpdate?.Invoke(this, EventArgs.Empty);
        }
    }

    private void updateBeatBeforeUserInput()
    {
        if (currentBeatProgress >= playerInputWindowStart)
        {
            onPlayerInputStart?.Invoke(this, EventArgs.Empty);
            updateCurrentBeatState = updateBeatAfterUserInputBeforeBeat;
        }
    }
    private void updateBeatAfterUserInputBeforeBeat()
    {
        if (currentBeatProgress >= beatLength)
        {
            updateCurrentBeatState = updateBeatBeforeEndOfUserInput;
            currentBeatProgress -= beatLength;
            
            onBeat?.Invoke(this, EventArgs.Empty);
            
            if (!calledBeatPrematurely)
            {
                onBeatMethod();
            }
            calledBeatPrematurely = false;
        }
    }
    private void onBeatMethod()
    {
        onBeatEnemy?.Invoke(this, EventArgs.Empty);
        onBeatLowerPriority?.Invoke(this, EventArgs.Empty);
    }
    public float getBeatProgress()
    {
        return currentBeatProgress / beatLength;
    }

    public float getBeatLength()
    {
        return beatLength;
    }

    public bool isInPlayerWindowInputStart()
    {
        return currentBeatProgress / beatLength > 0.5f;
    }

    public void prematureBeat()
    {
        calledBeatPrematurely = true;
        onBeatMethod();
    }

}
