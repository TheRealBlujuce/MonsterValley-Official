using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleMon playerPemo;
    [SerializeField] BattleMon opponentPemo;
    [SerializeField] BattleUI playerUI;
    [SerializeField] BattleUI opponentUI;
    [SerializeField] BattleDialog dialogText;
    private void Start()
    {
        SetupBattle();
    }

    private void SetupBattle()
    {
        playerPemo.Setup();
        playerUI.SetData(playerPemo.GetPemo());

        opponentPemo.Setup();
        opponentUI.SetData(opponentPemo.GetPemo());

       StartCoroutine(dialogText.TypeDialog($"A Wild {opponentPemo.GetPemo().GetBaseMonster().GetName()} Appeared!"));

    }
}
