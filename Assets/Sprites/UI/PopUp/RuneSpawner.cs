using System.Collections;
using UnityEngine;

public class RuneSpawner : MonoBehaviour
{
    [SerializeField] private Rune rune;
    [SerializeField] private Transform runeSpanwer;
    [SerializeField] private Sprite[] runeSprites;
    [SerializeField] private bool isLeftSpawner;
    private bool offset = false;
    private int runeIndex;
    private void Awake()
    {
        runeIndex = Random.Range(0, runeSprites.Length);
        StartCoroutine(spawnRunes());
    }

    private IEnumerator spawnRunes()
    {
        if (isLeftSpawner)
        {
            yield return new WaitForSeconds(1f);
        }
        while (true)
        {
            var runeInstance = Instantiate(rune, transform);
            var rt = runeInstance.getRectTransform();
            if (offset)
            {
                var pos = rt.localPosition;
                if (isLeftSpawner)
                {
                    pos.x += 6f;
                }
                else
                {
                    pos.x -= 6f;
                }
                rt.localPosition = pos;
            }
            offset = !offset;

            runeInstance.setUp(runeSprites[runeIndex]);
            runeIndex++;
            runeIndex %= runeSprites.Length;

            yield return new WaitForSeconds(2f);
        }
    }
}
