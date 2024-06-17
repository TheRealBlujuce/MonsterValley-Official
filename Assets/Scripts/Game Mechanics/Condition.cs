using System;

public class Condition
{
    public string conditionName;
    public string conditionDescription;
    public string conditionMessage;
    public ConditionID currentConditionID;
    public Func<Pemo, bool> onBeforeMove {get; set;}
    public Action<Pemo> onAfterTurn {get; set;}

}
