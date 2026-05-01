using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickCH0_Anim : MonoBehaviour
{

    public bool EmbryPunchShoot;

    private void Start()
    {
        EmbryPunchShoot = false;
    }


    public void EmbryPunchShootA()
    {
        EmbryPunchShoot = true;
    }
}
