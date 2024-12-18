using System.Collections;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup.alpha = 1f;
        //StartCoroutine(fadeOut());
    }
    private void Start()
    {
        Metronome.instance.countInBeat += delayFadeIn;
        //Metronome.instance.countInBeat += flickerFade;
    }

    private void delayFadeIn(object sender, System.EventArgs e)
    {
        Metronome.instance.countInBeat -= delayFadeIn;
        StartCoroutine(fadeOut());
    }

    private void flickerFade(object sender, System.EventArgs e)
    {
        canvasGroup.alpha -= 0.25f;
    }

    private IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(0.5f);
        float timeElapsed = 0f, duration = 1.5f;
        while(timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Pow(1f - timeElapsed / duration, 2f);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
