using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public float moveX, moveY;
    public bool isMoving;

    [SerializeField] List<Sprite> walkDownFrames;
    [SerializeField] List<Sprite> walkUpFrames;
    [SerializeField] List<Sprite> walkLeftFrames;
    [SerializeField] List<Sprite> walkRightFrames;

    // animation states
    SpriteAnimator walkDown;
    SpriteAnimator walkUp;
    SpriteAnimator walkLeft;
    SpriteAnimator walkRight;

    // current animation state
    SpriteAnimator currentAnimation;

    SpriteRenderer spriteRenderer;
    Animator animator;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // This will be to play the wiggle animation for the movement

        // These will be to play the animations for frame-by-frame animation for the character
        walkDown = new SpriteAnimator(walkDownFrames, spriteRenderer);
        walkUp = new SpriteAnimator(walkUpFrames, spriteRenderer);
        walkLeft = new SpriteAnimator(walkLeftFrames, spriteRenderer);
        walkRight = new SpriteAnimator(walkRightFrames, spriteRenderer);

        currentAnimation = walkDown;
    }

    private void Update()
    {
        var lastAnim = currentAnimation;
        var wasMoving = isMoving;

        if(moveX == 1) { currentAnimation = walkRight; } else if (moveX == -1) { currentAnimation = walkLeft; }
        else
        if(moveY == 1) { currentAnimation = walkUp; } else if (moveY == -1) { currentAnimation = walkDown; }

        if (currentAnimation != lastAnim || isMoving != wasMoving) { currentAnimation.Start(); }

        if(isMoving) {currentAnimation.AnimateCharacter();}
        else 
        {
            spriteRenderer.sprite = currentAnimation.GetFrames()[0];
        }

        animator.SetFloat("moveX", moveX); animator.SetFloat("moveY", moveY);
        animator.SetBool("isMoving", isMoving);

        wasMoving = isMoving;
    }

}
