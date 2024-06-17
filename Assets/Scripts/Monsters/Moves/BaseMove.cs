using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Move", menuName ="Monster/Create New Move")]
public class BaseMove : ScriptableObject
{
    [SerializeField] private string moveName;

    [TextArea]
    [SerializeField] private string moveDescription;
    [SerializeField] private PemoType moveType;
    [SerializeField] private int power;
    [SerializeField] private int accuracy;
    [SerializeField] private int movePoints;
    [SerializeField] private bool isSpecial;
    [SerializeField] private bool isStatus;

    [SerializeField] MoveEffects moveEffects;

    public string GetMoveName()
    {
        return moveName;
    }
    public string GetMoveDescription()
    {
        return moveDescription;
    }
    public PemoType GetMoveType()
    {
        return moveType;
    }
    public int GetMovePower()
    {
        return power;
    }
    public int GetMoveAccuracy()
    {
        return accuracy;
    }
    public int GetMovePoints()
    {
        return movePoints;
    }

    public bool CheckIsSpecial()
    {
        return isSpecial;
    }
    public bool CheckIsStatus()
    {
        return isStatus;
    }

    public MoveEffects GetMoveEffects()
    {
        return moveEffects;
    }
 
}

[System.Serializable]
public class MoveEffects
{
    [SerializeField] private ConditionID statusEffect;

    public ConditionID GetStatusEffect()
    {
        return statusEffect;
    }
}
