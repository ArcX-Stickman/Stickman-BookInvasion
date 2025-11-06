using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchTutorial : MonoBehaviour
{

    public GameObject HUD;
    public Movement StickManager;
    public Animator HUDAnim;


    // Start is called before the first frame update
    void Start()
    {
        HUD.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("StickMelee"))
        {
            HUDAnim.SetTrigger("Bye");
            Destroy(gameObject);

        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (StickManager.PunchBool)
            {
                HUDAnim.SetTrigger("Bye");
                Destroy(gameObject);

            }
            HUD.gameObject.SetActive(true);

        }
    }
}
