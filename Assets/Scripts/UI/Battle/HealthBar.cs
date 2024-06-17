using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private GameObject healthbar;

    public void SetHealth(float healthNormalized)
    {
        healthbar.transform.localScale = new Vector3(healthNormalized, 1f, 1f);
    }

    public IEnumerator SetSmoothHP(float newHP)
    {
        float currentHP = healthbar.transform.localScale.x;
        float changeAmount = currentHP - newHP;

        while (currentHP - newHP > Mathf.Epsilon)
        {
            currentHP -= changeAmount * Time.deltaTime;
            healthbar.transform.localScale = new Vector3(currentHP, 1f, 1f);
            yield return null;
        }

        healthbar.transform.localScale = new Vector3(newHP, 1f, 1f);
    }

}
