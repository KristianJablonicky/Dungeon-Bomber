using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject healthFlash, healthBar;
    private Character character;
    string formerCharacter;
    [SerializeField] private SpriteRenderer healthBarSprite; 

    public void setCharacter(Character character)
    {
        this.character = character;
        //transform.SetParent(character.transform);
        //transform.localPosition = new Vector3(0f, 0f);
        character.hpChanged += onHpChange;
        character.defeated += onDeath;
        formerCharacter = character.ToString();
        setColor(character.getWeakness());
    }

    private void setColor(spiritType type)
    {
        if (type == spiritType.bear)
        {
            healthBarSprite.color = Color.red;
        }
        else if (type == spiritType.wolf)
        {
            healthBarSprite.color = Color.green;
        }
        else if (type == spiritType.owl)
        {
            healthBarSprite.color = Color.blue;
        }
    }

    private void onDeath(object sender, System.EventArgs e)
    {
        character.defeated -= onDeath;
        Destroy(gameObject);
    }

    private void onHpChange(object sender, DamageArgs e)
    {
        int maxHp = Mathf.Max(character.getMaxHp(), character.getHp());
        StopAllCoroutines();
        StartCoroutine(hpTransition(healthFlash.transform.localScale.x, (float)character.getHp() / maxHp));
    }

    private IEnumerator hpTransition(float startingHp, float EndingHp)
    {
        setFill(healthBar, EndingHp);
        setFill(healthFlash, startingHp);
        float timeElapsed = 0f, duration = Mathf.Abs(startingHp - EndingHp);
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            setFill(healthFlash, Mathf.Lerp(startingHp, EndingHp, timeElapsed / duration));
            yield return null;
        }
        setFill(healthFlash, EndingHp);
    }

    private void setFill(GameObject bar, float fillAmount)
    {
        bar.transform.localScale = new Vector3(fillAmount, 1f);
    }

    private void Update()
    {
        transform.position = character.transform.position;
    }
}
