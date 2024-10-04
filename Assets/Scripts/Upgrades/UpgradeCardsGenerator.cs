using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeCardsGenerator : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private UpgradeCard upgradeCard;
    [SerializeField] private List<Upgrade> upgrades;
    [SerializeField] private Upgrade sacrifice;

    private List<UpgradeCard> upgradeCards;
    private float duration = 0.5f, maxScale, maxAlpha = 0.6f;
    
    private void Start()
    {
        upgradeCards = new List<UpgradeCard>();
        Dungeon.instance.ladderReached += generateUpgrades;
        StartCoroutine(fadeOut());
        maxScale = upgradeCard.transform.localScale.x;
        canvasGroup.alpha = 1f;
    }

    private IEnumerator fadeOut()
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            canvasGroup.alpha = maxAlpha - maxAlpha * timeElapsed / duration;

            yield return null;
        }
    }

    private void generateUpgrades(object sender, System.EventArgs e)
    {
        generatePair();
        StartCoroutine(fadeIn());
    }

    private IEnumerator fadeIn()
    {
        float timeElapsed = 0f;
        Vector3 newScale;
        float currentScale;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            canvasGroup.alpha = maxAlpha * timeElapsed / duration;

            currentScale = maxScale * timeElapsed / duration;
            newScale = new Vector3(currentScale, currentScale);

            foreach (var card in upgradeCards)
            {
                card.transform.localScale = newScale;
            }
            yield return null;
        }
        // hintText.text = "Choose one card by clicking and holding on it.";
        StartCoroutine(showText());
    }

    private IEnumerator showText()
    {
        yield return new WaitForSeconds(1f);

        string targetText = "Choose one card by clicking and holding on it.";
        foreach (var character in targetText)
        {
            hintText.text += character;

            if (character != ' ')
            {
                yield return new WaitForSeconds(0.02f);
            }

        }
    }

    private void generatePair()
    {
        int totalNumberOfCards = 3;
        for (int cardNumber = 0; cardNumber < totalNumberOfCards; cardNumber++)
        {
            var newCard = Instantiate(upgradeCard, transform);
            newCard.transform.SetParent(transform);

            if (cardNumber == totalNumberOfCards - 1)
            {
                newCard.setUpgrade(sacrifice, this);
            }
            else
            {
                var upgrade = Instantiate(upgrades[Random.Range(0, upgrades.Count)]);
                
                newCard.setUpgrade(upgrade, this);
            }
            
            
            upgradeCards.Add(newCard);
            newCard.transform.localScale = Vector3.zero;
        }
    }

    public void cardChosen()
    {
        StartCoroutine(shrinkCards());
    }

    private IEnumerator shrinkCards()
    {
        float timeElapsed = 0f;
        Vector3 newScale;
        float currentScale;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            currentScale = maxScale * (1f - timeElapsed / duration);
            newScale = new Vector3(currentScale, currentScale);

            foreach (var card in upgradeCards)
            {
                card.transform.localScale = newScale;
            }

            yield return null;
        }
        deleteCards();
        Dungeon.instance.nextFloor();
    }
    private void deleteCards()
    {
        for (int i = upgradeCards.Count - 1; i >= 0; i--)
        {
            Destroy(upgradeCards[i].gameObject);
            upgradeCards.RemoveAt(i);
        }
    }
}
