using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneForgeShamanLevelUpGraphic : MonoBehaviour
{
    [SerializeField] private Sprite shamanDefault;
    [SerializeField] private List<Sprite> levelUpFrames;
    [SerializeField] private Image shamanImage;
    [SerializeField] private CanvasGroup fade;

    private static RuneForgeShamanLevelUpGraphic instance;
    private void Awake()
    {
        instance = this;
    }

    public static void playAnimation(float duration)
    {
        instance.StartCoroutine(instance.levelUp(duration * 0.5f));
        instance.StartCoroutine(instance.flashBackground(duration * 0.5f));
    }

    public static void stopCoroutine()
    {
        instance.StopAllCoroutines();
    }

    private IEnumerator levelUp(float duration)
    {
        int framesCount = levelUpFrames.Count;
        foreach (var frame in levelUpFrames)
        {
            shamanImage.sprite = frame;
            yield return new WaitForSecondsRealtime(duration / framesCount);
        }
        shamanImage.sprite = shamanDefault;
    }

    private IEnumerator flashBackground(float duration)
    {
        float timeElapsed = 0f, progress;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.unscaledDeltaTime;
            progress = timeElapsed / duration;
            if (progress < 0.5f)
            {
                fade.alpha = progress;
            }
            else
            {
                fade.alpha = 1f - progress;
            }
            yield return null;
        }
        fade.alpha = 0f;
    }

}
