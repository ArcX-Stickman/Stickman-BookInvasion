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
            BoxCollider2D.enabled = false;
            PlatformEffector2D.enabled = false ;
        }
        else
        {
            BoxCollider2D.enabled = true;
            PlatformEffector2D.enabled = true;

        }


        if(SR != null)
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
}
