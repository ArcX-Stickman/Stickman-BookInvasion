using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingGlassV2 : MonoBehaviour
{
    public Transform PredeterminedEndPosition;
    public float SpeedTilFall;
    public float SlowSpeed;
    public AudioSource AudioP;
    public AudioClip GlassCrack;

    public float Distance;
    public float MinDisBeforeImpact;
    public float MinDIs;
    public Transform PredeterminedFinalPOS;

    public bool Impact;
    private bool NotStartedCouroutine;
    private Vector2 FakeDeterm;
    private bool TsGoingAllDown;
    private bool PlayerCollided;

    private Movement Stick;
    public Rigidbody2D rb;

    [Header("Rb Gravity Values")]
    public float NormGravity = 1f;
    public float SlowGravity = 0.4f;
    public float HeavyGravity = 3f;

    private bool PlayerLeft;
    private float defaultPlayerMass;
    private MotionBlur MotionBlur;

    private void OnEnable()
    {
        MotionBlur = GetComponent<MotionBlur>();
        rb = GetComponent<Rigidbody2D>();
        Stick = FindAnyObjectByType<Movement>();

        if (Stick != null && Stick.rb != null)
            defaultPlayerMass = Stick.rb.mass;
    }

    void Update()
    {

        
        if(rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
        }

        MotionBlur.HandleMotionBlur();

        if (rb == null) return;

        FakeDeterm = new Vector2(transform.position.x, PredeterminedEndPosition.position.y);
        Distance = Mathf.Abs(transform.position.y - FakeDeterm.y);

        if (!Impact && !NotStartedCouroutine)
        {
            NotStartedCouroutine = true;
            StartCoroutine(Fall());
        }

        if (TsGoingAllDown)
        {
            Vector2 finalTarget = new Vector2(transform.position.x, PredeterminedFinalPOS.position.y);
            transform.position = Vector2.MoveTowards(transform.position, finalTarget, SpeedTilFall * Time.deltaTime);
        }
    }

    public IEnumerator Fall()
    {
        const float FailSafeTime = 8f;
        float timer = 0f;
        while (!PlayerCollided)
        {
            timer += Time.deltaTime;
            if (timer > FailSafeTime) yield break;

            if (rb != null)
                rb.gravityScale = 0.007f;

            if (Stick != null && Stick.rb != null)
                Stick.rb.mass = 1f;

            yield return null;
        }

        while (Distance > MinDisBeforeImpact)
        {
            timer += Time.deltaTime;
            if (timer > FailSafeTime) yield break;

            if (Stick != null && Stick.rb != null)
                Stick.rb.mass = 1f;

            if (rb != null)
                rb.gravityScale = NormGravity;

            FakeDeterm = new Vector2(transform.position.x, PredeterminedEndPosition.position.y);
            Distance = Mathf.Abs(transform.position.y - FakeDeterm.y);

            yield return null;
        }

        while (Distance > MinDIs)
        {
            timer += Time.deltaTime;
            if (timer > FailSafeTime) yield break;

            if (Stick != null && Stick.rb != null)
                Stick.rb.mass = 1f;

            if (rb != null)
                rb.gravityScale = SlowGravity;

            FakeDeterm = new Vector2(transform.position.x, PredeterminedEndPosition.position.y);
            Distance = Mathf.Abs(transform.position.y - FakeDeterm.y);

            yield return null;
        }

        if (rb != null)
            rb.gravityScale = HeavyGravity;

        if (Stick != null && Stick.rb != null)
            Stick.rb.mass = defaultPlayerMass;

        TsGoingAllDown = true;
    }

    public IEnumerator StickPlayerToPlatform()
    {
        const float MaxStickTime = 4f;
        float t = 0f;

        while (!PlayerLeft)
        {
            t += Time.deltaTime;
            if (t > MaxStickTime) yield break;

            if (Stick == null) yield break;

            float buffer = 0.05f;
            if (Stick.transform.position.y < transform.position.y + buffer)
            {
                Stick.transform.position = new Vector3(
                    Stick.transform.position.x,
                    transform.position.y + buffer,
                    Stick.transform.position.z
                );
            }

            if (Stick.BackFlip || Stick.JumpDodge || Stick.isDashing)
                yield break;

            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        PlayerCollided = true;
        StartCoroutine(StickPlayerToPlatform());
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        PlayerLeft = true;
    }
}
