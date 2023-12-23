using UnityEngine;
using UnityEngine.UI;

public class BattleMon : MonoBehaviour
{
    [SerializeField] BasePemo basePemo;
    [SerializeField] int pemoLevel;
    [SerializeField] bool isPlayerPemo;

    public Pemo pemo;
    public void Setup()
    {
        pemo = new Pemo(basePemo, pemoLevel);
        if (isPlayerPemo)
        {
            GetComponent<Image>().sprite = pemo.GetBaseMonster().GetBackSprite();
        }
        else
        {
            GetComponent<Image>().sprite = pemo.GetBaseMonster().GetFrontSprite();
        }
    }

    public Pemo GetPemo()
    {
        return pemo;
    }
}
