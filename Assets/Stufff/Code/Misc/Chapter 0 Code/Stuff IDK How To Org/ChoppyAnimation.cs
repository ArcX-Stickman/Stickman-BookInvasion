using UnityEngine;

public class ChoppyAnimator : MonoBehaviour
{
    public Animator animator;
    public int skipFrames = 2; // "On twos" = skips every 1 frame, "On threes" = skips every 2 frames

    private int frameCounter = 0;

    void Update()
    {
        frameCounter++;
        if (frameCounter % skipFrames == 0)
        {
            animator.speed = 1f; // Play animation for this frame
        }
        else
        {
            animator.speed = 0f; // Freeze animation for skipped frames
        }
    }
}
