using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuIntroFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    private void Awake()
    {
        StartCoroutine(fadeIn());
    }

    private IEnumerator fadeIn()
    {
        float timeElapsed = 0f, duration = 2f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = 1f - Mathf.Pow(timeElapsed / duration, 2f);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
