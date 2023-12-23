using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pemo
{
    [SerializeField]
    private BasePemo basePemo;
    private int pemoLevel;
    private int currentHP;

    [SerializeField]
    List<Move> pemoMoves = new List<Move>();
    public Pemo(BasePemo bPemo, int pLvl)
    {
        basePemo = bPemo;
        pemoLevel = pLvl;
        currentHP = MaxHP();
        // Get the moves for the PeMo
        foreach(var move in basePemo.GetLearnableMoves())
        {
            if (move.GetLevelLearned() <= pemoLevel)
            {
                pemoMoves.Add(new Move(move.GetBaseMove()));
                if (pemoMoves.Count >= 4)
                {
                    break;
                }
            }
        } 
    }

    public BasePemo GetBaseMonster()
    {
        return basePemo;
    }
    public int GetCurrentLevel()
    {
        return pemoLevel;
    }
    public int CurrentHP()
    {
        return currentHP;
    }
    public int Attack()
    {
        return Mathf.FloorToInt((basePemo.GetAttack() * pemoLevel) / 100f) + 5;
    }
    public int Defense()
    {
        return Mathf.FloorToInt((basePemo.GetDefense() * pemoLevel) / 100f) + 5;
    }
    public int Special()
    {
        return Mathf.FloorToInt((basePemo.GetSpecial() * pemoLevel) / 100f) + 5;
    }
    public int Speed()
    {
        return Mathf.FloorToInt((basePemo.GetSpeed() * pemoLevel) / 100f) + 5;
    }
    public int MaxHP()
    {
        return Mathf.FloorToInt(basePemo.GetMaxHp() * pemoLevel / 100f) + 10;
    }
}
