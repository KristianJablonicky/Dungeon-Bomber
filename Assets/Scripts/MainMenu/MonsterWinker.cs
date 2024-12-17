using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWinker : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        StartCoroutine(waitForWink());
    }
    private IEnumerator waitForWink()
    {
        yield return new WaitForSecondsRealtime(Random.Range(3, 4));
        StartCoroutine(wink());
    }
    private IEnumerator wink()
    {
        for (int i = 1; i < sprites.Count; i++)
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSecondsRealtime(0.06f);
        }

        for (int i = sprites.Count-1; i >= 0; i--)
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSecondsRealtime(0.06f);
        }

        StartCoroutine(waitForWink());
    }
}
