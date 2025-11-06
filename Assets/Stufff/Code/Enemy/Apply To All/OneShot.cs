using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OneShot : MonoBehaviour
{

    public GameObject Death;
    public Transform ObjectPosition;
    public bool Alive;
    public float HP;
    public bool CanBeKilledByCollisionW_PLayer;
    public bool CanBeKilledByCollisionW_Book;
    public AudioSource AudioS;
    public AudioClip DeadSFX;
    public bool WantSFX;
    public bool SciencePuddle;
    public GameObject Puddle;
    public bool Want_Tutorial;
    public GameObject Tutorial;
    public AudioSource PopupAUDIOP;
    public AudioClip PopupSFX;
    public Animator HUDAnim;

    public GameObject HUD;
    public TextMeshProUGUI TipUI;

    // Start is called before the first frame update
    void Start()
    {
        if (SciencePuddle)
        {
            Puddle.gameObject.SetActive(false); 
        }
        Alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(HP <= 0)
        {
            if (WantSFX)
            {
                AudioS.pitch = Random.Range(0.9f, 1.2f);
                AudioS.PlayOneShot(DeadSFX);

                
            }
            if (SciencePuddle)
            {
                Puddle.gameObject.SetActive(true);
            }
            Alive = false;
            GameObject effect = Instantiate(Death, ObjectPosition.position, Quaternion.identity);
            Destroy(gameObject);
        }


        
        
        
    }


    private bool AlreadyPlayedPOP_UP;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            HP -= 1;
        }

        if (collision.CompareTag("Player"))
        {

            if (Want_Tutorial)
            {
                if (!AlreadyPlayedPOP_UP)
                {
                    PopupAUDIOP.PlayOneShot(PopupSFX);
                    AlreadyPlayedPOP_UP = true;


                }
                HUD.gameObject.SetActive(true);
                TipUI.text = "To Punch [Right Click]";
            }
            if (CanBeKilledByCollisionW_PLayer)
            {
                HP = 0;
            }
        }
        if (collision.CompareTag("StickMelee"))
        {
            if ((Want_Tutorial))
            {
                HUDAnim.SetTrigger("GoBack");
            }
            HP = 0;
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (CanBeKilledByCollisionW_PLayer)
            { 
                HP = 0;
            }

            if (Want_Tutorial)
            {
                HUD.gameObject.SetActive(true);
                TipUI.text = "To Punch [Right Click]";
            }
        }

        if (collision.gameObject.CompareTag("StickMelee"))
        {
            if ((Want_Tutorial))
            {
                HUDAnim.SetTrigger("GoBack");
            }
            HP = 0;
        }


        if (collision.gameObject.CompareTag("Book"))
        {
            if (CanBeKilledByCollisionW_Book)
            {
                HP = 0;
            }
        }
    }

        




}
