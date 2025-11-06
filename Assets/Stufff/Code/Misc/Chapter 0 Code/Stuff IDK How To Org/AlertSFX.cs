using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSFX : MonoBehaviour
{

    public AudioSource AudioP;
    public AudioClip Alert;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AlertSFXPLay()
    {
        AudioP.PlayOneShot(Alert);
    }

    public void JusAudioP()
    {
        AudioP.Play();
    }
}
