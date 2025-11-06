using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Blackhole : MonoBehaviour
{
    public float speed;
    public Health PlayerHP;
    public Movement Stick;
    public float DMG;
    public Animator Boom;
    private float Direction;
    public Transform StickPOS;
    public CircleCollider2D CirCD;
    public bool HitPlayer;
    public Animator MainCamAnim;
    public AudioSource AudioP;
    public AudioClip Hit;
    public GameObject FX;
    public bool WantFX;
    public bool isApple;
    public bool IsDestoyableByGround;
    public float Speed;
    public Rigidbody2D rb;
    public Vector2 StartPos;
    private float Velocity;
    public Vector2 AppleAcceleration;
    public float MaxDistance;
    public float Dis;
    public bool isTut;
    public TextMeshProUGUI TutTXt;
    public string text;
    private bool AlrDisplayedText;
    private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        StartPos = new Vector2(transform.position.x, transform.position.y); 
        CirCD.enabled = true;
        if(rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            rb.rotation = 0;
            rb.WakeUp();
        }
  
    }

    // Update is called once per frame
    void Update()
    {
        sprite = GetComponent<SpriteRenderer>();

        float Distance = Vector2.Distance(transform.position, StickPOS.position);
        Dis = Distance; 
        
    }

    private void FixedUpdate()
    {
        if (!isApple)
        {
            if(Dis <= MaxDistance)
            {
                float moveSpeed = speed * Time.deltaTime;
                transform.Translate(moveSpeed, 0, 0);
                if(sprite != null)
                {
                    sprite.enabled = true;
                }
            }
            else
            {
                if(sprite!= null)
                {
                    sprite.enabled = false;
                }
            }
            


        }
        else
        {
            if(rb != null)
            {
                rb.WakeUp();
                Velocity = rb.velocity.x;
                rb.AddForce(AppleAcceleration * rb.mass);
            }
           




        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !HitPlayer)
        {
            if (isTut)
            {
                TutTXt.text = text;
                TutTXt.gameObject.SetActive(true);

            }
            HitPlayer = true;
            CirCD.enabled = false;

            if (!Stick.isDashing && !Stick.BackFlip)
            {
                if (WantFX)
                {
                    ObjectPooler.SpawnObj(FX, StickPOS.position, Quaternion.identity,ObjectPooler.PoolType.Partcles);
                }
                MainCamAnim.ResetTrigger("ShakeMelee");
                MainCamAnim.SetTrigger("ShakeMelee");
                Stick.StickAnim.SetTrigger("Hurt");
                PlayerHP.PlayerHP -= DMG;
                if(Boom != null)
                {
                    Boom.SetTrigger("boom");

                }
                AudioP.PlayOneShot(Hit);
            }
            else
            {
                PlayerHP.PlayerHP -= 0;
            }

        }

        if (collision.CompareTag("Ground") && IsDestoyableByGround)
        {
            if (WantFX)
            {
                Instantiate(FX, transform.position, Quaternion.identity);
            }
            HitPlayer = true;
            CirCD.enabled = false;
            ObjectPooler.ReturnObjectToPool(gameObject);


        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !HitPlayer)
        {
            Stick.CameraScript.StartCoroutine(Stick.CameraScript.ShakeCam(3.1f, 0.1f));
            if (isTut)
            {
                TutTXt.text = text;
                TutTXt.gameObject.SetActive(true);

            }
            HitPlayer = true;
            CirCD.enabled = false;
            Destroy(gameObject);
            if (!Stick.isDashing && !Stick.BackFlip)
            {
                if (WantFX)
                {
                    Instantiate(FX, StickPOS.position, Quaternion.identity);
                }
                
                Stick.StickAnim.SetTrigger("Hurt");
                PlayerHP.PlayerHP -= DMG;
                if(Boom != null)
                {
                    Boom.SetTrigger("boom");

                }
                AudioP.PlayOneShot(Hit);
            }
            else
            {
                PlayerHP.PlayerHP -= 0;
            }

        }

        if (collision.gameObject.CompareTag("Ground") && IsDestoyableByGround)
        {
            if (WantFX)
            {
                Instantiate(FX, transform.position, Quaternion.identity);
            }
            HitPlayer = true;
            CirCD.enabled = false;
            Destroy(gameObject);


        }
    }


    public void SettingDirection(float _direction)
    {
        Direction = _direction;

        // Flip the scale based on direction
        if (Mathf.Sign(transform.localScale.x) != Mathf.Sign(Direction))
        {
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z
            );
        }

        // Adjust speed direction
        speed = Mathf.Abs(speed) * Direction;
    }










    public void Destroy()
    {
        Destroy(gameObject);
    }



}
