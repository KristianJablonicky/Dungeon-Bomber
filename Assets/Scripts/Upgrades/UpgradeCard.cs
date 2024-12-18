using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image icon, colorfulHighlights, holdHighlight;
    [SerializeField] private TMP_Text text;

    private Upgrade upgrade;
    private UpgradeCardsGenerator generator;
    private upgradeTypes type;

    private float heldDuration = 0f;
    private bool heldDown = false;

    public void setUpgrade(Upgrade upgrade, UpgradeCardsGenerator generator)
    {
        if (upgrade.getType() == upgradeTypes.Random)
        {
            type = upgrade.setUpSpecificSpiritTypeUtilities();
        }
        else
        {
            type = upgrade.getType();
        }
        this.upgrade = upgrade;
        this.generator = generator;
        icon.sprite = upgrade.getIcon();

        setColor();
        holdHighlight.color = Dungeon.instance.getFloorColor();

        text.text = upgrade.getDescription();
    }

    private void setColor()
    {
        if (type == upgradeTypes.Bear)
        {
            colorfulHighlights.color = Color.red;
        }
        else if (type == upgradeTypes.Wolf)
        {
            colorfulHighlights.color = Color.green;
        }
        else if (type == upgradeTypes.Owl)
        {
            colorfulHighlights.color = Color.blue;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(updateHeldDownTime());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        heldDown = false;
        if (heldDuration >= 1f)
        {
            upgrade.equip();
            upgrade.equipEffect(Dungeon.instance.getPlayer());
            generator.cardChosen();
        }
        else
        {
            heldDuration = 0f;
            holdHighlight.fillAmount = 0f;
        }
    }

    private IEnumerator updateHeldDownTime()
    {
        heldDown = true;
        while (heldDown)
        {
            heldDuration += Time.deltaTime * 1.5f;
            holdHighlight.fillAmount = Mathf.Floor(8f * heldDuration) / 8f;
            yield return null;
        }
    }

}
