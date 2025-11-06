using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaterpillarSquish : MonoBehaviour
{
    public Animator animator;
    public AudioSource AudioP;
    public AudioClip Squish;
    public Transform StickmanPOS;
    private SpriteRenderer Sprite;

    private void Update()
    {
        Sprite = GetComponent<SpriteRenderer>();
        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
        if (targetPosition.x < transform.position.x)
        {
            Sprite.flipX = true;

        }
        else if (targetPosition.x > transform.position.x)
        {
            Sprite.flipX = false;

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Squish");
        }
        if (collision.gameObject.CompareTag("Book"))
        {
            animator.SetTrigger("Squish");

        }
        if (collision.gameObject.CompareTag("Fox"))
        {
            animator.SetTrigger("Squish");

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fox"))
        {
            animator.SetTrigger("Squish");

        }
    }
    public void SquishSFX()
    {
        AudioP.PlayOneShot(Squish);
    }
}
