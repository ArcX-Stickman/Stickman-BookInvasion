using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class FallingDebris : MonoBehaviour
{
    public Rigidbody2D rb;

    public Movement Stick;

    [Header("Timing")]
    public float delay = 0.2f;
    public float randomDelay = 0.1f; 

    [Header("Fall Motion")]
    public float gravity = -25f;
    public float maxFallSpeed = -12f;
    public float horizontalDrift = 1.5f;

    public float fallSpeed = 0f;
    private bool falling = false;

    private float OGSpeed;

    [Header("Distance Based Stuff")]

    public bool DistanceBased;
    public float distance;
    public float SmallDis;
    public float mediumDis;
    public float longDis;
    public float WayToFar;

    [Header("Distance Based Speeds")]
    public float SlowSpeed;
    public float MediumSpeed;
    public float largeSpeed;
    public float extremeSpeed;


    void Awake()
    {
        OGSpeed = gravity;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; 
    }

    void OnEnable()
    {
        StartCoroutine(StartFallWithDelay());
    }

    private IEnumerator StartFallWithDelay()
    {
        yield return new WaitForSeconds(delay + Random.Range(0, randomDelay));
        falling = true;
        //rb.isKinematic = false;
    }

    private void Update() 
    {
        if(DistanceBased)
        {
            DistanceCal();
        }
    }
    void FixedUpdate()
    {
        if (!falling) return;

        fallSpeed += gravity * Time.fixedDeltaTime;
        fallSpeed = Mathf.Max(fallSpeed, maxFallSpeed);

        float drift = Mathf.Sin(Time.time * 2f) * horizontalDrift * Time.fixedDeltaTime;

        rb.velocity = new Vector2(drift, fallSpeed);
    }

    public void DistanceCal()
    {
        distance = Vector2.Distance(transform.position,Stick.transform.position);
        if(distance <= SmallDis)
        {
            gravity = SlowSpeed;   
        }
        else if(distance <= mediumDis)
        {
            gravity = MediumSpeed;
        }
        else if(distance <= longDis)
        {
            gravity = largeSpeed;
        }
        else if(distance < WayToFar)
        {
            gravity = extremeSpeed;
        }
        else
        {
            gravity = OGSpeed;
        }

    }
}
