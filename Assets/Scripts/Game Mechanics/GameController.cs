using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState { World, Battle, Dialog, Menu}

public class GameController : MonoBehaviour
{

    private GameState gameState = GameState.World;
    [SerializeField] PlayerController player;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera mainCam;
    [SerializeField] InventoryMenu inventoryMenu;
    [SerializeField] PartyMenu partyMenu;
    public static GameController gameControllerInstance;

    private void Awake()
    {
        gameControllerInstance = this;
        partyMenu.InitPartyMenuScreen();
        inventoryMenu.InitInventory();
        
    }

    private void Start()
    {
        player.OnEncountered += EnterBattleState;
        player.OnOpenMenu += EnterMenuState;
        battleSystem.EndBattle += EnterWorldState;
        inventoryMenu.OnMenuClose += ExitMenuState;
        DialogManager.dialogInstance.onShowDialog += () => { gameState = GameState.Dialog; };
        DialogManager.dialogInstance.onCloseDialog += () => { if(gameState == GameState.Dialog) {gameState = GameState.World;} };
        
    }

    private void EnterBattleState()
    {
        gameState = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        mainCam.gameObject.SetActive(false);

        var playerParty = player.GetComponent<PemoParty>();
        var wildPemo = FindObjectOfType<PemoEncounterTable>().GetRandmoPemo();
        battleSystem.StartBattle(playerParty, wildPemo);
    }

    private void EnterWorldState(bool didWin)
    {
        gameState = GameState.World;
        battleSystem.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);
    }

    private void EnterMenuState()
    {
        gameState = GameState.Menu;
        inventoryMenu.UpdatePartyMenuData(player.GetComponent<PemoParty>());
        inventoryMenu.gameObject.SetActive(true);
        
    }

    private void ExitMenuState()
    {
        gameState = GameState.World;
        inventoryMenu.gameObject.SetActive(false);
    }

    private void Update()
    {
        switch(gameState)
        {
            case GameState.World:
                player.HandlePlayerUpdate();
            break;
            case GameState.Battle:
                battleSystem.HandleBattleUpdate();
            break;
            case GameState.Dialog:
                DialogManager.dialogInstance.HandleDialogUpdate();
            break;
            case GameState.Menu:
                inventoryMenu.HandleMenuUpdate();
            break;
        }
    }


    public PlayerController GetPlayerController()
    {
        return player;
    }

    private void OnDestroy() 
    {
        if (gameControllerInstance != this) { Destroy(this.gameObject); }
    }

}
