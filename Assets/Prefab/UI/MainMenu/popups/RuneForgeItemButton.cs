using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RuneForgeItemButton : MonoBehaviour
{
    [SerializeField] private RuneForgeItemEnum itemType;
    [SerializeField] private TMP_Text description, values, cost;
    [SerializeField] private Image buttonImage;

    private RuneForgeItem item;
    private enum RuneForgeItemEnum
    {
        vitality,
        criticalHitChance
    }

    private void Awake()
    {
        if (itemType == RuneForgeItemEnum.vitality)
        {
            item = new RuneVitality();
        }
        else if (itemType == RuneForgeItemEnum.criticalHitChance)
        {
            item = new RuneCritical();
        }
        description.text = item.getDescription();
        updateTexts();
        Currencies.instance.goldChanged += updateTexts;
    }

    private void updateTexts(object sender, CurrencyArgs e)
    {
        updateTexts();
    }

    private void updateTexts()
    {
        string percentage = item.isInPercent() ? "%" : "";
        if (item.currentBonus == item.getLimit())
        {
            values.text = item.getCurrentValue().ToString();
        }
        else
        {
            values.text = $"{item.getCurrentValue()}{percentage} => <color=\"green\">{item.getUpgradeValue()}{percentage}";
        }
        cost.text = $"{item.getCost()} <size=9><sprite name=\"Gold\">";

        if (!item.canBeBought())
        {
            buttonImage.color = Color.gray;
        }
        if (item.getCost() > Currencies.instance.getGold())
        {
            cost.color = Color.red;
        }
    }

    public void onClick()
    {
        if (item.attemptUpgrade())
        {
            updateTexts();
            float duration = 2f;
            StopAllCoroutines();
            RuneForgeShamanLevelUpGraphic.stopCoroutine();
            RuneForgeShamanLevelUpGraphic.playAnimation(duration);
            StartCoroutine(speedUp(duration));
        }
    }

    private IEnumerator speedUp(float duration)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.unscaledDeltaTime;
            if (timeElapsed < duration * 0.25f)
            {
                Time.timeScale = timeElapsed / duration * 4f * 2f;
            }
            else if (timeElapsed > duration * 0.75f)
            {
                Time.timeScale = 2f - (timeElapsed / duration * 4f - 3f);
            }
            else
            {
                Time.timeScale = 4f;
            }

            yield return null;
        }
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;
    }

    private abstract class RuneForgeItem
    {
        public int currentBonus;
        public RuneForgeItem()
        {
            currentBonus = PlayerPrefs.GetInt(getPrefsString(), 0);
        }

        /// <summary>
        /// Upgrade the item's attribute if possible.
        /// </summary>
        /// <returns>true if the item's attribute was upgraded</returns>
        public bool attemptUpgrade()
        {
            if (!canBeBought())
            {
                return false;
            }

            Currencies.instance.adjustGold(-getCost());
            gainBonus();
            return true;
        }
        public bool canBeBought()
        {
            int limit = getLimit();
            if (limit != -1 &&
                currentBonus == limit)
            {
                return false;
            }

            if (Currencies.instance.getGold() < getCost())
            {
                return false;
            }
            return true;
        }
        public void gainBonus()
        {
            currentBonus++;
            PlayerPrefs.SetInt(getPrefsString(), currentBonus);
        }
        public abstract int getCost();
        public abstract int getLimit();
        public abstract int getCurrentValue();
        public abstract int getUpgradeValue();
        public abstract string getPrefsString();
        public abstract string getDescription();
        public abstract bool isInPercent();

    }

    private class RuneVitality : RuneForgeItem
    {
        public override int getCost()
        {
            return 30 + currentBonus * 5;
        }

        public override int getCurrentValue()
        {
            return currentBonus;
        }

        public override string getDescription()
        {
            return $"Increase your vitality(<size=8><sprite name=\"Vitality\"></size>) by one (limit: {getLimit()})";
        }

        public override int getLimit()
        {
            return 5;
        }

        public override string getPrefsString()
        {
            return "RuneVitalityBonus";
        }

        public override int getUpgradeValue()
        {
            return currentBonus + 1;
        }

        public override bool isInPercent()
        {
            return false;
        }
    }


    private class RuneCritical : RuneForgeItem
    {
        public override int getCost()
        {
            return 10 + currentBonus * 4;
        }

        public override int getCurrentValue()
        {
            return (int)(100d * CriticalHitChanceCalculator.getCritChance(currentBonus));
        }

        public override string getDescription()
        {
            return "Increase your critical hit chance across all your spirit animal companions.";
        }

        public override int getLimit()
        {
            return -1;
        }

        public override string getPrefsString()
        {
            return "RuneCriticalChance";
        }

        public override int getUpgradeValue()
        {
            return (int)(100d * CriticalHitChanceCalculator.getCritChance(currentBonus + 1));
        }

        public override bool isInPercent()
        {
            return true;
        }
    }
}


public class CriticalHitChanceCalculator
{
    public static double getCritChance(int critChanceBase)
    {
        return (double)critChanceBase / (critChanceBase + 20d);
    }
}
