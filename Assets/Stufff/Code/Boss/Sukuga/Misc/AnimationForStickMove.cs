using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationForStickMove : MonoBehaviour
{

    public Animator animator;
    public Rigidbody2D rb;
    public string AnimBool;
    public float SpeedMin;
    public bool MustMatchDirec;
    private float transStart;
    public Movement StickM;
    public StickCinematicVer_BossFight StickSukuga;
    public GameObject Stick;
    public float Gap;
    private void Start()
    {
        if(StickSukuga != null)
        {
           Stick = StickSukuga.gameObject;
        }
        else if(StickM != null)
        {
            Stick = StickM.gameObject;
        }
        if (MustMatchDirec)
        {
            transStart = transform.localScale.x;
        }
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(rb.velocity.x) >= SpeedMin)
        {
            animator.SetBool(AnimBool, true);
        }
        else
        {
            if (!InWait)
            {
                StartCoroutine(WaitForStop());
            }
        }

        if (MustMatchDirec)
        {
            if(StickSukuga != null && StickSukuga.InBackflip || StickM != null && !StickM.BackFlip)
            {
                transform.localScale = new Vector3(transStart * -Stick.transform.localScale.x, transform.localScale.y);
            }
            else
            {
                if (rb.transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(transStart, transform.localScale.y);
                }
                else
                {
                    transform.localScale = new Vector3(-transStart, transform.localScale.y);
                }
            }
           
        }
    }

    private bool InWait;
    public IEnumerator WaitForStop()
    {
        InWait = true;
        yield return new WaitForSecondsRealtime(Gap);
        animator.SetBool(AnimBool, false);
        InWait = false;
        yield break;

    }
}
