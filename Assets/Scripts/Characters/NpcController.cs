using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour, Interactable
{
    [SerializeField] private string npcName; 
    [SerializeField] private float moveSpeed; 
    [SerializeField] private Dialog dialog;
    [SerializeField] private List<Vector2> movementPattern;
    [SerializeField] private float timeBetweenMovement;
    private float idleTimer = 0;
    private int currentPatternIndex = 0;
    private NpcState state = NpcState.Idle;
    private Transform currentInitiator;
    [SerializeField] private ActorController actor;

    private void Awake()
    {
        actor = GetComponent<ActorController>();
    }


    public void Interact(Transform initiator)
    {
        currentInitiator = initiator;
        if(state == NpcState.Idle) { state = NpcState.Talking; StartCoroutine(Talk()); }
    }

    public IEnumerator Talk()
    {   
        // Interact with the np
        actor.LookTowards(currentInitiator.position);
        yield return DialogManager.dialogInstance.ShowDialog(npcName, dialog, () => { state = NpcState.Idle; idleTimer = 0f; });
    }

    private void Update()
    {

        if (state == NpcState.Idle) 
        { 
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenMovement) 
            { 
                idleTimer = 0;
                if(movementPattern != null)
                {
                    StartCoroutine(Walk()); 
                }
                
            }
        }
        
        actor.HandleActorUpdate();
    }

    private IEnumerator Walk()
    {
        state = NpcState.Walk;
        var oldPos = transform.position;

        yield return actor.Move(movementPattern[currentPatternIndex], moveSpeed);
        if(transform.position != oldPos)
        {
            currentPatternIndex = (currentPatternIndex += 1) % movementPattern.Count;
        }


        state = NpcState.Idle;
    }

}

public enum NpcState{ Idle, Walk, Talking}