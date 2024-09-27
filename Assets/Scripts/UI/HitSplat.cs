using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class HitSplat : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    float timeAlive = 0f, duration = 0.5f;
    Vector3 maxY;

    public void setUp(int damageTaken) 
    {
        text.text = damageTaken.ToString();
        StartCoroutine(Rescale());
        maxY = new Vector3(0, 0.5f);
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > duration)
        {
            Destroy(gameObject);
        }
        transform.position += maxY * Time.deltaTime / duration;

    }

    private IEnumerator Rescale()
    {
        float popUpDuration = 0.25f * duration;
        while (timeAlive < popUpDuration)
        {
            transform.localScale = Vector3.one * timeAlive / popUpDuration;
            yield return null;
        }
        transform.localScale = Vector3.one;

        yield return new WaitForSeconds(0.5f * duration);

        float shrinkDuration = 0.25f * duration, shrinkingTime = 0f;
        while (shrinkingTime < shrinkDuration)
        {
            shrinkingTime += Time.deltaTime;
            transform.localScale = Vector3.one - Vector3.one * (Mathf.Pow(shrinkingTime / shrinkDuration, 2));
            yield return null;
        }
    }
}

public class DamageArgs : EventArgs
{
    public int damage;
    public DamageArgs(int damage)
    {
        this.damage = damage;
    }
}
