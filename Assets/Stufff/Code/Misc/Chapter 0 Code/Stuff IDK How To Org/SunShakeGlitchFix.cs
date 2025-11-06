using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunShakeGlitchFix : MonoBehaviour
{

    public Animator Cam;
    public Movement Player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.HeCanMove == true)
        {
            Cam.SetBool("Shake", false);
        }
    }
}
