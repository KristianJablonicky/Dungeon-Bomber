using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingLight : MonoBehaviour
{
    [SerializeField] private Image image;

    void Start()
    {
        var metronome = Metronome.instance;
        metronome.onPlayerInputStart += onWindowStart;
        metronome.userInputEnd += onWindowEnd;
    }

    private void onWindowStart(object sender, System.EventArgs e)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.025f);
    }
    private void onWindowEnd(object sender, System.EventArgs e)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
    }
}
