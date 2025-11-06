using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{

    public Rigidbody2D rb;
    public Health PlayerHP;
    public float Distance;
    public Transform Player;
    public float MinDis;
    private float DelayCD = 0.5f;
    private float LastDelay;
    public float DMG;
    public bool isDifferent;
    public bool DrainStamina;
    public bool WantToBeGoneIfTouchPlayer;
    public AudioSource AudioP;
    public AudioClip Hit;
    public bool Hybrid;
    public Movement Stickman;

    // Start is called before the first frame update
    void Start()
    {
        Stickman = PlayerHP.GetComponent<Movement>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        Distance = distance;
        if (Distance <= MinDis )
        {
            rb.simulated = true;
        }
        else
        {
            rb.simulated = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") )
        {
            ObjectPooler.ReturnObjectToPool(gameObject);
        }
        if (collision.CompareTag("Player"))
        {
            
            if(Time.time >= LastDelay + DelayCD && !Stickman.isDashing)
            {
                
                LastDelay = Time.time;
                if (!isDifferent && !DrainStamina)
                {
                    PlayerHP.PlayerHP -= 0.1f;

                    if (WantToBeGoneIfTouchPlayer)
                    {
                        ObjectPooler.ReturnObjectToPool(gameObject);
                        AudioP.PlayOneShot(Hit);
                    }

                }
                else if(isDifferent)
                {
                    Debug.Log("Yo Chat We Hit for" + DMG);
                    PlayerHP.PlayerHP -= DMG;
                    if (WantToBeGoneIfTouchPlayer)
                    {
                        AudioP.PlayOneShot(Hit);

                        ObjectPooler.ReturnObjectToPool(gameObject);
                    }
                }
                else if (DrainStamina && PlayerHP.MegaCharge > 1)
                {
                    Debug.Log("Yo Chat We Drained for" + DMG);
                    PlayerHP.MegaCharge -= DMG;
                    if (WantToBeGoneIfTouchPlayer)
                    {
                        AudioP.PlayOneShot(Hit);

                        ObjectPooler.ReturnObjectToPool(gameObject);
                    }
                }
                else if(Hybrid && PlayerHP.MegaCharge > 1)
                {
                    Debug.Log("Yo Chat We Drained for" + DMG);
                    PlayerHP.MegaCharge -= DMG;
                    PlayerHP.PlayerHP -= DMG;
                    if (WantToBeGoneIfTouchPlayer)
                    {
                        AudioP.PlayOneShot(Hit);

                        ObjectPooler.ReturnObjectToPool(gameObject);
                    }
                }

            }

        }
    }
}
