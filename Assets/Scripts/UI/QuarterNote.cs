using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuarterNote : MonoBehaviour
{
    [SerializeField] private Image image;
    private Metronome metronome;
    
    private void Start()
    {
        metronome = Metronome.instance;
        //metronome.userInputStart += turnRed;
        //metronome.userInputEnd += turnNormal;
    }

    private void turnRed(object sender, System.EventArgs e)
    {
        image.color = Color.red;
    }
    private void turnNormal(object sender, System.EventArgs e)
    {
        image.color = Color.white;
    }
    private void Update()
    {
        transform.localScale = Vector3.one * (1f + Mathf.Pow(Mathf.Cos(metronome.getBeatProgress() * Mathf.PI), 2f) * 0.5f);
    }
}
