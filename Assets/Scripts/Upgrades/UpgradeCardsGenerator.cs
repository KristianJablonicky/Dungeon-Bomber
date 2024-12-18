using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeCardsGenerator : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text hintText, selectionsLeft;
    [SerializeField] private UpgradeCard upgradeCard;
    [SerializeField] private List<Upgrade> upgrades, upgradesRare;
    [SerializeField] private Upgrade sacrifice;

    private List<UpgradeCard> upgradeCards;
    private float duration = 0.5f, maxScale, maxAlpha = 0.6f;
    private int pairsToBeGenerated = 3;

    public static EventHandler upgradePicked;
    
    private void Start()
    {
        upgradeCards = new List<UpgradeCard>();
        Dungeon.instance.ladderReached += generateUpgrades;
        maxScale = upgradeCard.transform.localScale.x;
    }

    private void generateUpgrades(object sender, EventArgs e)
    {
        pairsToBeGenerated += Dungeon.instance.getPlayer().getCurrentFloorUpgrades();
        generatePair(true);
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


        foreach (var card in upgradeCards)
        {
            card.transform.localScale = new Vector2(maxScale, maxScale);
        }
        canvasGroup.alpha = maxAlpha;
        StartCoroutine(showText());
    }

    private IEnumerator showText()
    {
        yield return new WaitForSeconds(1f);

        string targetText = "Click and hold to choose one card.";
        foreach (var character in targetText)
        {
            hintText.text += character;

            if (character != ' ')
            {
                yield return new WaitForSeconds(0.02f);
            }

        }
    }

    private void generatePair(bool fadeInAnimation)
    {
        string s = "";
        if (pairsToBeGenerated > 1)
        {
            s = "s";
        }
        selectionsLeft.text = $"You have {pairsToBeGenerated} selection{s} remaining.";
        pairsToBeGenerated--;
        int totalNumberOfCards = 4;
        
        if (fadeInAnimation)
        {
            StartCoroutine(fadeIn());
        }
        
        for (int cardNumber = 0; cardNumber < totalNumberOfCards; cardNumber++)
        {
            var newCard = Instantiate(upgradeCard, transform);
            newCard.transform.SetParent(transform);

            if (cardNumber == totalNumberOfCards - 1)
            {
                newCard.setUpgrade(sacrifice, this);
            }
            else if (cardNumber == totalNumberOfCards - 2)
            {
                var upgrade = Instantiate(upgradesRare[UnityEngine.Random.Range(0, upgradesRare.Count)]);
                newCard.setUpgrade(upgrade, this);
            }
            else
            {
                var upgrade = Instantiate(upgrades[UnityEngine.Random.Range(0, upgrades.Count)]);
                
                newCard.setUpgrade(upgrade, this);
            }
            
            
            upgradeCards.Add(newCard);
            if (fadeInAnimation)
            {
                newCard.transform.localScale = Vector3.zero;
            }
        }
    }

    public void cardChosen()
    {
        if (pairsToBeGenerated == 0)
        {
            StartCoroutine(shrinkCards());
        }
        else
        {
            deleteCards();

            upgradePicked?.Invoke(this, EventArgs.Empty);


            generatePair(false);
        }
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
