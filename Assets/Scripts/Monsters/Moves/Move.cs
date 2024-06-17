using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    [SerializeField]
    private BaseMove baseMove;

    [SerializeField]
    private int MP;

    public Move(BaseMove bMove)
    {
        baseMove = bMove;
        MP = baseMove.GetMovePoints();
    }

    public void SetBaseMove(BaseMove newMove)
    {
        baseMove = newMove;
    }
    public BaseMove GetBaseMove()
    {
        return baseMove;
    }
    public void SetMP(int movePoints)
    {
        MP = movePoints;
    }
    public int GetMP()
    {
        return MP;
    }
    public int GetHP()
    {
        return MP;
    }


}
