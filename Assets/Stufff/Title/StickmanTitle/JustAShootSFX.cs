using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustAShootSFX : MonoBehaviour
{

    public AudioSource AudioP;
    public AudioClip Shoot;
    public MainMenu Menu;
    public GameObject fADE;
    public bool Fadeactive;



    // Start is called before the first frame update
    void Start()
    {
        fADE.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    public void Shoots()
    {
        AudioP.PlayOneShot(Shoot);
    }

    public void Transition()
    {
        if (Fadeactive)
        {
            fADE.SetActive(true);
        }
        
    }

    

    public void UNTransition()
    {
        Menu.CanTransition = false;
    }
}
