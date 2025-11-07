using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StickEnterTree : MonoBehaviour
{
    public bool ImportantTree;
        public GameObject BackflipPopUP;
    public SpriteRenderer Tree;
    public float OpacityThing = 0.5f;
    public float OG_Opacity;
    public Movement PlayerManager;
    public BoxCollider2D BoxCD;
    public bool StickInTree;
    public bool Tut;
    public GameObject BackflipTut;
    public string Txt;
    public KeyCode Kc;

    public bool NeedForCheckPoint_Water;

// sigma 
    private int umIDKMan = 0;
    // Start is called before the first frame update
    void Start()
    {
        PlayerManager = FindAnyObjectByType<Movement>();
        BoxCD = GetComponent<BoxCollider2D>();
        OG_Opacity = Tree.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if(BoxCD != null)
        {
            if (PlayerManager.InStealthKill)
            {
                BoxCD.enabled = false;
            }

            if (!PlayerManager.InStealthKill)
            {
                BoxCD.enabled = true;
            }
        }
       
    }

    private bool AlrTrigAnimTutRest;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if(PlayerManager.Grounded && NeedForCheckPoint_Water)
            {
                PlayerManager.SafePosition = new Vector3(PlayerManager.transform.position.x,PlayerManager.transform.position.y,0);
            }
            StickInTree = true;

            PlayerManager.InStealth = true;
            Color c = Tree.color;
           
            c.a = OpacityThing;
            Tree.color = c;
            if (Tut && BackflipTut !=null)
            {
                if(BackflipPopUP != null)
                {
                    BackflipPopUP.gameObject.SetActive(true);
                }
                BackflipTut.gameObject.SetActive(true);
                Animator anim = BackflipTut.GetComponent<Animator>();
                TextMeshProUGUI tx = BackflipTut.GetComponent<TextMeshProUGUI>();
                tx.text = Txt;
                TutorialKeyInput keyCode = BackflipTut.gameObject.GetComponent<TutorialKeyInput>();
                keyCode.InputS = Kc;
                if (!AlrTrigAnimTutRest)
                {
                    AlrTrigAnimTutRest = true;
                    anim.CrossFade("1", 0, 0);

                }
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StickInTree = false;
            PlayerManager.InStealth = false;


            Color c = Tree.color;

            c.a = OG_Opacity;
            Tree.color = c;

        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if(PlayerManager.Grounded && NeedForCheckPoint_Water)
            {
                PlayerManager.SafePosition = new Vector3(PlayerManager.transform.position.x,PlayerManager.transform.position.y,0);
            }
            StickInTree = true;

            PlayerManager.InStealth = true;
            Color c = Tree.color;

            c.a = OpacityThing;
            Tree.color = c;

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StickInTree = false;

            PlayerManager.InStealth = false;


            Color c = Tree.color;

            c.a = OG_Opacity;
            Tree.color = c;

        }
    }

}
