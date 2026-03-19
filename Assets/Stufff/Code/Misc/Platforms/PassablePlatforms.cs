using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassablePlatforms : MonoBehaviour
{

    public Movement StickManager;
    public BoxCollider2D BoxCollider2D;
    public PlatformEffector2D PlatformEffector2D;

    public SpriteRenderer SR;
    private CapsuleCollider2D StickCap;

    void Start()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
        PlatformEffector2D = GetComponent<PlatformEffector2D>();
        SR = GetComponent<SpriteRenderer>();
        if(SR == null)
        {
            SR = GetComponentInChildren<SpriteRenderer>();
        }

        StickCap = StickManager.GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if(StickManager.rb.velocity.y > 0)
        {
            StartCoroutine(PreventStickVisualGlitch_StickJump2(false)); 
        }
        else
        {
            StartCoroutine(PreventStickVisualGlitch_StickJump2(true));

        }


        if (SR != null)
        {
            if (BoxCollider2D.IsTouching(StickCap) && StickManager.Grounded)
            {
                SR.sortingOrder = StickManager.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
            else
            {
                SR.sortingOrder = -10;
            }
        }
    }


    /// theres like this rlly weird gltich where if mark jumps on these platforms it causes his jump animation to happen twice.
    /// this happens because the miisceond he jumps the platform is turned off which causes the jump animation function to hallucinate thinking hes falling
    /// and this causes the jump animtion to be fired twice causing weird stuff.
    public IEnumerator PreventStickVisualGlitch_StickJump2(bool k)
    {

        yield return new WaitForSecondsRealtime(0.01f);

        switch (k)
        {
            case true:
                BoxCollider2D.enabled = true;
                PlatformEffector2D.enabled = true;
                break;
            case false:
                BoxCollider2D.enabled = false;
                PlatformEffector2D.enabled = false;
                break;
        }
            

        yield break;
    }
}
