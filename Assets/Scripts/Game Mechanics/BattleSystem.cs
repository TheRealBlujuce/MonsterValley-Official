using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public enum BattleState { START, ACTION_SELECTION, MOVE_SELECTION, PARTY_SCREEN, PERFORM_TURN, BUSY, BATTLE_OVER }
public enum BattleAction {FIGHT, PEMO, BAG, RUN}
public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleMon playerPemo;
    [SerializeField] BattleMon opponentPemo;
    [SerializeField] BattleDialog dialogText;
    [SerializeField] PartyScreen partyScreen;

    public BattleState state;
    public BattleState? lastState;
    private int currentAction;
    private int currentMove;
    private int currentMember;

    public event Action<bool> EndBattle;
    PemoParty playerParty;
    Pemo wildPemo;

    public void StartBattle(PemoParty _playerParty, Pemo _wildPemo)
    {
        playerParty = _playerParty; wildPemo = _wildPemo;
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        playerPemo.Setup(playerParty.GetHealthyPemo());
        playerPemo.GetPemoBattleUI().SetData(playerPemo.GetPemo());
        playerPemo.GetPemoBattleUI().SetCurrentStatusImage(playerPemo.pemo);

        opponentPemo.Setup(wildPemo);
        opponentPemo.GetPemoBattleUI().SetData(opponentPemo.GetPemo());
        opponentPemo.pemo.ResetCurrentCondition();

        dialogText.SetMoveNames(playerPemo.pemo.GetPemoMoves());
        partyScreen.InitPartyScreen();

        yield return dialogText.TypeDialog($"A Wild {opponentPemo.GetPemo().GetBaseMonster().GetName()} Appeared!");
        yield return new WaitForSeconds(1f);

        yield return ActionSelection();

    }

#region Battle States


    private IEnumerator ActionSelection()
    {
        state = BattleState.ACTION_SELECTION;
        dialogText.EnableDialogText(true);
        yield return dialogText.TypeDialog("What Will You Do?");

        if (currentAction != 0) { currentAction = 0; }
        dialogText.EnableActionSelector(true);
        dialogText.EnableMoveSelector(false);
    }
    private void OpenPartyScreen()
    {
        state = BattleState.PARTY_SCREEN;
        dialogText.EnableActionSelector(false);
        partyScreen.SetPartyData(playerParty.GetPemoParty());
        partyScreen.gameObject.SetActive(true);
    }
    private void MoveSelection()
    {
        state = BattleState.MOVE_SELECTION;
        if (currentMove != 0) { currentMove = 0; }
        dialogText.EnableActionSelector(false);
        dialogText.EnableDialogText(false);
        dialogText.EnableMoveSelector(true);
    }

    private void BattleIsOver(bool hasWon)
    {
        state = BattleState.BATTLE_OVER;
        EndBattle(hasWon);
    }

#endregion

#region Performing Moves and Taking Damage

    private IEnumerator PerformTurns(BattleAction playerAction) // run the player and ai turns
    {
        state = BattleState.PERFORM_TURN;

        switch (playerAction)
        {
            // Performing a Move 
            case BattleAction.FIGHT:

                // get the moves for the player pemo and the opponent pemo
                playerPemo.pemo.SetCurrentMove(playerPemo.pemo.GetPemoMoves()[currentMove]);
                opponentPemo.pemo.SetCurrentMove(opponentPemo.pemo.GetRandomMove());

                // check who goes first
                bool playerGoesFirst = playerPemo.pemo.Speed() >= opponentPemo.pemo.Speed();

                var firstBattlePemo = (playerGoesFirst) ? playerPemo : opponentPemo;
                var secondBattlePemo = (playerGoesFirst) ? opponentPemo : playerPemo;


                // perform the player turn
                yield return PerformMove(firstBattlePemo, secondBattlePemo, firstBattlePemo.pemo.GetCurrentMove());
                yield return PerformAfterTurn(firstBattlePemo);
                if (state == BattleState.BATTLE_OVER) { yield break; } //stop the battle if the opponent is fainted or if the player has no more pemo
                
                if (secondBattlePemo.pemo.CurrentHP() > 0)
                {
                    // perform the opponent turn
                    yield return PerformMove(secondBattlePemo, firstBattlePemo, secondBattlePemo.pemo.GetCurrentMove());
                    yield return PerformAfterTurn(secondBattlePemo);
                    if (state == BattleState.BATTLE_OVER) { yield break; } //stop the battle if the opponent is fainted or if the player has no more pemo
                }

            break;

            // Switching Pemo
            case BattleAction.PEMO:

                
                var selectedMember = playerParty.GetPemoParty()[currentMember];
                state = BattleState.BUSY;
                yield return SwitchPemo(selectedMember);

                // After switching, it is now the opponents turn
                opponentPemo.pemo.SetCurrentMove(opponentPemo.pemo.GetRandomMove());
                // perform the opponent turn
                yield return PerformMove(opponentPemo, playerPemo, opponentPemo.pemo.GetCurrentMove());
                yield return PerformAfterTurn(opponentPemo);
                if (state == BattleState.BATTLE_OVER) { yield break; } //stop the battle if the opponent is fainted or if the player has no more pemo

            break;
        }

        if (state != BattleState.BATTLE_OVER) { yield return ActionSelection(); }
    }

    private IEnumerator PerformMove(BattleMon actingPemo, BattleMon defendingPemo, Move move) // Run the move or action of the currently acting Pemo
    {

        bool canPerfomMove = actingPemo.pemo.PerformBeforeTurnAction();

        yield return ShowStatusChanges(actingPemo.pemo);

        if(canPerfomMove == false) { yield return ShowStatusChanges(actingPemo.pemo); yield break; }

        move.SetMP(move.GetMP()-1);

        yield return dialogText.TypeDialog(
            actingPemo.pemo.GetBaseMonster().GetName() + " used " + move.GetBaseMove().GetMoveName() +"!");
            // end of dialog
        
        // check to see if the move actually hits. If it does not, we display a message and move on.
        if (CheckIfMoveHits(move))
        {
            // play attack and hit animation
            actingPemo.AttackAnimation();

            yield return new WaitForSeconds(0.5f);

            defendingPemo.HitAnimation();

            // apply damage to the oposing Pemo
            var damageDetails = defendingPemo.pemo.TakeDamage(move, actingPemo.pemo);
            yield return defendingPemo.GetPemoBattleUI().UpdateHP();
            yield return ShowDamageDetails(damageDetails);
            
            // If the move has a status condition, show that the condition has been applied to the target
            if (move.GetBaseMove().GetMoveEffects().GetStatusEffect() != ConditionID.none && !damageDetails.Fainted)
            {
            yield return PerformMoveEffects(move, actingPemo.pemo, defendingPemo.pemo, defendingPemo);
            }

            // check if the current pemo has fainted
            if (damageDetails.Fainted)
            {
                // Debug.Log("Checking for Fainted Pemo");
                if (defendingPemo.GetIsPlayerPemo() == true) 
                {
                    yield return dialogText.TypeDialog(defendingPemo.pemo.GetBaseMonster().GetName() + " has fainted!");
                }
                else
                {
                    yield return dialogText.TypeDialog("The wild " + defendingPemo.pemo.GetBaseMonster().GetName() + " has fainted!");
                }
                
                // reset the fainted pemo's current condition
                defendingPemo.pemo.ResetCurrentCondition();
                defendingPemo.GetPemoBattleUI().SetCurrentStatusImage(defendingPemo.pemo);
                
                defendingPemo.FaintAnimation();

                yield return new WaitForSeconds(1.5f);

                CheckIfBattleOver(defendingPemo);
            }

        }
        else // display the message
        {
            if(actingPemo.GetIsPlayerPemo())
            {
                yield return dialogText.TypeDialog(actingPemo.pemo.GetBaseMonster().GetName() + "'s move missed!");
            }
            else
            {
                yield return dialogText.TypeDialog("The wild " + actingPemo.pemo.GetBaseMonster().GetName() + "'s move missed!");
            }
        }
        
    }
    private IEnumerator PerformAfterTurn(BattleMon currentPemo) // perform any damage from effects and also check if a pemo has fainted after the turn ends
    {
        
        if (state == BattleState.BATTLE_OVER) { yield break; } //stop the battle if the opponent is fainted or if the player has no more pemo
        yield return new WaitUntil(() => state == BattleState.PERFORM_TURN); // wait until we are performing the turn again

        // Debug.Log("Applying After Turn Effects");
        // Apply effects of poison, burn, or frost bite after the end of the turn.
        currentPemo.pemo.PerformAfterTurnAction();
        yield return ShowStatusChanges(currentPemo.pemo);
        yield return currentPemo.GetPemoBattleUI().UpdateHP();

        // check if the current pemo has fainted
        if (currentPemo.pemo.CurrentHP() <= 0)
        {
            // Debug.Log("Checking for Fainted Pemo");
            if (currentPemo.GetIsPlayerPemo() == true) 
            {
                yield return dialogText.TypeDialog(currentPemo.pemo.GetBaseMonster().GetName() + " has fainted!");
            }
            else
            {
                yield return dialogText.TypeDialog("The wild " + currentPemo.pemo.GetBaseMonster().GetName() + " has fainted!");
            }
            
            // reset the fainted pemo's current condition
            currentPemo.pemo.ResetCurrentCondition();
            currentPemo.GetPemoBattleUI().SetCurrentStatusImage(currentPemo.pemo);
            
            currentPemo.FaintAnimation();

            yield return new WaitForSeconds(1.5f);

            CheckIfBattleOver(currentPemo);
        }
    }
    private void CheckIfBattleOver(BattleMon faintedPemo)
    {
        // Debug.Log("Checking if the Battle Is Over");
        if ( faintedPemo.GetIsPlayerPemo() == true)
        {
            var nextPemo = playerParty.GetHealthyPemo();
            if (nextPemo != null) { OpenPartyScreen(); } 
            else 
            { 
                BattleIsOver(false); 
            }
        }
        else
        {
            BattleIsOver(true);
        }
    }

    private IEnumerator ShowDamageDetails(DamageDetails details)
    {
        
        if (details.Critical > 1f) { yield return dialogText.TypeDialog("Critical Hit!"); }
        if (details.Type > 1f) { yield return dialogText.TypeDialog("The Damage Was Effective!"); } 
        else 
        if (details.Type < 1f) { yield return dialogText.TypeDialog("The Damage Was Not Very Effective.."); }
    }

    private bool CheckIfMoveHits(Move move)
    {
        float moveAccuracy = move.GetBaseMove().GetMoveAccuracy();

        return UnityEngine.Random.Range(1, 101) <= moveAccuracy;
    } 
    
   

#endregion 

#region Player Action and Move Selection

    private void HandlePlayerActionSelection() 
    { 
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentAction++;
            if (currentAction > 3) { currentAction = 0; }
        }
        else
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentAction--;
            if (currentAction < 0) { currentAction = 3; }
        }

        dialogText.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            switch(currentAction)
            {
                case 0:
                    // Select A Move
                    MoveSelection();
                break;
                case 1:
                    // Change Your PeMo
                    lastState = state;
                    OpenPartyScreen();
                break;
                case 2:
                    // Access Your Bag
                break;
                case 3:
                    // Run Away!
                break;
            }
        }
    }    
    
    private void HandlePlayerMoveSelection() 
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMove++;
            if (currentMove > playerPemo.pemo.GetPemoMoves().Count - 1) { currentMove = 0; }
        }
        else
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMove--;
            if (currentMove < 0) { currentMove = playerPemo.pemo.GetPemoMoves().Count - 1; }
        }

        dialogText.UpdateMoveSelection(currentMove, playerPemo.pemo.GetPemoMoves()[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMove = playerPemo.pemo.GetPemoMoves()[currentMove];
            if (selectedMove.GetMP() > 0)
            {
                dialogText.EnableMoveSelector(false);
                dialogText.EnableDialogText(true);
                StartCoroutine(PerformTurns(BattleAction.FIGHT));
            }
        }

        // Go back too the player action selection
        if (Input.GetKeyDown(KeyCode.X))
        {
            dialogText.EnableMoveSelector(false);
            dialogText.EnableDialogText(true);
            StartCoroutine(ActionSelection());
        }
    }

    private void HandlePartyScreenSelection() 
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMember++;
            if (currentMember > playerParty.GetPemoParty().Count -1) { currentMember = 0; }
        }
        else
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMember--;
            if (currentMember < 0) { currentMember = playerParty.GetPemoParty().Count -1; }
        }

        partyScreen.UpdatePartySelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMember = playerParty.GetPemoParty()[currentMember];
            if (selectedMember.CurrentHP() <= 0) { partyScreen.SetMessageText("That Pemo Is Unable To Battle."); return; }
            if (selectedMember == playerPemo.pemo) { partyScreen.SetMessageText("That Pemo Is Already In Battle!"); return; }
            
            partyScreen.gameObject.SetActive(false);

            if (lastState == BattleState.ACTION_SELECTION)
            {
                lastState = null;
                StartCoroutine(PerformTurns(BattleAction.PEMO));
            }
            else
            {
                state = BattleState.BUSY;
                StartCoroutine(SwitchPemo(selectedMember));
            }
            
        }

        // Go back too the player action selection
        if (Input.GetKeyDown(KeyCode.X))
        {
            partyScreen.gameObject.SetActive(false);
            StartCoroutine(ActionSelection());
        }
    }

#endregion

    private IEnumerator PerformMoveEffects(Move move, Pemo activePemo, Pemo targetPemo, BattleMon targetBattleMon) // This is to perform any effects due to a move like taking poision, burn, or frost bite damage, etc.
    {
        var effects = move.GetBaseMove().GetMoveEffects();
        // Debug.LogWarning(effects.GetStatusEffect());

        if ( effects.GetStatusEffect() != ConditionID.none)
        {
            targetPemo.SetCurrentCondition(effects.GetStatusEffect());
            // Debug.LogWarning(targetPemo.GetCurrentCondition().conditionName);
        }

        yield return new WaitForSeconds(1f);
        targetBattleMon.GetPemoBattleUI().SetCurrentStatusImage(targetPemo);
        yield return ShowStatusChanges(targetPemo);
    }

    private IEnumerator ShowStatusChanges(Pemo pemo)
    {
        while( pemo.GetStatusChanges().Count > 0)
        {
            var message = pemo.GetStatusChanges().Dequeue();
            yield return dialogText.TypeDialog(message);
            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator SwitchPemo(Pemo newPemo)
    {
        if (playerPemo.pemo.CurrentHP() > 0)
        {

            yield return dialogText.TypeDialog($"Come back {playerPemo.pemo.GetBaseMonster().GetName()}!");
            playerPemo.FaintAnimation();

            yield return new WaitForSeconds(1f);

        }
        
        playerPemo.Setup(newPemo);
        playerPemo.GetPemoBattleUI().SetData(newPemo);
        playerPemo.GetPemoBattleUI().SetCurrentStatusImage(newPemo);
        dialogText.SetMoveNames(newPemo.GetPemoMoves());

        yield return dialogText.TypeDialog($"Get Out There {playerPemo.pemo.GetBaseMonster().GetName()}!");
        yield return new WaitForSeconds(1.25f);

       state = BattleState.PERFORM_TURN;
        
        
    }

    public void HandleBattleUpdate()
    {
        switch(state)
        {
            case BattleState.ACTION_SELECTION:
                // Handle the selectioon of the Actions
                HandlePlayerActionSelection();
            break;
            case BattleState.MOVE_SELECTION:
                // Handle the selection of the Moves
                HandlePlayerMoveSelection();
            break;
            case BattleState.PARTY_SCREEN:
                // Handle the selection of the Moves
                HandlePartyScreenSelection();
            break;
        }

    }

}
