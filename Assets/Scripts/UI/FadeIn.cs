using System.Collections;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup.alpha = 1f;
        StartCoroutine(fadeOut());
    }
    private IEnumerator fadeOut()
    {
        float timeElapsed = 0f, duration = 1f;
        while(timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Pow(1f - timeElapsed / duration, 2f);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
