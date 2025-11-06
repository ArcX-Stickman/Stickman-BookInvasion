using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHeliChase : MonoBehaviour
{

    public Movement Stick;
    private BoxCollider2D Bxcd2d;
    private SpriteRenderer sp;
    private float Dis;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        Bxcd2d = sp.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Dis <= 25)
        {
            Stick.rb.constraints = RigidbodyConstraints2D.None;

        }
        sp = GetComponent<SpriteRenderer>();
        Bxcd2d = sp.GetComponent<BoxCollider2D>();
        if (!Stick.OnLadder)
        {
            sp.enabled = false;
            Bxcd2d.enabled = false;
        }
        else
        {
            sp.enabled = true;
            Bxcd2d.enabled = true;
        }
        Dis = Vector2.Distance(transform.position, Stick.transform.position);

    }
}
