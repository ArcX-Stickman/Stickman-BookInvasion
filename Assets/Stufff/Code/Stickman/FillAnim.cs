using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillAnim : MonoBehaviour
{

    public Health PlayerManager;

    public Animator FA;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerManager.MegaCharge >= 30)
        {
            FA.SetBool("Max", true);
        }
        else
        {
            FA.SetBool("Max", false);
        }


        if(PlayerManager.MegaCharge < 30)
        {
            FA.SetBool("Max", false);
        }
    }
}
