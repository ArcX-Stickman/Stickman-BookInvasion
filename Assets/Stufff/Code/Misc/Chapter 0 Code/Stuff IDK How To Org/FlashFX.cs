using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashFX : MonoBehaviour
{

    public GameObject Flash;
    public AudioSource AudioP;
    public AudioClip FlahsFX;
    public TimeManager TimeManager;
    



    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SFX()
    {
        AudioP.PlayOneShot(FlahsFX);
    }


    public void Disapear()
    {
        
        Flash.gameObject.SetActive(false);
    }

    public void StickColor()
    {
        TimeManager.IsFlash = false;
    }

}
