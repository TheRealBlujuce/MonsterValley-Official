using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator
{
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] List<Sprite> frames;
    [SerializeField] private float frameRate;

    private int currentFrame;
    private float timer;


    public SpriteAnimator(List<Sprite> animFrames, SpriteRenderer charRenderer, float framesPerSecond = 0.16f)
    {
        frames = animFrames;
        renderer = charRenderer;
        frameRate = framesPerSecond;
    }

    public void Start()
    {
        currentFrame = 0;
        timer = 0;
        renderer.sprite = frames[0];
    }

    public void AnimateCharacter()
    {
        
        timer += Time.deltaTime;
        if (timer > frameRate)
        {
           currentFrame = (currentFrame += 1) % frames.Count;
           renderer.sprite = frames[currentFrame];
           timer -= frameRate;
        }
    }

    public List<Sprite> GetFrames()
    {
        return frames;
    }

}
