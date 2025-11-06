using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperBar : MonoBehaviour
{

    public Health PlayerManager;

    public Image Fill;

    public Slider slide;





   

    // Update is called once per frame
    void Update()
    {
        slide.value = PlayerManager.MegaCharge;


       




    }
}
