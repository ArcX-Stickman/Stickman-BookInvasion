using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TreeHeliChase : MonoBehaviour
{
    public Movement Stickman;
    private bool AlrTouched;
    private bool AlrDid = false;
    public bool isExamFoot;
    public AudioClip Kick;
    public AudioSource AudioP;
    private float SFXCD = 0.5f;
    private float Last_SFX= 0;
    private bool HeliAnimStick;


    private void Update()
    {
        BoxCollider2D BXcd = GetComponent<BoxCollider2D>();
        HeliAnimStick = Stickman.StickAnim.GetBool("Heli");

        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") &&  Stickman.OnLadder && Stickman.StickAnim.GetBool("Heli") && HeliAnimStick && !Stickman.UpLadderWant)
        {
            Debug.Log("Tree Colllided Player");
            Debug.Log("Heli Fall");
            Stickman.isDashing = false;
            AlrTouched = true;
            Stickman.OnLadder = false;
            Stickman.isFallingHeli = true;
            Stickman.StartCoroutine(Stickman.BackFlipMove());
            // Stickman.rb.velocity = new Vector2(-Mathf.Abs(-100), Stickman.rb.velocity.y);
            Stickman.StickAnim.SetBool("Heli", false);





        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Stickman.OnLadder && Stickman.UpLadderWant)
        {
            if (isExamFoot && Time.time >= SFXCD + Last_SFX)
            {
                Stickman.StickHP.PlayerHP -= 15;
                Last_SFX = Time.time;
                AudioP.PlayOneShot(Kick);
            }
            Debug.Log("Heli Fall");
            Stickman.isDashing = false;
            AlrTouched = true;
            Stickman.OnLadder = false;
            Stickman.isFallingHeli = true;
            Stickman.StartCoroutine(Stickman.BackFlipMove());
            Stickman.StickAnim.SetBool("Heli", false);




        }
    }



}
