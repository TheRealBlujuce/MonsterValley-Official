using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BattleUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI healthText;

    [SerializeField]
    private HealthBar healthBar;

    // [SerializeField]
    // private TextMeshPro xpText;

    public void SetData(Pemo pemo)
    {
        nameText.text = pemo.GetBaseMonster().GetName();
        levelText.text = "Lvl " + pemo.GetCurrentLevel();
        healthBar.SetHealth((float) pemo.CurrentHP() / pemo.MaxHP());
        healthText.text = (float) pemo.CurrentHP() + " / " + pemo.MaxHP();
    }
}
