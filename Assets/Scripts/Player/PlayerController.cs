using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed = 2;
    private bool isMoving;
    private Vector2 playerInput;
    private Animator animator;
    [SerializeField]
    private LayerMask collisionLayer;
    [SerializeField]
    private LayerMask grassLayer;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void SetMoveSpeed(float newSpeed) { moveSpeed = newSpeed; }
    public bool GetIsMoving() { return isMoving; }
    private void MovePlayer()
    {
        if (!isMoving)
        {
            playerInput.x = Input.GetAxisRaw("Horizontal");
            playerInput.y = Input.GetAxisRaw("Vertical");

            if (playerInput.x != 0){playerInput.y = 0;}


            if (playerInput != Vector2.zero)
            {
                animator.SetFloat("moveX", playerInput.x);
                animator.SetFloat("moveY", playerInput.y);

                var targetPos = transform.position;
                targetPos.x += playerInput.x;
                targetPos.y += playerInput.y;

                if (isWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
                
            }
        }

        animator.SetBool("isMoving", isMoving);
    }
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;

        CheckForEncounters();
    }
    private bool isWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.3f, collisionLayer) != null)
        {
            return false;
        }

        return true;
    }
    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
           if( Random.Range(1, 101) <= 10 )
           {
            Debug.Log("Encounted a Monster!");
           }
        }
    }
    
    
    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }
}
