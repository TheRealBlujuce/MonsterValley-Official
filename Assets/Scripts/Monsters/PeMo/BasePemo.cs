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
    private Sprite pemoBack;

    [SerializeField]
    private PeMoType type;

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
    public Sprite GetBackSprite()
    {
        return pemoBack;
    }
    public PeMoType GetPeMoType()
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

public enum PeMoType
{
    Normal,
    Fire,
    Water,
    Grass,
    Electric,
    Flying,
    Ground,
    Fairy,
    Dragon
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