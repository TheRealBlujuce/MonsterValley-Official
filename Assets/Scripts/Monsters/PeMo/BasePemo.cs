using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Monster", menuName = "Monster/Create New Monster")]
public class BasePemo : ScriptableObject
{
    [SerializeField]
    private string pemoName;

    [TextArea]
    [SerializeField]
    private string pemoDescription;

    [SerializeField]
    private Sprite pemoFront;

    [SerializeField]
    private PemoType type;

    [SerializeField]
    private int maxHp;

    [SerializeField]
    private int attack;

    [SerializeField]
    private int defense;

    [SerializeField]
    private int special;

    [SerializeField]
    private int speed;

    [SerializeField]
    private List<LearnableMoves> learnableMoves = new List<LearnableMoves>();


    public string GetName()
    {
        return pemoName;
    }
    public string GetDescription()
    {
        return pemoDescription;
    }
    public Sprite GetFrontSprite()
    {
        return pemoFront;
    }
    public PemoType GetPeMoType()
    {
        return type;
    }
    public int GetMaxHp()
    {
        return maxHp;
    }
    public int GetAttack()
    {
        return attack;
    }
    public int GetDefense()
    {
        return defense;
    }
    public int GetSpecial()
    {
        return special;
    }
    public int GetSpeed()
    {
        return speed;
    }
    public List<LearnableMoves> GetLearnableMoves()
    {
        return learnableMoves;
    }

}

public enum PemoType
{
    None, // only used for "null" types
    Normal, // only used for moves and not for PeMo
    Fire,
    Water,
    Grass,
    Flying,
    Bug,
    Electric,
    Psychic,
    Fighting
}

[System.Serializable]
public class LearnableMoves
{
    [SerializeField]
    private BaseMove baseMove;
    
    [SerializeField]
    private int levelLearned;

    public BaseMove GetBaseMove()
    {
        return baseMove;
    }

    public int GetLevelLearned()
    {
        return levelLearned;
    }

}

public class TypeChart
{
    private static float[][] chart =
    {                              /*N*/   /*F*/   /*W*/   /*G*/   /*FL*/   /*B*/   /*E*/   /*P*/   /*FHGT*/
        /*Normal*/      new float[] {1f,    1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f},
        /*Fire*/        new float[] {1f,    1f,   0.5f,     2f,     1f,     2f,     1f,     1f,     1f},
        /*Water*/       new float[] {1f,    2f,     1f,   0.5f,     1f,     1f,   0.5f,     1f,     1f},
        /*Grass*/       new float[] {1f,  0.5f,     2f,     1f,     1f,   0.5f,     1f,     1f,     1f},
        /*Flying*/      new float[] {1f,    1f,     1f,     2f,     1f,     2f,   0.5f,     1f,   0.5f},
        /*Bug*/         new float[] {1f,  0.5f,     1f,     2f,     1f,     1f,     1f,     2f,     1f},
        /*Electric*/    new float[] {1f,    1f,     2f,     1f,     2f,     1f,     1f,     1f,   0.5f},
        /*Psychic*/     new float[] {1f,    1f,     1f,     1f,     1f,   0.5f,     1f,     1f,     2f},
        /*Fighting*/    new float[] {1f,    1f,     1f,     1f,   0.5f,   0.5f,     1f,     2f,     2f},
    };

    public static float GetTypeEffectivness(PemoType attackType, PemoType defenseType)
    {
        if (attackType == PemoType.None || defenseType == PemoType.None) { return 1; }

        // get the specific row and col of the chart
        int row = (int)attackType - 1; 
        int col = (int)defenseType - 1; 

        return chart[row][col]; // return the row and col of the chart
    }
}