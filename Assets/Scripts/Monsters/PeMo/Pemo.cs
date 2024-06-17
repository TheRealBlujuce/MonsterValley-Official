using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class Pemo
{
    [SerializeField] private BasePemo _basePemo;
    [SerializeField] private int _pemoLevel;
    private BasePemo basePemo {get {return _basePemo;} }
    private int pemoLevel {get {return _pemoLevel;} }
    private int currentHP;
    private Condition currentCondition;
    private Queue<string> statusChanges = new Queue<string>();
    [SerializeField] List<Move> pemoMoves = new List<Move>();
    private Move currentMove;
    private bool healthHasChanged;


    public void InitPemo()
    {
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

    #region Getters and Setters

        public BasePemo GetBaseMonster()
        {
            return basePemo;
        }
        public List<Move> GetPemoMoves()
        {
            return pemoMoves;
        }
        public Move GetCurrentMove()
        {
            return currentMove;
        }
        public void SetCurrentMove(Move move)
        {
            currentMove = move;
        }
        public int GetCurrentLevel()
        {
            return pemoLevel;
        }
        public Queue<string> GetStatusChanges()
        {
            return statusChanges;
        }
        public Condition GetCurrentCondition()
        {
            return currentCondition;
        }
        public void SetCurrentCondition(ConditionID conditionID)
        {
            if (currentCondition.currentConditionID != ConditionID.none) { return; }
            currentCondition = ConditionsTable.GetConditionTable()[conditionID];
            statusChanges.Enqueue($"{basePemo.GetName()} {currentCondition.conditionMessage}");
        }
        public void ResetCurrentCondition()
        {
            currentCondition = ConditionsTable.GetConditionTable()[ConditionID.none];
        }
        public void ResetHealthChangedStatus()
        {
            healthHasChanged = false;
        }
        public bool CheckForChangeInHealth()
        {
            return healthHasChanged;
        }
        
    #endregion

    #region Pemo Stats

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
            return Mathf.FloorToInt(basePemo.GetMaxHp() * pemoLevel / 100f) + 10 + pemoLevel;
        }
    
    #endregion

    public DamageDetails TakeDamage(Move move, Pemo attacker)
    {
        
        float critical = 1f;
        if (Random.value * 100f <= 6.25f) { critical = 2f; }

        float type = TypeChart.GetTypeEffectivness(move.GetBaseMove().GetMoveType(), basePemo.GetPeMoType());

        var damageDetails = new DamageDetails()
        {
            Fainted = false,
            Critical = critical,
            Type = type
        };

        if (move.GetBaseMove().CheckIsStatus() == false) // check if the move is a status move or not. If it is, we can skip any damage.
        {

            float attackStat = move.GetBaseMove().CheckIsSpecial()? attacker.Special() : attacker.Attack();
            float defenseStat = move.GetBaseMove().CheckIsSpecial()? attacker.Special() : attacker.Defense();
            

            float modifiers = Random.Range(0.85f, 1f) * type * critical;
            float a = (2 * attacker.GetCurrentLevel() + 10f ) / 250f;
            float d = a * move.GetBaseMove().GetMovePower() * ((float) attackStat / defenseStat) + 2f;
            int damage = Mathf.FloorToInt(d * modifiers);


            UpdateCurrentHP(damage);
            if (currentHP <= 0)
            {
                currentHP = 0;
                damageDetails.Fainted = true;
            }
    
        }

        return damageDetails;
    }
    public void UpdateCurrentHP(int damage)
    {
        healthHasChanged = true;
        currentHP = Mathf.Clamp(currentHP-= damage, 0, MaxHP());
    }
    public Move GetRandomMove()
    {
        var movesWithPP = pemoMoves.Where(x => x.GetMP() > 0).ToList();
        int r = UnityEngine.Random.Range(0, GetPemoMoves().Count);
        return movesWithPP[r];
    }
    
    
    public void PerformAfterTurnAction()
    {
        currentCondition?.onAfterTurn?.Invoke(this);
    }
    public bool PerformBeforeTurnAction()
    {
        if (currentCondition?.onBeforeMove != null)
        {
            // Debug.Log("Running Before Move Action");
            GetStatusChanges().Enqueue($"{GetBaseMonster().GetName()} is {currentCondition.conditionName.ToLower()}");
            return currentCondition.onBeforeMove(this);
        }

        return true;
    }
}

public class DamageDetails
{
    public bool Fainted {get; set;}
    public float Critical {get; set;}
    public float Type {get; set;}

}
