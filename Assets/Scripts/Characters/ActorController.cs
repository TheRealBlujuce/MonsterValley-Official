using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public bool isMoving;
    public CharacterAnimator animator;
    
    
    public IEnumerator Move(Vector2 moveVector, float moveSpeed, Action onMoveOver=null)
    {
        animator.moveX = Mathf.Clamp(moveVector.x, -1, 1);
        animator.moveY = Mathf.Clamp(moveVector.y, -1, 1);

        var targetPos = transform.position;
        targetPos.x += moveVector.x;
        targetPos.y += moveVector.y;

        if( !IsPathClear(targetPos)) { yield break; }
        
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;

        onMoveOver?.Invoke();
    }

    private bool isWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.3f, 
        GameLayers.gameLayersInstance.CollisionLayer() | GameLayers.gameLayersInstance.InteractableLayer() | GameLayers.gameLayersInstance.PlayerLayer()) != null)
        {
            return false;
        }

        return true;
    }

    public void LookTowards(Vector3 targetPos)
    {
        var xDiff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
        var yDiff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

        if (xDiff == 0 || yDiff == 0)
        {
            animator.moveX = Mathf.Clamp(xDiff, -1, 1);
            animator.moveY = Mathf.Clamp(yDiff, -1, 1);
        }
        
    }

    private bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;
        if (Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude-1,
        GameLayers.gameLayersInstance.CollisionLayer() | GameLayers.gameLayersInstance.InteractableLayer() | GameLayers.gameLayersInstance.PlayerLayer()) == true)
        {
            return false;
        }
        
        return true;
    }

    public void HandleActorUpdate() { animator.isMoving = isMoving; }
}
