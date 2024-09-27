using System;
using System.Collections;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    public static Metronome instance;

    [SerializeField] private int bpm = 60;
    private float beatLength = 1f;
    private readonly float inputWindowLength = 0.2f;

    private int BPM { get => bpm;
        set {
            bpm = value;
            beatLength = 60f / bpm;
            playerInputWindowStart = beatLength - inputWindowLength;
            midBeat = 0.5f * beatLength;
        }
    }

    private float playerInputWindowStart = 0.9f, midBeat = 0.5f;


    public event EventHandler countInBeat, userInputStart, onBeat, onBeatLowerPriority, userInputEnd, onUpdate, onBombUpdate;


    private float currentBeatProgress = 0f;
    private MetronomeBeatStates beatState = MetronomeBeatStates.beforeUserInput;
    private Action updateCurrentBeatState;

    private void OnValidate()
    {
        BPM = bpm;
    }

    private void Awake()
    {
        instance = this;
        updateCurrentBeatState = updateBeatBeforeUserInput;
        enabled = false;
        StartCoroutine(countIn());
    }

    private IEnumerator countIn()
    {
        yield return null;
        BPM = 90 + 5 * (DataStorage.instance.floor - 1);
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
        updateCurrentBeatState();
    }

    private void updateBeatBeforeEndOfUserInput()
    {
        if (currentBeatProgress >= inputWindowLength)
        {
            updateCurrentBeatState = updateBeatBeforeBombExplosion;
            userInputEnd?.Invoke(this, EventArgs.Empty);
            onUpdate?.Invoke(this, EventArgs.Empty);
        }
    }

    private void updateBeatBeforeBombExplosion()
    {
        if (currentBeatProgress >= midBeat)
        {
            updateCurrentBeatState = updateBeatBeforeUserInput;
            onBombUpdate?.Invoke(this, EventArgs.Empty);
        }
    }

    private void updateBeatBeforeUserInput()
    {
        if (currentBeatProgress >= playerInputWindowStart)
        {
            userInputStart?.Invoke(this, EventArgs.Empty);
            updateCurrentBeatState = updateBeatAfterUserInputBeforeBeat;
        }
    }
    private void updateBeatAfterUserInputBeforeBeat()
    {
        if (currentBeatProgress >= beatLength)
        {
            updateCurrentBeatState = updateBeatBeforeEndOfUserInput;
            currentBeatProgress -= beatLength;
            onBeatMethod();
        }
    }
    private void onBeatMethod()
    {
        onBeat?.Invoke(this, EventArgs.Empty);
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
        return currentBeatProgress >= playerInputWindowStart;
    }

}
