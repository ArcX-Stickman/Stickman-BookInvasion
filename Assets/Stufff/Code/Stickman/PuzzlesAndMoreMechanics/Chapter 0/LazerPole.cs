using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerPole : MonoBehaviour
{

    public Movement Stickman;
    public AudioSource AudioP;
    public AudioClip Hit;
    public float DistanceFromPlayer;
    public float MinDis;
    public Sprite NoLazer;
    public Sprite WithLazer;
    public SpriteRenderer SpriteRenderer;
    private bool AlrHit;
    private BoxCollider2D Collider;
    public GameObject Lazer1;
    public GameObject Lazer2;
    public Animator ScienceAnimator;
    public bool LazersUp;
    public bool Player_Rush_Start;
    // Start is called before the first frame update
    void Start()
    {
        Collider.isTrigger = true;
        SpriteRenderer.sprite = NoLazer;
        Lazer1.gameObject.SetActive(false);
        Lazer2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Collider = GetComponent<BoxCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        DistanceFromPlayer = Vector2.Distance(transform.position, Stickman.gameObject.transform.position);
        if(DistanceFromPlayer <= MinDis)
        {
            ScienceAnimator.SetTrigger("Push");
        }
        if (LazersUp)
        {
            SpriteRenderer.sprite = WithLazer;

        }
        if (SpriteRenderer.sprite == WithLazer)
        {
            Lazer1.gameObject.SetActive(true);
            Lazer2.gameObject.SetActive(true);
        }
        else
        {
            Lazer1.gameObject.SetActive(false);
            Lazer2.gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!AlrHit && !Stickman.isSliding && LazersUp)
            {
                Stickman.StickHP.PlayerHP = 0;
                AudioP.PlayOneShot(Hit);
            }
            if (Stickman.isSliding)
            {
                if (!Player_Rush_Start)
                {
                    Player_Rush_Start = true;
                    Stickman.SlideCancelSpeed += 100;
                }
            
            }
         
        }
        if (collision.CompareTag("Book"))
        {
            Destroy(collision.gameObject);

        }
    }
}
