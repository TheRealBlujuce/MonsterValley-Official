using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsTable : MonoBehaviour
{
    private static Dictionary<ConditionID, Condition> conditions = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.none,
            new Condition{
                conditionName = "none", 
                conditionMessage = "",
                currentConditionID = ConditionID.none
            }
        },
        {
            ConditionID.psn,
            new Condition{
                conditionName = "Poisoned", 
                conditionMessage = " has been poisoned!",
                currentConditionID = ConditionID.psn,
                onAfterTurn = (Pemo pemo) =>    {   pemo.UpdateCurrentHP(pemo.MaxHP() / 6); 
                                                    pemo.GetStatusChanges().Enqueue($"{pemo.GetBaseMonster().GetName()} took damage from its poison!");
                                                }
            }
        },
        {
            ConditionID.brn,
            new Condition{
                conditionName = "Burned", 
                conditionMessage = " has been burned!",
                currentConditionID = ConditionID.brn,
                onAfterTurn = (Pemo pemo) =>    {   pemo.UpdateCurrentHP(pemo.MaxHP() / 6); 
                                                    pemo.GetStatusChanges().Enqueue($"{pemo.GetBaseMonster().GetName()} took damage from its burn!"); 
                                                }
            }
        },
        {
            ConditionID.frst,
            new Condition{
                conditionName = "Frost Bitten", 
                conditionMessage = " has been frost bitten!",
                currentConditionID = ConditionID.frst,
                onAfterTurn = (Pemo pemo) =>    {   pemo.UpdateCurrentHP(pemo.MaxHP() / 6); 
                                                    pemo.GetStatusChanges().Enqueue($"{pemo.GetBaseMonster().GetName()} took damage from its frost bite!");
                                                }
            }
        },
        {
            ConditionID.par,
            new Condition{
                conditionName = "Paralyzed", 
                conditionMessage = " has been paralyzed!",
                currentConditionID = ConditionID.par,
                onBeforeMove = (Pemo pemo) =>   {   if (Random.Range(1, 5) == 1)
                                                    {
                                                        pemo.GetStatusChanges().Enqueue($"{pemo.GetBaseMonster().GetName()} is paralyzed and unable to move!");
                                                        return false;
                                                    }; 

                                                    return true;
                    
                                                }
            }
        },
        {
            ConditionID.drz,
            new Condition{
                conditionName = "Drowzy", 
                conditionMessage = " is feeling drowzy!",
                currentConditionID = ConditionID.drz,
                onBeforeMove = (Pemo pemo) =>   {   if (Random.Range(1, 5) == 1)
                                                    {
                                                        pemo.GetStatusChanges().Enqueue($"{pemo.GetBaseMonster().GetName()} is tired and unable to move!");
                                                        return false;
                                                    }; 

                                                    return true;
                    
                                                }
            }
        }
    };
    
    public static Dictionary<ConditionID, Condition> GetConditionTable()
    {
        return conditions;
    }
}

public enum ConditionID
{
    none, psn, brn, frst, drz, par
}

// Poison, Burn, and Frost Bitten inflict damage each turn
// Drowsy and Paralysis have a chance of making your Pemo skip its turn due to being too tired or unable to move.