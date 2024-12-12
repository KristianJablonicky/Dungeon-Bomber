using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class HitSplat : MonoBehaviour
{
    [SerializeField] private TMP_Text hitSplatText;
    float timeAlive = 0f, duration = 0.5f;
    Vector3 maxY;
    private Character owner;
    private string prefix, suffix;
    private int value;

    public void setUp(DamageArgs damageArgs, Character owner) 
    {
        if (damageArgs.amount == 0)
        {
            Destroy(gameObject);
        }
        this.owner = owner;

        hitSplatText.text = damageArgs.amount.ToString();
        if (damageArgs.tag == damageTags.MaxHpIncreaseHeal)
        {
            prefix = "+";
        }
        if (damageArgs.tag.ToString().Contains("critical"))
        {
            suffix = "!";
        }
        if (damageArgs.tag.ToString().Contains("Heal"))
        {
            hitSplatText.color = Color.green;
        }
        value = damageArgs.amount;
        setText(value);
        StartCoroutine(Rescale());
        maxY = new Vector3(0, 0.5f);
        owner.hpChanged += onOwnerHpChange;
    }


    private void setText(int value)
    {
        hitSplatText.text = $"{prefix}{value}{suffix}";
    }
    private void onOwnerHpChange(object sender, DamageArgs e)
    {
        value += e.amount;
        setText(value);
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > duration)
        {
            owner.setHitSplatInstance(null);
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
    public int amount;
    public damageTags tag;
    public DamageArgs(int damage, damageTags tag)
    {
        this.amount = damage;
        this.tag = tag;
    }
}

public enum damageTags
{
    None,
    Damage,
    CriticalDamage,
    Heal,
    MaxHpIncreaseHeal
}
