using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist_AdrelineRush : MonoBehaviour
{
    public Transform Stickman;
    public Movement StickMove;
    public float Speed;
    public Animator Animator;
    public HitMarker HitMarker;
    public bool Dead;
    private float Distance;
    private float MaxDis = 800;
    void Update()
    {
        Animator = GetComponent<Animator>();
        HitMarker = GetComponent<HitMarker>();
        Vector2 StickPosFaked = new Vector2(Stickman.position.x,transform.position.y);
        if(!Dead)
        {
            transform.position = Vector2.MoveTowards(transform.position,StickPosFaked, Speed * Time.deltaTime);
            Animator.SetBool("Run", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && StickMove.isSliding)
        {
            Dead = true;
            Animator.SetTrigger("Dead");
            this.enabled = false;
        }
    }
}
