using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTon_Smash_Wind : MonoBehaviour
{
    public bool PlayerDodged;
    public Vector2 PushBack;
    public Movement StickManager;
    private bool Hit;
    public Transform PhysicsBookPOS;
    public float Multiplier;
    private float OGForce;
    public float MaxTime;
    public BookAI BookAI;

    private void OnEnable()
    {
        OGForce = PushBack.x;
        PushBack.x = OGForce;
        PlayerDodged = false;
        Hit = false;
        StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !PlayerDodged) 
        {
            if (StickManager.BackFlip && !Hit)
            {
                PlayerDodged = true;
            }
            else if(!Hit)
            {
                Hit = true;
                StickManager.StickHP.PlayerHP -= 10;
                StartCoroutine(StickManager.PlayerKnockbackBouncy(200, 10, 2f, 4, 1.5f));

//                StartCoroutine(BlowPlayerAway());
            }

        }
    }

    public IEnumerator BlowPlayerAway()
    {

        Vector2 direction = PhysicsBookPOS.localScale.x > 0 ? Vector2.right : Vector2.left;
        Debug.Log("Direction Of ViolentWind Push:  " + direction);
        float timer = 0;
        //StickManager.Jump();

        //// StickManager.StickAnim.CrossFade("Backflip", 0, 0);
        //while (timer < MaxTime)
        //{
        //    StickManager.StickAnim.CrossFade("StickKnockBack", 0, 0);

        //    StickManager.rb.velocity = new Vector2(transform.localScale.x * ((StickManager.StickDirection)), StickManager.BackFlipForce);
        //    timer += Time.deltaTime;
        //    PushBack.x += Multiplier * Time.deltaTime;
        //    StickManager.rb.AddForce(-direction * Multiplier,ForceMode2D.Force);
        //    yield return null;
        //}
        //StickManager.Falling_KnockBack = false;

        yield return null;
    }
}
