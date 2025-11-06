using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genetics : MonoBehaviour
{

    public Animator Gen_Anim;
    public LabBreakIn AnimManager;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(AnimManager.BreakIn == true)
        {
            Gen_Anim.SetBool("Turn",true);
        }
    }
}
