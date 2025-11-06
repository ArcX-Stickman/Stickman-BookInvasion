using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionForPlayer : MonoBehaviour
{

    public BoxCollider2D Bxcd;
    public BoxCollider2D StickBxcd;
    public Movement PlayerManager;
    // Start is called before the first frame update
    void Start()
    {
        Bxcd = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StickBxcd.IsTouching(Bxcd) && PlayerManager.isSliding)
        {
            Debug.Log("fALSE COLLISION");
            Physics2D.IgnoreCollision(Bxcd, StickBxcd, true);



        }
        if (StickBxcd.IsTouching(Bxcd) && PlayerManager.isDashing)
        {
            Debug.Log("fALSE COLLISION");
            Physics2D.IgnoreCollision(Bxcd, StickBxcd, true);
        }

    }
}
