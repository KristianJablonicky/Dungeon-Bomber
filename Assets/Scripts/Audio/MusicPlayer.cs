using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip metronomeAccented, metronomeUnaccented;
    [SerializeField] private AudioSource audioSource;
    private Metronome metronome;
    private int currentBeat = 0;
    private void Start()
    {
        metronome = Metronome.instance;
        metronome.countInBeat += onBeat;
        metronome.onBeat += onBeat;
    }

    private void onBeat(object sender, System.EventArgs e)
    {
        if (currentBeat == 0)
        {
            audioSource.clip = metronomeAccented;
        }
        else
        {
            audioSource.clip = metronomeUnaccented;
        }
        audioSource.Play();
        currentBeat++;
        currentBeat %= 4;
    }
}
