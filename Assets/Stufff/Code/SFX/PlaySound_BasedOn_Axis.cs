using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound_BasedOn_Axis : MonoBehaviour
{

    [Header("Axis")]
    public bool X;
    public bool Y;
    public bool Z;

    [Header("Info")]
    public float Axis;
    public AudioSource AudioP;
    public AudioClip SFX;

    public bool LessThan;
    public bool GreaterThan;

    private bool Played;

    private void Start()
    {
        if(AudioP == null)
        {
            AudioP = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (!Played)
        {
            if (transform.position.x == Axis && X || transform.position.x <= Axis && X && LessThan || transform.position.x >= Axis && X && GreaterThan)
            {
                AudioP.PlayOneShot(SFX);
                Played = true;
            }
            if (transform.position.y == Axis && Y || transform.position.y <= Axis && Y && LessThan || transform.position.y >= Axis && Y && GreaterThan)
            {
                AudioP.PlayOneShot(SFX);
                Played = true;

            }
            if (transform.position.z == Axis && Z || transform.position.z <= Axis && Z && LessThan || transform.position.z >= Axis && Z && GreaterThan)
            {
                AudioP.PlayOneShot(SFX);
                Played = true;

            }
        }
        else
        {
            this.enabled = false;
        }
       
    }
}
