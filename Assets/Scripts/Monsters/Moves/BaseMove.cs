using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Move", menuName ="Monster/Create New Move")]
public class BaseMove : ScriptableObject
{
    [SerializeField]
    private string moveName;

    [TextArea]
    [SerializeField]
    private string moveDescription;

    [SerializeField]
    private PeMoType moveType;

    [SerializeField]
    private int power;

    [SerializeField]
    private int accuracy;

    [SerializeField]
    private int movePoints;

    public string GetMoveName()
    {
        return moveName;
    }
    public string GetMoveDescription()
    {
        return moveDescription;
    }
    public PeMoType GetMoveType()
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
 
}
