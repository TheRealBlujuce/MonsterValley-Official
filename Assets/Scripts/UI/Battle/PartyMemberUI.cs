using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image pemoSprite;

    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private HealthBar healthBar;

    // [SerializeField]
    // private TextMeshPro xpText;

    Pemo currentPemo;

    public void SetData(Pemo pemo)
    {
        currentPemo = pemo;
        pemoSprite.sprite = currentPemo.GetBaseMonster().GetFrontSprite();
        nameText.text = pemo.GetBaseMonster().GetName();
        levelText.text = "Lvl " + pemo.GetCurrentLevel();
        healthBar.SetHealth((float) pemo.CurrentHP() / pemo.MaxHP());
        healthText.text = (float) pemo.CurrentHP() + " / " + pemo.MaxHP();
    }

    public IEnumerator UpdateHP()
    {
        yield return healthBar.SetSmoothHP((float) currentPemo.CurrentHP() / currentPemo.MaxHP());
        healthText.text = (float) currentPemo.CurrentHP() + " / " + currentPemo.MaxHP();
    }
}
