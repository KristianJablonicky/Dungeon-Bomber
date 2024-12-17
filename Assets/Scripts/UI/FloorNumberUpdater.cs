using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private TMP_Text floorText;
    private DataStorage storage;
    private void Start()
    {
        storage = DataStorage.instance;
        setText();
        Metronome.instance.onBeat += onBeat;
    }

    private void onBeat(object sender, System.EventArgs e)
    {
        setText();
    }

    private void setText()
    {
        if (storage.highScore == -1)
        {
            floorText.text = $"Beat: {storage.currentBeats}";
        }
        else
        {
            floorText.text = $"Beat: {storage.currentBeats}\nBest: {storage.highScore}";
        }
    }
}
