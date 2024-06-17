using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed = 2;
    private Vector2 playerInput;
    [SerializeField] private ActorController actor;
    private Camera playerCam;
    [SerializeField] float camFriction = 0.25f;
    
    public event Action OnEncountered;
    public event Action OnOpenMenu;


    private void Awake()
    {
        actor = GetComponent<ActorController>();
        playerCam = Camera.main;
    }

    public void SetMoveSpeed(float newSpeed) { moveSpeed = newSpeed; }
    
    private void MovePlayer()
    {
        if (!actor.isMoving)
        {
            playerInput.x = Input.GetAxisRaw("Horizontal");
            playerInput.y = Input.GetAxisRaw("Vertical");

            if (playerInput.x != 0){playerInput.y = 0;}

            if (playerInput != Vector2.zero)
            {
                StartCoroutine(actor.Move(playerInput, moveSpeed, CheckForEncounters));
            }
        }
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.gameLayersInstance.GrassLayer()) != null)
        {
            if( UnityEngine.Random.Range(1, 101) <= 10 )
            {
                actor.isMoving = false;
                OnEncountered();
            }
        }
    }
    
    private void MoveCamera()
    {
        Vector3 cameraTargetPos = Vector3.Lerp(playerCam.transform.localPosition, this.transform.localPosition, camFriction * Time.deltaTime);
        playerCam.transform.localPosition = new Vector3(cameraTargetPos.x, cameraTargetPos.y, -10f);
    }

    private void PlayerInteract()
    {
        var faceDir = new Vector3(actor.animator.moveX, actor.animator.moveY);

        var interactPos = transform.position + faceDir;

        var interactCollision = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.gameLayersInstance.InteractableLayer());

        if (interactCollision != null)
        {
            interactCollision.GetComponent<Interactable>()?.Interact(transform);
        }
    }

    private void OpenMenuScreen()
    {
        if (Input.GetKeyDown(KeyCode.C)) { OnOpenMenu(); }
    }

    // Update is called once per frame
    public void HandlePlayerUpdate()
    {
        MovePlayer();
        actor.HandleActorUpdate();
        if(Input.GetKeyDown(KeyCode.Z)) { PlayerInteract(); }
        OpenMenuScreen();
    }

    private void LateUpdate() 
    {
        MoveCamera();
    }
}
