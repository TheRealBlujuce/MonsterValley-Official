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

}
