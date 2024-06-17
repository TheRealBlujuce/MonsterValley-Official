using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BattleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private HealthBar healthBar;

    [SerializeField] private Image statusImage;
    [SerializeField] private Sprite[] statusImages;

    // [SerializeField]
    // private TextMeshPro xpText;

    Pemo currentPemo;

    public void SetData(Pemo pemo)
    {
        currentPemo = pemo;
        nameText.text = pemo.GetBaseMonster().GetName();
        levelText.text = "Lvl " + pemo.GetCurrentLevel();
        healthBar.SetHealth((float) pemo.CurrentHP() / pemo.MaxHP());
        healthText.text = (float) pemo.CurrentHP() + " / " + pemo.MaxHP();
    }

    public IEnumerator UpdateHP()
    {
        if (currentPemo.CheckForChangeInHealth() == true)
        {
            healthText.text = (float) currentPemo.CurrentHP() + " / " + currentPemo.MaxHP();
            yield return healthBar.SetSmoothHP((float) currentPemo.CurrentHP() / currentPemo.MaxHP());
            currentPemo.ResetHealthChangedStatus();    
        }

    }

    public void SetCurrentStatusImage(Pemo pemo)
    {
        if (pemo.GetCurrentCondition() != null)
        {
            statusImage.sprite = statusImages[(int)pemo.GetCurrentCondition().currentConditionID];
        }
        else
        {
            statusImage.sprite = statusImages[0];
        }
    }
}
